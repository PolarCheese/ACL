using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL.UI
{
    public class Button : Component
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
        public Texture2D ButtonTexture { get; set; } = GameInstance.PlainTexture;
        public Color ButtonColor { get; set; } = Color.White;
        public Color ButtonHoverColor { get; set; } = new(200, 200, 200, 255);
        public Color ButtonLockedColor { get; set; } = new(127, 127, 127, 255);
        public Color TextColor { get; set; } = Color.White;
        public Color TextHoverColor { get; set; } = new(200, 200, 200, 255);

        public string? Text { get; set; }
        public PositionVector TextPosition { get; set; } = new(.5f, .5f, 0, 0);
        public float TextScale { get; set; } = 1f;
        public SpriteFont? TextFont { get; set; }

        // Texturing
        public Rectangle TextureSourceRectangle { get; set; }
        public PositionVector TextureSourcePosition { get; set; } = new(0, 0, 0, 0);
        public PositionVector TextureSourceSize { get; set; } = new(1, 1, 0, 0);

        #endregion

        public Rectangle ButtonRectangle { get; set; }

        
        public Button(GameInstance game) : base(game)
        {
            ButtonRectangle = new();
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
            ButtonRectangle = new((int)ActualPosition.X, (int)ActualPosition.Y, (int)ActualSize.X, (int)ActualSize.Y);
            if (ButtonRectangle.Width != 0 && ButtonRectangle.Height != 0)
            {
                // Draw Button
                Color DrawColor;
                if (Locked) { DrawColor = ButtonLockedColor; } else { DrawColor = _isHovering ? ButtonHoverColor : ButtonColor; }
                spriteBatch.Draw(ButtonTexture, ButtonRectangle, TextureSourceRectangle, DrawColor, MathHelper.ToRadians(Rotation), new(0,0), SpriteEffects.None, 0.1f);

                // Draw Text
                if (!string.IsNullOrEmpty(Text) && TextFont != null)
                {
                    var xOffset = ButtonRectangle.Width * TextPosition.RelativeX + TextPosition.AbsoluteX;
                    var yOffset = ButtonRectangle.Height * TextPosition.RelativeY + TextPosition.AbsoluteY;
                    var x = ButtonRectangle.X + xOffset - TextFont.MeasureString(Text).X / 2f * TextScale;
                    var y = ButtonRectangle.Y + yOffset - TextFont.MeasureString(Text).Y / 2f * TextScale;
                    var textColor = _isHovering ? TextHoverColor : TextColor;
                    spriteBatch.DrawString(TextFont, Text, new(x, y), textColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.25f);
                }
            }
            
            base.Draw(gameTime, spriteBatch);
        }

        public virtual void UpdateSourceRectangles()
        {
            Rectangle TextureBounds = new(0, 0, ButtonTexture.Width, ButtonTexture.Height);
            Vector2 Position = TextureSourcePosition.ToVector2(TextureBounds); Vector2 Size = TextureSourceSize.ToVector2(TextureBounds);
            TextureSourceRectangle = new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
        #endregion
    }
}