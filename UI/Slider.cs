using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL.UI
{
    public class Slider : Component
    {
        #region Fields

        protected MouseState _currentMouseState;
        protected MouseState _previousMouseState;

        protected Rectangle _currentMouse;
        protected Rectangle _previousMouse;

        public bool _isHovering { get; protected set; }

        #endregion

        #region Properties

        public event EventHandler Click = null!;
        public bool Locked { get; set;} = false;

        // Appearence
        public Texture2D SliderTexture { get; set; } = GameInstance.PlainTexture;
        public Color SliderBarColor { get; set; } = Color.White;
        public Color ThumbColor { get; set; } = Color.White;
        public Color ThumbHoverColor { get; set; } = new(200, 200, 200, 255);
        public Color ThumbLockedColor { get; set; } = new(127, 127, 127, 255);
        public Color TextColor { get; set; } = Color.White;
        public Color TextHoverColor { get; set; } = new(200, 200, 200, 255);

        public string? Text { get; set; }
        public QuadVector TextPosition { get; set; } = new(.5f, .5f, 0, 0);
        public bool CenterTextY{ get; set; } = true;
        public float TextScale { get; set; } = 1f;
        public SpriteFont? TextFont { get; set; }

        // Texturing
        public Rectangle TextureSourceRectangle { get; set; }
        public QuadVector TextureSourcePosition { get; set; } = new(0, 0, 0, 0);
        public QuadVector TextureSourceSize { get; set; } = new(1, 1, 0, 0);

        #endregion

        public Rectangle SliderBar { get; set; }
        public Rectangle ThumbRectangle { get; set; }

        
        public Slider(GameInstance game) : base(game)
        {
            SliderBar = new();
            ThumbRectangle = new();
            TextureSourceRectangle = new();
        }
    }
}