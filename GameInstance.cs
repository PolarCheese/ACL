using ACL.UI;
using ACL.IO;
using ACL.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL;

public class GameInstance : Game
{
    // Managers
    protected readonly GraphicsDeviceManager _graphics;
    protected internal SpriteBatch SpriteBatch {get; protected set;} = null!;
    public FileManager FileManager {get; protected set;} = null!;
    public ScreenManager ScreenManager {get; protected set;} = null!;
    public ComponentManager ComponentManager {get; protected set;} = null!;
    public PhysicsEngine PhysicsEngine {get; protected set;} = null!;
    
    // Properties
    public static Texture2D PlainTexture { get; protected set; } = null!; // 1x1 white pixel texture
    public SamplerState SpritebatchSamplerState {get; protected set; } = SamplerState.PointClamp; // Determines if the spritebatch uses the PointClamp SamplerState

    // Fixed updates (aka Physics)
    protected const float TargetFixedFrameRate = 90f;
    protected float FixedDeltaTime = 1f / TargetFixedFrameRate;
    protected float SinceLastFixedUpdate = 0f;

    // Cursor & Camera
    public Rectangle PlayerCursor;
    public Camera Camera;

    // Current Instances
    public GameInstance CurrentGameInstance {get; private set;} = null!;

    public GameInstance()
    {
        CurrentGameInstance = this;

        _graphics = new GraphicsDeviceManager(CurrentGameInstance);
        FileManager = new FileManager();
        ScreenManager = new ScreenManager(CurrentGameInstance);
        ComponentManager = new ComponentManager(CurrentGameInstance);
        PhysicsEngine = new PhysicsEngine(CurrentGameInstance);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1f / 90);
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
        // Set Game viewport
        GraphicsDevice.Viewport = new Viewport(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        Camera = new Camera(CurrentGameInstance);

        SpriteBatch = new SpriteBatch(GraphicsDevice);
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
        ScreenManager.Update(gameTime);
        ComponentManager.Update(gameTime);
        SinceLastFixedUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (SinceLastFixedUpdate >= FixedDeltaTime)
        {
            // Trigger fixed update
            FixedUpdate(gameTime);
        }
        Camera.Update();
        base.Update(gameTime);
    }

    protected virtual void FixedUpdate(GameTime gameTime) // Fixed update method
    {
        // This method is for calling the physics engine at a rate independent from the Update/Draw framerate.
        PhysicsEngine.FixedUpdate();
    }

    protected override void Draw(GameTime gameTime) // Draw method
    {
        base.Draw(gameTime);
        SpriteBatch.Begin(samplerState: SpritebatchSamplerState, transformMatrix: Camera.Transform);
        ScreenManager.Draw(gameTime);
        ComponentManager.Draw(gameTime);
        SpriteBatch.End();
    }

    protected override void OnExiting(object sender, EventArgs args) // Exiting method
    {
        base.OnExiting(sender, args);
    }

    // Resolution
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

    // Cursor
    public void GetPlayerCursor(MouseState mouseState)
    {
        PlayerCursor.X = mouseState.X; PlayerCursor.Y = mouseState.Y;
    }

    // Framerate
    public void SetFramerate(int framerate)
    {
        TargetElapsedTime = TimeSpan.FromSeconds(1f / framerate);
    }
    #endregion
}