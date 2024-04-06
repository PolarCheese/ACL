using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ACL.UI.BuiltIn
{
    public class Slider : Component
    {
        #region Fields
        public bool IsHovering {get; protected set;}

        #endregion

        #region Properties

        public event EventHandler Click = null!;
        public bool Locked {get; set;} = false;

        // Functionality
        public float MinimumValue { get; set; } = 0;
        public float MaximumValue { get; set; } = 100;
        public float Value { get; set; }
        public float RoundByNumber { get; set; } = 0;

        // Appearence
        public Texture2D SliderTexture {get; set;} = GameInstance.PlainTexture;
        public Color SliderBarColor {get; set;} = Color.White;
        public Color ThumbColor {get; set;} = Color.White;
        public Color ThumbHoverColor {get; set;} = new(200, 200, 200, 255);
        public Color ThumbLockedColor {get; set;} = new(127, 127, 127, 255);

        public int EdgesWidth {get; set;} = 4;
        

        public string? Text {get; set;} // Text
        public Color TextColor {get; set;} = Color.White;
        public Color TextHoverColor {get; set;} = new(200, 200, 200, 255);
        public Vector2 TextPosition {get; set;} = new(1.25f, 0);
        public bool CenterTextY {get; set;} = true;
        public float TextScale {get; set;} = 1f;
        public SpriteFont? TextFont {get; set;}

        // Texturing
        Rectangle BarTextureSourceRectangle {get; set;} // Bar
        public Vector2 BarTextureSourcePosition {get; set;} = Vector2.Zero;
        public Vector2 BarTextureSourceSize {get; set;} = Vector2.One;

        Rectangle ThumbTextureSourceRectangle {get; set;} // Thumb
        public Vector2 ThumbTextureSourcePosition {get; set;} = Vector2.Zero;
        public Vector2 ThumbTextureSourceSize {get; set;} = Vector2.One;

        Rectangle EdgeLTextureSourceRectangle {get; set;} // Left Edge
        public Vector2 EdgeLTextureSourcePosition {get; set;} = Vector2.Zero;
        public Vector2 EdgeLTextureSourceSize {get; set;} = Vector2.One;

        Rectangle EdgeRTextureSourceRectangle {get; set;} // Right Edge
        public Vector2 EdgeRTextureSourcePosition {get; set;} = Vector2.Zero;
        public Vector2 EdgeRTextureSourceSize {get; set;} = Vector2.One;

        #endregion

        public Rectangle SliderBar {get; set;}
        public Rectangle ThumbRectangle {get; set;}

        
        public Slider(GameInstance game) : base(game)
        {
            SliderBar = new();
            ThumbRectangle = new();
        }

        public override void Update(GameTime gameTime)
        {
            // Update rectangles
            SliderBar = new((int)(Position.X - Size.X * Origin.X), (int)(Position.Y - Size.Y * Origin.Y), (int)Size.X, (int)Size.Y);
            float thumbSize = SliderBar.Height * 1.25f;

            BarTextureSourceRectangle = new((int)BarTextureSourcePosition.X, (int)BarTextureSourcePosition.Y, (int)BarTextureSourceSize.X, (int)BarTextureSourceSize.Y);
            ThumbTextureSourceRectangle = new((int)ThumbTextureSourcePosition.X, (int)ThumbTextureSourcePosition.Y, (int)ThumbTextureSourceSize.X, (int)ThumbTextureSourceSize.Y);
            EdgeLTextureSourceRectangle = new((int)EdgeLTextureSourcePosition.X, (int)EdgeLTextureSourcePosition.Y, (int)EdgeLTextureSourceSize.X, (int)EdgeLTextureSourceSize.Y);
            EdgeRTextureSourceRectangle = new((int)EdgeRTextureSourcePosition.X, (int)EdgeRTextureSourcePosition.Y, (int)EdgeRTextureSourceSize.X, (int)EdgeRTextureSourceSize.Y);

            // Cursor logic
            _previousMouseState = MouseState;
            MouseState = Mouse.GetState();

            _previousCursor = Cursor;
            Cursor = Bound == null ? Game.Cursor : Bound.Cursor;

            if (Cursor.Intersects(ThumbRectangle) || Cursor.Intersects(SliderBar))
            {
                IsHovering = true;

                if (MouseState.LeftButton == ButtonState.Pressed)
                {
                    // Calculate value
                    float MouseRelativeX = Cursor.X - SliderBar.X;
                    float SliderLength = SliderBar.Width; // end of the slider X RelativePosition
                    float RelativePercentage = MouseRelativeX / SliderLength;
                    float RelativeInterval = MaximumValue - MinimumValue;
                    float calcValue = (float)((float)RelativePercentage * (float)RelativeInterval + (float)MinimumValue); // result
                    
                    // Round calculated value
                    if (RoundByNumber != 0) { calcValue = (float)Math.Ceiling(calcValue / RoundByNumber) * RoundByNumber; }

                    // Clamp value
                    if (MinimumValue > calcValue) { Value = MinimumValue; }
                    else if (calcValue > MaximumValue) { Value = MaximumValue; }
                    else { Value = calcValue; }

                    Click?.Invoke(this, new EventArgs());
                }
            }
            else { IsHovering = false; }

            // Update thumb position
            float thumbPosition = (float)(Value - MinimumValue) / (float)(MaximumValue - MinimumValue) * SliderBar.Width - thumbSize / 2f;
            ThumbRectangle = new((int)(SliderBar.X + thumbPosition), (int)(SliderBar.Y + (SliderBar.Height - thumbSize) / 2f), (int)thumbSize, (int)thumbSize);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw slider
            spriteBatch.Draw(SliderTexture, SliderBar, BarTextureSourceRectangle, SliderBarColor, MathHelper.ToRadians(Rotation), new(0,0), SpriteEffects.None, 0.1f); // Draw bar
            spriteBatch.Draw(SliderTexture, new(SliderBar.X - EdgesWidth, SliderBar.Y, EdgesWidth, SliderBar.Height), EdgeLTextureSourceRectangle, SliderBarColor, MathHelper.ToRadians(Rotation), new(0,0), SpriteEffects.None, 0.1f); // Draw left edge
            spriteBatch.Draw(SliderTexture, new(SliderBar.X + SliderBar.Width, SliderBar.Y, EdgesWidth, SliderBar.Height), EdgeRTextureSourceRectangle, SliderBarColor, MathHelper.ToRadians(Rotation), new(0,0), SpriteEffects.None, 0.1f); // Draw right edge

            // Draw Thumb
            Color StateColor;
            if (Locked) { StateColor = ThumbLockedColor; } else { StateColor = IsHovering ? ThumbHoverColor : ThumbColor; }
            spriteBatch.Draw(SliderTexture, ThumbRectangle, ThumbTextureSourceRectangle, StateColor, MathHelper.ToRadians(Rotation), new(0,0), SpriteEffects.None, 0.1f); // Draw bar

            // Draw Value as text
            if (!string.IsNullOrEmpty(Text) && TextFont != null)
            {
                var x = Position.X + Size.X * TextPosition.X;
                var y = CenterTextY ? Position.Y + (Size.Y / 2) - (TextFont.MeasureString(Text).Y / 2f * TextScale) :
                Position.Y + Size.Y * TextPosition.Y;
                var textDrawColor = IsHovering ? TextHoverColor : TextColor;
                spriteBatch.DrawString(TextFont, Text, new(x, y), textDrawColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
            }

            base.Draw(gameTime, spriteBatch);
        }
    }
}