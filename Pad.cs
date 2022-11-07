using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pong;

public enum Side
{
    Left,
    Right
}

public enum LastDirection
{
    Up,
    Down,
    None
} 

public class Pad
{
    private readonly Side _padSide;
    private float _padSpeed;
    private Texture2D _padTexture;
    private Rectangle _padRectangle;
    private float _bWidth;
    private float _bHeight;
    private LastDirection _lastDirection;

    public Pad(float bWidth, float bHeight, Side side)
    {
        _bWidth = bWidth;
        _bHeight = bHeight;
        _padSide = side;
        _padSpeed = 3f;
        _lastDirection = LastDirection.None;
        if (_padSide == Side.Left)
        {
            _padRectangle = new Rectangle(
                (int)_bWidth / 50,
                (int)(_bHeight / 2 - (_bHeight / 7) /2), 
                (int)_bWidth / 50, 
                (int)_bHeight / 7);
        }
        else
        {
            _padRectangle = new Rectangle(
                (int)_bWidth - ((int)_bWidth / 50) * 2,
                (int)(_bHeight / 2 - (_bHeight / 7) /2), 
                (int)_bWidth / 50, 
                (int)_bHeight / 7);
        }
    }

    public void Resize(float bWidth, float bHeight)
    {
        _bWidth = bWidth;
        _bHeight = bHeight;
        if (_padSide == Side.Right)
        {
            _padRectangle.X = (int)_bWidth - ((int)_bWidth / 50) * 2;
            _padRectangle.Y = (int)(_bHeight / 2 - (_bHeight / 7) /2);
            _padRectangle.Width = (int)_bWidth / 50;
            _padRectangle.Height = (int)_bHeight / 7;
        }
        else
        {
            _padRectangle.X = (int)_bWidth / 50;
            _padRectangle.Y = (int)(_bHeight / 2 - (_bHeight / 7) /2);
            _padRectangle.Width = (int)_bWidth / 50;
            _padRectangle.Height = (int)_bHeight / 7;
        }
    }

    public void ChangeSpeed(int newSpeed)
    {
        _padSpeed = newSpeed;
    }

    public void SetTexture(Texture2D padTexture)
    {
        _padTexture = padTexture;
    }

    public void MoveUp()
    {
        _lastDirection = LastDirection.Up;
        _padRectangle.Y = _padRectangle.Y - (int) _padSpeed;
        if (_padRectangle.Y <= 0) {
            _padRectangle.Y = 0;
        }
    }
    
    public void MoveDown()
    {
        _lastDirection = LastDirection.Down;
        _padRectangle.Y = _padRectangle.Y + (int) _padSpeed;
        if (_padRectangle.Y + _padRectangle.Height > _bHeight) {
            _padRectangle.Y = (int) _bHeight - _padRectangle.Height;
        }
    }

    public void DrawPad(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_padTexture, _padRectangle, Color.White);
    }

    public int GetPadSidePos()
    {
        if (_padSide == Side.Left)
        {
            return _padRectangle.Width * 2;
        }
        return _padRectangle.X;
    }

    public Rectangle GetPadRectangle()
    {
        return _padRectangle;
    }

    public LastDirection GetLastDirection()
    {
        return _lastDirection;
    }

    public void FollowBall(int ballY)
    {
        // move right pad up
        if (ballY < _padRectangle.Y + _padRectangle.Height/2f)
        {
            MoveUp();
        }
        // move right pad down
        if (ballY > _padRectangle.Y + _padRectangle.Height / 2f)
        {
            MoveDown();
        }
    }
    
    public void DisposePad()
    {
        _padTexture.Dispose();
    }
    
}