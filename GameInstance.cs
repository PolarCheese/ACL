using ACL.UI;
using ACL.Save;
using ACL.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL;

public class GameInstance : Game
{
    // Managers
    protected readonly GraphicsDeviceManager _graphics;
    protected internal SpriteBatch spriteBatch {get; protected set;} = null!;
    protected internal SaveManager saveManager {get; protected set;} = null!;
    protected internal ScreenManager screenManager {get; protected set;} = null!;
    protected internal ComponentManager componentManager {get; protected set;} = null!;
    protected internal PhysicsEngine physicsEngine {get; protected set;} = null!;
    
    // Properties
    public static Texture2D? PlainTexture { get; protected set; } // 1x1 white pixel texture
    public SamplerState SpritebatchSamplerState {get; protected set; } = SamplerState.PointClamp; // Determines if the spritebatch uses the PointClamp SamplerState

    // Fixed update system
    protected const float TargetFixedFrameRate = 90f;
    protected float FixedDeltaTime = 1f / TargetFixedFrameRate;
    protected float SinceLastFixedUpdate = 0f;
    public event EventHandler FixedUpdateEvent = null!;
    public Rectangle PlayerCursor;

    // Current Instances
    public GameInstance CurrentGameInstance {get; private set;} = null!;

    public GameInstance()
    {
        CurrentGameInstance ??= this;

        _graphics = new GraphicsDeviceManager(CurrentGameInstance);
        saveManager ??= new SaveManager();
        screenManager ??= new ScreenManager(CurrentGameInstance);
        componentManager ??= new ComponentManager(CurrentGameInstance);
        physicsEngine = new PhysicsEngine(CurrentGameInstance);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1f / 90);
        //TargetElapsedTime = TimeSpan.FromSeconds(1f / Settings.FPSCap);

        
    }
    #region Methods
    protected override void Initialize() // Startup
    {
        // Window Properties
        Window.AllowUserResizing = false;
        Window.AllowAltF4 = true;
        Window.Title = "Game Window";

        base.Initialize();
    }
    protected override void LoadContent() // Load method
    {
        base.LoadContent();
        GraphicsDevice.Viewport = new Viewport(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        spriteBatch ??= new SpriteBatch(GraphicsDevice);
        PlainTexture = new Texture2D(GraphicsDevice, 1, 1);
        PlainTexture.SetData(new[] { Color.White });
    }
    protected override void UnloadContent() // Unload method
    {
        base.UnloadContent();
    }
    protected override void Update(GameTime gameTime) // Update method
    {
        GetPlayerCursor(Mouse.GetState());
        SinceLastFixedUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (SinceLastFixedUpdate >= FixedDeltaTime)
        {
            // Trigger fixed update
            FixedUpdate(gameTime);
            FixedUpdateEvent.Invoke(this, EventArgs.Empty);
            SinceLastFixedUpdate -= FixedDeltaTime;
        }
        screenManager.Update(gameTime);
        componentManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected virtual void FixedUpdate(GameTime gameTime) // Fixed update method
    {
        // ..
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        screenManager.Draw(gameTime);
        componentManager.Draw(gameTime);
        spriteBatch.End();
    }

    public void SetWindowResolution(int Width, int Height)
    {
        if (Width > 0 && Height > 0)
        {
            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            _graphics.ApplyChanges();
        }
    }

    public int[] GetWindowResolution()
    {
        int [] resolution = new int[2];
        resolution[0] = GraphicsDevice.Viewport.Width; resolution[1] = GraphicsDevice.Viewport.Height;
        return resolution;
    }

    public void GetPlayerCursor(MouseState mouseState)
    {
        PlayerCursor.X = mouseState.X; PlayerCursor.Y = mouseState.Y;
    }
    #endregion
}