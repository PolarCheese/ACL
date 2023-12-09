using ACL.UI;
using ACL.Save;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL;

public class GameInstance : Game
{
    // Managers
    private readonly GraphicsDeviceManager _graphics;
    internal static SpriteBatch spriteBatch = null!;
    protected internal SaveManager saveManager {get; private set;} = null!;
    protected internal ScreenManager screenManager {get; private set;} = null!;
    protected internal ComponentManager componentManager {get; private set;} = null!;
    //internal static PhysicsEngine physicsEngine;
    
    // Properties
    public static int WindowWidth { get; private set; }
    public static int WindowHeight { get; private set; }
    public static int WindowRatio { get; private set; }
    public static int LargestDim { get; private set;}

    // Fixed update system
    private const float TargetFixedFrameRate = 90f;
    private float FixedDeltaTime = 1f / TargetFixedFrameRate;
    private float SinceLastFixedUpdate = 0f;
    public event EventHandler FixedUpdateEvent = null!;
    internal static Rectangle PlayerCursor;

    // Current Instances
    public static GameInstance CurrentGameInstance {get; private set;} = null!;

    public GameInstance()
    {
        CurrentGameInstance ??= this;

        _graphics = new GraphicsDeviceManager(CurrentGameInstance);
        saveManager ??= new SaveManager();
        //physicsEngine = new PhysicsEngine();
        screenManager ??= new ScreenManager(CurrentGameInstance);
        componentManager ??= new ComponentManager();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
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
    }
    protected override void UnloadContent() // Unload method
    {
        base.UnloadContent();
    }
    protected override void Update(GameTime gameTime) // Update method
    {
        SinceLastFixedUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (SinceLastFixedUpdate >= FixedDeltaTime)
        {
            // Trigger fixed update
            FixedUpdate(gameTime);
            FixedUpdateEvent.Invoke(this, EventArgs.Empty);
            SinceLastFixedUpdate -= FixedDeltaTime;
        }
        base.Update(gameTime);
    }

    protected virtual void FixedUpdate(GameTime gameTime) // Fixed update method
    {
        // ..
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
    #endregion
}