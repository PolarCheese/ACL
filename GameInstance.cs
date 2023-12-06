using ACL.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL;

public class GameInstance : Game
{
    // Managers
    private readonly GraphicsDeviceManager? _graphics;
    //private readonly ScreenManager _screenManager;
    //internal readonly ComponentManager componentManager;
    //internal static PhysicsEngine physicsEngine;
    internal static SpriteBatch? spriteBatch;
    //public static SettingsClass Settings;

    // Properties
    public static int WindowWidth { get; private set; }
    public static int WindowHeight { get; private set; }
    public static int WindowRatio { get; private set; }
    public static int LargestDim { get; private set;}

    // Fixed update system
    private const float TargetFixedFrameRate = 90f;
    private float FixedDeltaTime = 1f / TargetFixedFrameRate;
    private float SinceLastFixedUpdate = 0f;
    public event EventHandler? FixedUpdateEvent;

    //internal static Camera2D PlayerCamera; // 2D Camera
    internal static Rectangle PlayerCursor; // Player Cursor

    // Current Instances
    public static GameInstance? CurrentGameInstance {get; private set;}
    internal static Screen? CurrentScreen {get; private set;}

    public GameInstance()
    {
        CurrentGameInstance = this;

        _graphics = new GraphicsDeviceManager(CurrentGameInstance);
        Content.RootDirectory = "Content";
        //Settings = EnsureJson<SettingsClass>("Settings.json");
        IsMouseVisible = true;
        //TargetElapsedTime = TimeSpan.FromSeconds(1f / Settings.FPSCap);

        //physicsEngine = new PhysicsEngine();
        //ComponentManager = new ComponentManager();
        //_screenManager = Components.Add<ScreenManager>();
    }
    #region Methods
    protected override void Initialize() // Startup
    {
        // Window Properties
        Window.AllowUserResizing = false;
        Window.AllowAltF4 = true;
        Window.Title = "Game Window";

        /*
        PlayerCamera = new Camera2D()
        {
            Zoom = 1f,
            Position = Vector2.Zero,
        };
        */
        // Apply Graphical settings
        //LoadSettings();

        base.Initialize();
    }
    protected override void LoadContent() // Load method
    {
        base.LoadContent();

        spriteBatch = new SpriteBatch(GraphicsDevice);

        // Make sure the Camera is set 
        /*
        GraphicsDevice.Viewport = new Viewport(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        PlayerCamera.SetViewport(GraphicsDevice.Viewport);

        LoadScreen(new TitleScreen(this), new FadeTransition(GraphicsDevice, Color.Black, 2f));
        */
    }
    protected override void UnloadContent() // Unload method
    {
        // SaveJson<SettingsClass>("Settings.json", Settings); // Save when unloaded

        base.UnloadContent();
    }
    protected override void Update(GameTime gameTime) // Update method
    {
        //CalculatePlayerCursor(Mouse.GetState());

        SinceLastFixedUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (SinceLastFixedUpdate >= FixedDeltaTime)
        {
            // Trigger fixed update
            FixedUpdate(gameTime);
            FixedUpdateEvent?.Invoke(this, EventArgs.Empty);
            SinceLastFixedUpdate -= FixedDeltaTime;
        }
        /*
        PlayerCamera.Update();
        
        if (Keyboard.GetState().IsKeyDown(Keys.R))
        {
            ReloadSettings(); // Reload settings
        }

        if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
        {
            if (PlayerCamera.Zoom < 1.25f)
            {
                PlayerCamera.Zoom += 0.01f; // Increase Zoom
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
        {
            if (PlayerCamera.Zoom > 0.5f)
            {
                PlayerCamera.Zoom -= 0.01f; // Decrease Zoom
            }
        } */
        base.Update(gameTime);
    }

    protected virtual void FixedUpdate(GameTime gameTime) // Fixed update method
    {
        // ..
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
    }
    /*
    public void LoadScreen<TScreen>(TScreen screen, FadeTransition transition)
        where TScreen : BetterScreen
    {
        _screenManager.LoadScreen(screen, transition);
        CurrentScreen = screen;
    }

    private void CalculatePlayerCursor(MouseState mouseState)
    {
        // Calculates the Mouse position every frame to work with the 2D camera.
        Matrix inverseTransform = Matrix.Invert(PlayerCamera.GetViewMatrix());
        Vector2 MousePositionVector2 = new Vector2(mouseState.X, mouseState.Y);
        Vector2 transformedCursorPosition = Vector2.Transform(MousePositionVector2, inverseTransform);

        PlayerCursor.X = (int)transformedCursorPosition.X; PlayerCursor.Y = (int)transformedCursorPosition.Y;
    } */
    #endregion
    #region Settings
    /*
    public void LoadSettings()
    {
        TargetElapsedTime = TimeSpan.FromSeconds(1f / Settings.FPSCap);
        IsFixedTimeStep = Settings.IsFixedTimeStep;
        _graphics.SynchronizeWithVerticalRetrace = Settings.IsVSync;
        _graphics.IsFullScreen = Settings.IsFullscreen;
        _graphics.HardwareModeSwitch = Settings.IsBorderless;
        if (Settings.Width < 600)
        {
            _graphics.PreferredBackBufferWidth = 600;
        }
        else
        {
            _graphics.PreferredBackBufferWidth = Settings.Width;
        }
        if (Settings.Height < 600)
        {
            _graphics.PreferredBackBufferHeight = 600;
        }
        else
        {
            _graphics.PreferredBackBufferHeight = Settings.Height;
        }
        if (!Settings.IsFullscreen)
        {
            _graphics.PreferredBackBufferWidth = Settings.Width;
            _graphics.PreferredBackBufferHeight = Settings.Height;
            WindowWidth = Settings.Width;
            WindowHeight = Settings.Height;
        }
        else
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            WindowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            WindowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }
        GraphicsDevice.Viewport = new Viewport(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        PlayerCamera.SetViewport(GraphicsDevice.Viewport);
        _graphics.ApplyChanges(); 
        WindowRatio = WindowWidth / WindowHeight;
        LargestDim = Math.Max(WindowWidth, WindowHeight);
    }
    public static void ToggleFullscreen()
    {
        Settings.IsFullscreen = !Settings.IsFullscreen;
    }
    public static void ToggleBorderless()
    {
        // Make window borderless

        Settings.IsBorderless = !Settings.IsBorderless;
        if (!Settings.IsFullscreen && Settings.IsBorderless)
        {
            Settings.IsFullscreen = true;
        }
    }
    public static void ToggleVSync()
    {
        Settings.IsVSync = !Settings.IsVSync;
    }
    public static void SetViewportSettingsSize(int width, int height)
    {
        Settings.Width = width;
        Settings.Height = height;
    } */

    // add settings saving ig.
    #endregion
}