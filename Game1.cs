using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pong;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private const int WIDTH = 800;
    private const int HEIGHT = 600;
    private SpriteBatch _spriteBatch;
    private int _scoreLeft, _scoreRight;
    private int _bHeight;
    private int _bWidth;
    private readonly GameWindow _window;
    private Ball _ball;
    private Pad _leftPad;
    private Pad _rightPad;
    private Header _header;
    private RenderTarget2D _renderTarget;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = WIDTH;
        _graphics.PreferredBackBufferHeight = HEIGHT;
        _window = Window;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //60)        
    }

    protected override void Initialize()
    {
        _bHeight = _graphics.PreferredBackBufferHeight;
        _bWidth = _graphics.PreferredBackBufferWidth;
        _scoreLeft = 0;
        _scoreRight = 0;
        _graphics.IsFullScreen = false;
        _graphics.PreferMultiSampling = true;
        _window.IsBorderless = false;
        _header = new Header(_bWidth, _bHeight);
        _leftPad = new Pad(_bWidth, _bHeight, Side.Left);
        _rightPad = new Pad(_bWidth, _bHeight, Side.Right);
        _ball = new Ball(_bWidth, _bHeight);
        _renderTarget = new RenderTarget2D(GraphicsDevice, _bWidth, _bHeight);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _ball.SetTexture(Content.Load<Texture2D>("ball"));
        _leftPad.SetTexture(Content.Load<Texture2D>("paddle"));
        _rightPad.SetTexture(Content.Load<Texture2D>("paddle"));
        _header.SetTexture(Content.Load<Texture2D>("header"));
        _header.SetFont(Content.Load<SpriteFont>("defaultFont"));
    }

    protected override void UnloadContent()
    {
        _spriteBatch.Dispose();
        _leftPad.DisposePad();
        _rightPad.DisposePad();
        _ball.DisposeBall();
        _header.DisposeHeader();
    }

    private void Input()
    {
        var kstate = Keyboard.GetState();
        if (kstate.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        if (kstate.IsKeyDown(Keys.W))
        {
            _leftPad.MoveUp();
        }

        if (kstate.IsKeyDown(Keys.S))
        {
            _leftPad.MoveDown();
        }

        if (kstate.IsKeyDown(Keys.F))
        {
            if (!_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                _bWidth = _graphics.PreferredBackBufferWidth;
                _bHeight = _graphics.PreferredBackBufferHeight;
                _leftPad.Resize(_bWidth, _bHeight);
                _rightPad.Resize(_bWidth, _bHeight);
                _ball.Resize(_bWidth, _bHeight);
                _header.Resize(_bWidth, _bHeight);
                _renderTarget = new RenderTarget2D(GraphicsDevice, _bWidth, _bHeight);
                _graphics.HardwareModeSwitch = false;
                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();
            }
        }

        if (kstate.IsKeyDown(Keys.R))
        {
            if (_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferWidth = WIDTH;
                _graphics.PreferredBackBufferHeight = HEIGHT;
                _bWidth = _graphics.PreferredBackBufferWidth;
                _bHeight = _graphics.PreferredBackBufferHeight;
                _leftPad.Resize(_bWidth, _bHeight);
                _rightPad.Resize(_bWidth, _bHeight);
                _header.Resize(_bWidth, _bHeight);
                _ball.Resize(_bWidth, _bHeight);
                _renderTarget = new RenderTarget2D(GraphicsDevice, _bWidth, _bHeight);
                _graphics.HardwareModeSwitch = false;
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
            }
        }
    }
    
    protected override void Update(GameTime gameTime)
    {
        Input();

        // move ball
        var score = _ball.MoveBall();
        switch (score)
        {
            case Side.Left:
                _scoreRight += 1;
                break;
            case Side.Right:
                _scoreLeft += 1;
                break;
            case null:
                break;
        }
        _rightPad.FollowBall(_ball.GetBallY());
        _ball.CheckPadCollision(_rightPad);
        _ball.CheckPadCollision(_leftPad);
        base.Update(gameTime);
    }

    private void DrawToTexture()
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.DarkGray);
        _spriteBatch.Begin();
        _header.DrawHeader(_spriteBatch, _scoreLeft.ToString(), _scoreRight.ToString());
        _leftPad.DrawPad(_spriteBatch);
        _rightPad.DrawPad(_spriteBatch);
        _ball.DrawBall(_spriteBatch);
        _spriteBatch.End();
        GraphicsDevice.SetRenderTarget(null);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        DrawToTexture();
        _spriteBatch.Begin();
        _spriteBatch.Draw(_renderTarget, new Vector2(0, 0), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
