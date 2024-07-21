using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL.UI.BuiltIn
{
    public class Checkbox : Component
    {
        #region Fields

        public bool IsHovering { get; protected set; }

        #endregion

        #region Properties

        public event EventHandler? Click;
        public bool Value {get; set;} = false;
        public bool Locked { get; set;} = false;

        // Appearence

        public Texture2D CheckboxTexture {get; set;} = GameInstance.PlainTexture;
        public Color Color {get; set;} = Color.White;
        public Color HoverColor {get; set;} = new(200, 200, 200, 255);
        public Color LockedColor {get; set;} = new(127, 127, 127, 255);
        public Color TextColor {get; set;} = Color.White;
        public Color TextHoverColor {get; set;} = new(200, 200, 200, 255);

        public string? Text {get; set;}
        public Vector2 TextPosition {get; set;} = new Vector2(1.25f, 0f);
        public bool CenterTextY {get; set;} = true;
        public float TextScale {get; set;} = 1f;
        public SpriteFont? TextFont {get; set;}

        // Texturing
        Rectangle TextureSourceRectangle {get; set;}
        public Vector2 TextureOffSourcePosition {get; set;} = new(1.25f, 0f);
        public Vector2 TextureOffSourceSize {get; set;} = new(.5f, .5f);
        public Vector2 TextureOnSourcePosition {get; set;} = new(.5f, 0f);
        public Vector2 TextureOnSourceSize {get; set;} = new(.5f, .5f);

        #endregion

        public Rectangle CheckboxRectangle {get; set;}
        
        public Checkbox(GameInstance game) : base(game)
        {
            CheckboxRectangle = new();
            TextureSourceRectangle = new();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            CheckboxRectangle = new((int)ActualPosition.X, (int)ActualPosition.Y, (int)Size.X, (int)Size.Y);

            _previousMouseState = MouseState;
            MouseState = Mouse.GetState();

            _previousCursor = Cursor;
            Cursor = Game.Cursor;

            if (Cursor.Intersects(CheckboxRectangle))
            {
                IsHovering = true;

                if (MouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed && !Locked)
                {
                    Toggle();
                    Click?.Invoke(this, new EventArgs());
                }
            }
            else { IsHovering = false; }

            // Update texture source rectangle
            if (!Value) { TextureSourceRectangle = new((int)TextureOffSourcePosition.X, (int)TextureOffSourcePosition.Y, (int)TextureOffSourceSize.X, (int)TextureOffSourceSize.Y); } 
            else {TextureSourceRectangle = new((int)TextureOnSourcePosition.X, (int)TextureOnSourcePosition.Y, (int)TextureOnSourceSize.X, (int)TextureOnSourceSize.Y); } 
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (CheckboxRectangle.Width != 0 && CheckboxRectangle.Height != 0)
            {
                // Draw Checkbox
                Color DrawColor;
                if (Locked) { DrawColor = LockedColor; } else { DrawColor = IsHovering ? HoverColor : Color; }
                spriteBatch.Draw(CheckboxTexture, CheckboxRectangle, TextureSourceRectangle, DrawColor);
                
                // Draw Text
                if (!string.IsNullOrEmpty(Text) && TextFont != null)
                {
                    var x = CheckboxRectangle.X + CheckboxRectangle.Width * TextPosition.X;
                    var y = CenterTextY ? CheckboxRectangle.Y + (CheckboxRectangle.Height / 2) - (TextFont.MeasureString(Text).Y / 2f * TextScale) :
                    CheckboxRectangle.Y + CheckboxRectangle.Height * TextPosition.Y;
                    var textColor = IsHovering ? TextHoverColor : TextColor;
                    spriteBatch.DrawString(TextFont, Text, new(x, y), textColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.1f);
                }
            }
            
            base.Draw(gameTime, spriteBatch);
        }
        
        public virtual void Toggle(bool? NewValue = null)
        {
            if (NewValue.HasValue) {Value = NewValue.Value;} else {Value = !Value;}
        }
        #endregion
    }
}