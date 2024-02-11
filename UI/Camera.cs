using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class Camera
    {
        public GameInstance Game;
        protected Viewport Viewport => Game.GraphicsDevice.Viewport; // Game viewport
        public Rectangle Cursor; // Cursor relative to the camera viewport
        public List<Component> SubComponents {get; set;} = new List<Component>();

        #region Properties
        public bool Enabled {get; set;} = true;
        public float Zoom {get; set;} = 1f; // Zoom level
        public QuadVector Position {get; set;} = new(0, 0, 0, 0);
        public Component?[] Target {get; set;}= new Component[1]; // If not null, camera will follow the "target" component
        public Matrix Transform {get; protected set;} // Camera Matrix
        #endregion

        public Camera(GameInstance gameInstance)
        {
            Game = gameInstance;
        }

        #region Methods
        public void Update()
        {
            // Calculate cursor position from camera's perspective.
            Vector2 TransformedPosition = Vector2.Transform(new(Game.Cursor.X, Game.Cursor.Y), Transform);
            Cursor.X = (int)TransformedPosition.X; Cursor.Y = (int)TransformedPosition.Y;

            // Check for target
            if (Target[0] != null)
            {
                Position = new(Target[0]!.Position);
            }

            // Update Transform
            Transform = Matrix.CreateTranslation(new Vector3(-Position.ToVector2(Viewport.Bounds), 0f)) *
                        Matrix.CreateScale(Zoom) *
                        Matrix.CreateTranslation(new Vector3(Viewport.Width * 0.5f, Viewport.Height * 0.5f, 0f));
        }
        
        public void SetTarget(Component Component) // Set the camera to follow a specific component
        {
            Target[0] = Component;
        }

        public void RemoveTarget() // Set Target to null
        {
            Target[0] = null;
        }
        #endregion
    }
}
