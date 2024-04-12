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
    public static Texture2D PlainTexture {get; protected set;} = null!; // 1x1 white pixel texture
    public SamplerState SpritebatchSamplerState {get; protected set;} = SamplerState.PointClamp; // Determines if the spritebatch uses the PointClamp SamplerState

    // Fixed updates (aka Physics)
    protected const float TargetFixedFrameRate = 90f;
    protected float FixedDeltaTime = 1f / TargetFixedFrameRate;
    protected float SinceLastFixedUpdate = 0f;

    // Cursor & Camera
    public Rectangle Cursor;


    public GameInstance()
    {
        _graphics = new GraphicsDeviceManager(this);
        FileManager = new FileManager();
        ScreenManager = new ScreenManager(this);
        ComponentManager = new ComponentManager(this);
        PhysicsEngine = new PhysicsEngine(this);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1f / 90);

        Window.ClientSizeChanged += Window_ClientSizeChanged;
    }
    #region Methods
    protected override void Initialize() // Startup
    {
        base.Initialize();
    }
    protected override void LoadContent() // Load method
    {
        base.LoadContent();

        // Set Game viewport
        GraphicsDevice.Viewport = new Viewport(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        SpriteBatch = new SpriteBatch(GraphicsDevice);
        PlainTexture = new Texture2D(GraphicsDevice, 1, 1);
        PlainTexture.SetData(new[] {Color.White});
    }
    protected override void UnloadContent() // Unload method
    {
        base.UnloadContent();
    }
    protected override void Update(GameTime gameTime) // Update method
    {
        // Update Cursor position
        MouseState mouseState = Mouse.GetState();
        Cursor.X = mouseState.X; Cursor.Y = mouseState.Y;

        // Update Screens/Components
        ScreenManager.Update(gameTime);
        ComponentManager.Update(gameTime);

        // Fixed update call cycle
        SinceLastFixedUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (SinceLastFixedUpdate >= FixedDeltaTime)
        {
            // Count number of cycles.
            int cycles = (int)(SinceLastFixedUpdate / FixedDeltaTime);
            
            // Trigger fixed update
            while (cycles > 0)
            {
                FixedUpdate(gameTime);
                SinceLastFixedUpdate -= FixedDeltaTime;
                cycles--;
            }
        }
        base.Update(gameTime);
    }

    /// <summary> Calls the physics engine. </summary>
    protected virtual void FixedUpdate(GameTime gameTime) // Fixed update method
    {
        // This method is for calling the physics engine at a rate independent from the Update/Draw framerate.
        PhysicsEngine.FixedUpdate(gameTime);
    }

    protected override void Draw(GameTime gameTime) // Draw method
    {
        base.Draw(gameTime);
        ScreenManager.Draw(gameTime);
        ComponentManager.Draw(gameTime);
    }

    /// <summary> Triggers whenever the game window is resized by the user. </summary>
    protected virtual void Window_ClientSizeChanged(object? sender, EventArgs args) // Window resize method
    {
        // Call ComponentManager to rescale components.
        ComponentManager.Resize();
    }

    /// <summary> Triggers whenever the game is closed. </summary>
    protected override void OnExiting(object sender, EventArgs args) // Exiting method
    {
        base.OnExiting(sender, args);
    }

    // Screen
    /// <returns> The width and height of the screen. </returns>
    public int[] GetScreenSize()
    {
        int[] size = new int[2];
        size[0] = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        size[1] = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        return size;
    }
    
    // Window
    /// <returns> The width and height of the game window. </returns>
    public int[] GetWindowSize()
    {
        int[] size = new int[2];
        size[0] = Window.ClientBounds.Width; size[1] = Window.ClientBounds.Height;
        return size;
    }

    // Buffer
    /// <summary> Sets the graphics backbuffer. </summary>
    public void SetBufferSize(int width, int height)
    {
        _graphics.PreferredBackBufferWidth= width; _graphics.PreferredBackBufferHeight = height;
        _graphics.ApplyChanges();

        ComponentManager.UpdateWindowSize();
    }
    /// <returns> The width and height of the graphics backbuffer. </returns>
    public int[] GetBufferSize()
    {
        int[] size = new int[2];
        size[0] = _graphics.PreferredBackBufferWidth; size[1] = _graphics.PreferredBackBufferHeight;
        return size;
    }

    // Framerate
    /// <summary> Sets the target framerate. </summary>
    public void SetFramerate(int framerate)
    {
        TargetElapsedTime = TimeSpan.FromSeconds(1f / framerate);
    }
    #endregion
}