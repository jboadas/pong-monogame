using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pong;

public class Header
{
    private int _bWidth;
    private int _bHeight;
    private Texture2D _headerTexture;
    private Rectangle _headerRectangle;
    private SpriteFont _scoreFont;

    public Header(int bWidth, int bheight)
    {
        _bWidth = bWidth;
        _bHeight = bheight;
        _headerRectangle = new Rectangle(
            0,
            0,
            _bWidth,
            _bHeight / 10);
    }
    public void SetTexture(Texture2D ballTexture)
    {
        _headerTexture = ballTexture;
    }
    public void SetFont(SpriteFont scoreFont)
    {
        _scoreFont = scoreFont;
    }
    public void DisposeHeader()
    {
        _headerTexture.Dispose();
    }
    public void DrawHeader(SpriteBatch spriteBatch, string scoreLeft, string scoreRight)
    {
        spriteBatch.Draw(_headerTexture, _headerRectangle, Color.White);
        spriteBatch.DrawString(_scoreFont, scoreLeft, new Vector2((float) _bWidth/4, 5f), Color.Green);
        spriteBatch.DrawString(_scoreFont, scoreRight, new Vector2((_bWidth / 4) * 3, 5f), Color.Green);
    }
    public void Resize(int bWidth, int bHeight)
    {
        _bWidth = bWidth;
        _bHeight = bHeight;
        _headerRectangle.X = 0;
        _headerRectangle.Y = 0;
        _headerRectangle.Width = _bWidth;
        _headerRectangle.Height = _bHeight / 10;
    }
}