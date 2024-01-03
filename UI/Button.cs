using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL.UI
{
    public class Button : Component
    {
        #region Fields

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private Rectangle _currentMouse;
        private Rectangle _previousMouse;

        private bool _isHovering;

        #endregion

        #region Properties

        public event EventHandler Click = null!;
        public bool Locked { get; set;} = false;

        // Appearence
        public Texture2D ButtonTexture {get; set;} = GameInstance.PlainTexture;
        public Color ButtonColor { get; set; } = Color.White;
        public Color ButtonHoverColor { get; set; } = new Color(200, 200, 200, 255);
        public Color ButtonLockedColor { get; set; } = new Color(127, 127, 127, 255);
        public Color TextColor { get; set; } = Color.White;
        public Color TextHoverColor { get; set; } = new Color(200, 200, 200, 255);

        public string? Text { get; set; }
        public float TextScale { get; set; } = 1f;
        public SpriteFont? TextFont { get; set; }

        // Texturing
        public Rectangle TextureSourceRectangle { get; set; }
        public PositionVector TextureSourcePosition { get; set; } = new PositionVector(0, 0, 0, 0);
        public PositionVector TextureSourceSize { get; set; } = new PositionVector(1, 1, 0, 0);

        #endregion

        public Rectangle ButtonRectangle { get; set; }

        
        public Button(GameInstance game) : base(game)
        {
            ButtonRectangle = new Rectangle();
            TextureSourceRectangle = new Rectangle();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _previousMouse = _currentMouse;
            _currentMouse = Game.PlayerCursor;

            _isHovering = false;

            if (_currentMouse.Intersects(ButtonRectangle))
            {
                _isHovering = true;

                if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed && !Locked)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }

            UpdateSourceRectangles();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ButtonRectangle = new Rectangle((int)Position.ConvertToScreenPosition(Game)[0], (int)Position.ConvertToScreenPosition(Game)[1], (int)Size.ConvertToScreenPosition(Game)[0], (int)Size.ConvertToScreenPosition(Game)[1]);
            if (ButtonRectangle.Width != 0 && ButtonRectangle.Height != 0)
            {
                // Draw Button
                Color DrawColor;
                if (Locked) { DrawColor = ButtonLockedColor; } else { DrawColor = _isHovering ? ButtonHoverColor : ButtonColor; }
                spriteBatch.Draw(ButtonTexture, ButtonRectangle, TextureSourceRectangle, DrawColor);

                // Draw Text
                if (!string.IsNullOrEmpty(Text) && TextFont != null)
                {
                    var x = ButtonRectangle.X + ButtonRectangle.Width / 2 - TextFont.MeasureString(Text).X / 2 * TextScale;
                    var y = ButtonRectangle.Y + ButtonRectangle.Height / 2 - TextFont.MeasureString(Text).Y / 2 * TextScale;
                    var textColor = _isHovering ? TextHoverColor : TextColor;
                    spriteBatch.DrawString(TextFont, Text, new Vector2(x, y), textColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.1f);
                }
            }
            
            base.Draw(gameTime, spriteBatch);
        }

        public void UpdateSourceRectangles()
        {
            Rectangle TextureBounds = new Rectangle(0, 0, ButtonTexture.Width, ButtonTexture.Height);
            TextureSourceRectangle = new Rectangle((int)TextureSourcePosition.ConvertToBound(TextureBounds)[0], (int)TextureSourcePosition.ConvertToBound(TextureBounds)[1], (int)TextureSourceSize.ConvertToBound(TextureBounds)[0], (int)TextureSourceSize.ConvertToBound(TextureBounds)[1]); 
        }
        #endregion
    }
}