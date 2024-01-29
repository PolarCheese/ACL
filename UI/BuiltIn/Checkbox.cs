using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL.UI.BuiltIn
{
    public class Checkbox : Component
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
        public bool Value { get; set; } = false;
        public bool Locked { get; set;} = false;

        // Appearence

        public Texture2D CheckboxTexture { get; set; } = GameInstance.PlainTexture;
        public Color CheckboxColor { get; set; } = Color.White;
        public Color CheckboxHoverColor { get; set; } = new(200, 200, 200, 255);
        public Color CheckboxLockedColor { get; set; } = new(127, 127, 127, 255);
        public Color TextColor { get; set; } = Color.White;
        public Color TextHoverColor { get; set; } = new(200, 200, 200, 255);

        public string? Text { get; set; }
        public QuadVector TextPosition { get; set; } = new(1.25f, 0, 0, 0);
        public bool CenterTextY{ get; set; } = true;
        public float TextScale { get; set; } = 1f;
        public SpriteFont? TextFont { get; set; }

        // Texturing
        public Rectangle TextureSourceRectangle { get; set; }
        public QuadVector TextureOffSourcePosition { get; set; } = new(0, 0, 0, 0);
        public QuadVector TextureOffSourceSize { get; set; } = new(.5f, .5f, 0, 0);
        public QuadVector TextureOnSourcePosition { get; set; } = new(.5f, 0, 0, 0);
        public QuadVector TextureOnSourceSize { get; set; } = new(.5f, .5f, 0, 0);

        #endregion

        public Rectangle CheckboxRectangle { get; set; }
        
        public Checkbox(GameInstance game) : base(game)
        {
            CheckboxRectangle = new();
            TextureSourceRectangle = new();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _previousMouse = _currentMouse;
            _currentMouse = Game.PlayerCursor;

            _isHovering = false;

            if (_currentMouse.Intersects(CheckboxRectangle))
            {
                _isHovering = true;

                if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed && !Locked)
                {
                    Toggle();
                    Click?.Invoke(this, new EventArgs());
                }
            }

            UpdateSourceRectangles();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            CheckboxRectangle = new((int)ActualPosition.X, (int)ActualPosition.Y, (int)ActualSize.X, (int)ActualSize.Y);
            if (CheckboxRectangle.Width != 0 && CheckboxRectangle.Height != 0)
            {
                // Draw Checkbox
                Color DrawColor;
                if (Locked) { DrawColor = CheckboxLockedColor; } else { DrawColor = _isHovering ? CheckboxHoverColor : CheckboxColor; }
                spriteBatch.Draw(CheckboxTexture, CheckboxRectangle, TextureSourceRectangle, DrawColor);
                
                // Draw Text
                if (!string.IsNullOrEmpty(Text) && TextFont != null)
                {
                    var xOffset = CheckboxRectangle.Width * TextPosition.RelativeX + TextPosition.AbsoluteX;
                    var yOffset = CenterTextY ? (CheckboxRectangle.Height / 2) - (TextFont.MeasureString(Text).Y / 2f * TextScale) : CheckboxRectangle.Height * TextPosition.RelativeY + TextPosition.AbsoluteY;
                    var x = CheckboxRectangle.X + xOffset;
                    var y = CheckboxRectangle.Y + yOffset;
                    var textColor = _isHovering ? TextHoverColor : TextColor;
                    spriteBatch.DrawString(TextFont, Text, new(x, y), textColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.1f);
                }
            }
            
            base.Draw(gameTime, spriteBatch);
        }
        
        public virtual void Toggle(bool? NewValue = null)
        {
            if (NewValue.HasValue) {Value = NewValue.Value;} else {Value = !Value;}
        }

        public virtual void UpdateSourceRectangles()
        {
            Rectangle TextureBounds = new(0, 0, CheckboxTexture.Width, CheckboxTexture.Height);
            Vector2 OffPosition = TextureOffSourcePosition.ToVector2(TextureBounds); Vector2 OnPosition = TextureOnSourcePosition.ToVector2(TextureBounds);
            Vector2 OffSize = TextureOffSourceSize.ToVector2(TextureBounds); Vector2 OnSize = TextureOnSourceSize.ToVector2(TextureBounds);
            if (!Value) { TextureSourceRectangle = new((int)OffPosition.X, (int)OffPosition.Y, (int)OffSize.X, (int)OffSize.Y); } 
            else {TextureSourceRectangle = new((int)OnPosition.X, (int)OnPosition.Y, (int)OnSize.X, (int)OnSize.Y); } 
        }
        #endregion
    }
}