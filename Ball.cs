using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pong;

public class Ball
{
    private Texture2D _ballTexture;
    private Rectangle _ballRectangle;
    private float _ballSpeedX;
    private float _ballSpeedY;
    private float _bWidth;
    private float _bHeight;

    public Ball(float bWidth, float bHeight)
    {
        _bWidth = bWidth;
        _bHeight = bHeight;
        _ballSpeedX = 8f;
        _ballSpeedY = 8f;
        _ballRectangle = new Rectangle(
            (int) _bWidth / 2,
            (int) _bHeight / 2,
            (int) _bWidth / 36,
            (int) _bWidth / 36);
    }

    public void SetTexture(Texture2D ballTexture)
    {
        _ballTexture = ballTexture;
    }

    public void Resize(float bWidth, float bHeight)
    {
        var bally = this._bHeight / _ballRectangle.Y;
        var ballx = this._bWidth / _ballRectangle.X;
        
        _bWidth = bWidth;
        _bHeight = bHeight;
        _ballRectangle.X = (int) (_bWidth / ballx);
        _ballRectangle.Y = (int) (_bHeight / bally);
        _ballRectangle.Width = (int) _bWidth / 36;
        _ballRectangle.Height = (int) _bWidth / 36;
    }
    
    
    public Side? MoveBall()
    {
        _ballRectangle.X = _ballRectangle.X + (int) _ballSpeedX;
        _ballRectangle.Y = _ballRectangle.Y + (int) _ballSpeedY;
        // down and top wall 
        if (_ballRectangle.Y > _bHeight - _ballRectangle.Height) {
            _ballSpeedY = -_ballSpeedY;
        }else if (_ballRectangle.Y < 0) {
            _ballSpeedY = -_ballSpeedY;
        }
        // right wall collision
        if(_ballRectangle.X > _bWidth - _ballRectangle.Width){
            _ballSpeedX = -_ballSpeedX;
            return Side.Right;
        }// left wall collision
        if(_ballRectangle.X <= 0) {
            _ballSpeedX = -_ballSpeedX;
            return Side.Left;
        }
        return null;
    }
    
    public void CheckPadCollision(Pad pad)
    {
        if (_ballRectangle.Intersects(pad.GetPadRectangle()))
        {
            if (pad.GetPadSidePos() > (_bWidth / 2))
            {
                _ballRectangle.X = pad.GetPadSidePos() - _ballRectangle.Width;
            }
            else
            {
                _ballRectangle.X = pad.GetPadSidePos();
            }
            _ballSpeedX = -_ballSpeedX;
            if (pad.GetLastDirection() == LastDirection.None)
            {
                _ballSpeedY = -_ballSpeedY;
            }
            if (pad.GetLastDirection() == LastDirection.Down && _ballSpeedY < 0)
            {
                _ballSpeedY = -_ballSpeedY;
            }
            if (pad.GetLastDirection() == LastDirection.Up && _ballSpeedY > 0)
            {
                _ballSpeedY = -_ballSpeedY;
            }
            
        }
    }
    
    
    public void DrawBall(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_ballTexture, _ballRectangle, Color.White);
    }

    public int GetBallY()
    {
        return _ballRectangle.Y;
    }

    public void DisposeBall()
    {
        _ballTexture.Dispose();
    }

}