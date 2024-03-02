using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL.UI.BuiltIn
{
    public class Button : Component
    {
        #region Fields

        public bool IsHovering { get; protected set; }

        #endregion

        #region Properties

        public event EventHandler Click = null!;
        public bool Locked { get; set;} = false;

        // Appearence
        public Texture2D ButtonTexture {get; set;} = GameInstance.PlainTexture;
        public Color ButtonColor {get; set;} = Color.White;
        public Color ButtonHoverColor {get; set;} = new(200, 200, 200, 255);
        public Color ButtonLockedColor {get; set;} = new(127, 127, 127, 255);
        public Color TextColor {get; set;} = Color.White;
        public Color TextHoverColor {get; set;} = new(200, 200, 200, 255);

        public string? Text {get; set;}
        public Vector2 TextPosition {get; set;} = new(.5f, .5f);
        public float TextScale {get; set;} = 1f;
        public SpriteFont? TextFont {get; set;}

        // Texturing
        public Rectangle TextureSourceRectangle {get; set;}
        public Vector2 TextureSourcePosition {get; set;} = Vector2.Zero;
        public Vector2 TextureSourceSize {get; set;} = Vector2.One;

        #endregion

        public Rectangle ButtonRectangle {get; set;}

        
        public Button(GameInstance game) : base(game)
        {
            ButtonRectangle = new();
            TextureSourceRectangle = new();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            _previousMouseState = MouseState;
            MouseState = Mouse.GetState();

            _previousCursor = Cursor;
            Cursor = Bound == null ? Game.Cursor : Bound.Cursor;

            if (Cursor.Intersects(ButtonRectangle))
            {
                IsHovering = true;

                if (MouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed && !Locked)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
            else { IsHovering = false; }

            UpdateSourceRectangles();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ButtonRectangle = new((int)(Position.X - Size.X * Origin.X), (int)(Position.Y - Size.Y * Origin.Y), (int)Size.X, (int)Size.Y);
            if (ButtonRectangle.Width != 0 && ButtonRectangle.Height != 0)
            {
                // Draw Button
                Color DrawColor;
                if (Locked) { DrawColor = ButtonLockedColor; } else { DrawColor = IsHovering ? ButtonHoverColor : ButtonColor; }
                spriteBatch.Draw(ButtonTexture, ButtonRectangle, TextureSourceRectangle, DrawColor, MathHelper.ToRadians(Rotation), new(0,0), SpriteEffects.None, 0.1f);

                // Draw Text
                if (!string.IsNullOrEmpty(Text) && TextFont != null)
                {
                    var xOffset = ButtonRectangle.Width * TextPosition.X;
                    var yOffset = ButtonRectangle.Height * TextPosition.Y;
                    var x = ButtonRectangle.X + xOffset - TextFont.MeasureString(Text).X / 2f * TextScale;
                    var y = ButtonRectangle.Y + yOffset - TextFont.MeasureString(Text).Y / 2f * TextScale;
                    var textColor = IsHovering ? TextHoverColor : TextColor;
                    spriteBatch.DrawString(TextFont, Text, new(x, y), textColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.25f);
                }
            }
            
            base.Draw(gameTime, spriteBatch);
        }

        public virtual void UpdateSourceRectangles()
        {
            TextureSourceRectangle = new((int)TextureSourcePosition.X, (int)TextureSourcePosition.Y, (int)TextureSourceSize.X, (int)TextureSourceSize.Y);
        }
        #endregion
    }
}