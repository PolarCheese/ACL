using ACL.UI;
using ACL.Save;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL;

public class GameInstance : Game
{
    // Managers
    private readonly GraphicsDeviceManager? _graphics;
    public static SaveManager? SaveManager;
    //private readonly ScreenManager _screenManager;
    //internal readonly ComponentManager componentManager;
    //internal static PhysicsEngine physicsEngine;
    
    internal static SpriteBatch? spriteBatch;

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
    internal static Rectangle PlayerCursor;

    // Current Instances
    public static GameInstance? CurrentGameInstance {get; private set;}
    internal static Screen? CurrentScreen {get; private set;}

    public GameInstance()
    {
        CurrentGameInstance = this;

        _graphics = new GraphicsDeviceManager(CurrentGameInstance);
        SaveManager = new SaveManager();
        //physicsEngine = new PhysicsEngine();
        //ComponentManager = new ComponentManager();
        //_screenManager = Components.Add<ScreenManager>();
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

        spriteBatch = new SpriteBatch(GraphicsDevice);
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
            FixedUpdateEvent?.Invoke(this, EventArgs.Empty);
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
        GraphicsDevice.Clear(Color.Blue);

        base.Draw(gameTime);
    }
    #endregion
}