using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class Text : Component
    {
        #region Fields

        private PositionVector PreviousPosition {get; set;} = PositionVector.Zero;
        private float PreviousScale {get; set;} = 1f;

        #endregion

        #region Properties
        
        public string? Content { get; set; }
        public Color TextColor { get; set; } = Color.White;
        public float TextScale { get; set; } = 1f;
        public SpriteFont? TextFont { get; set; }
        public Rectangle TextBounds { get; set; }

        #endregion

        public Text(GameInstance game) : base(game)
        {
            TextBounds = new Rectangle();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            // Check for any changes.
            if (TextFont != null && (Position != PreviousPosition || TextScale != PreviousScale))
            {
                // Update text bounds
                TextBounds = new Rectangle((int)Position.ConvertToScreenPosition(Game)[0], (int)Position.ConvertToScreenPosition(Game)[1], (int)(TextFont.MeasureString(Content).X * TextScale), (int)(TextFont.MeasureString(Content).Y * TextScale));
            }

            PreviousPosition = Position; PreviousScale = TextScale;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (TextFont != null)
            {
                Vector2 ConvertedPosition = new Vector2(Position.ConvertToScreenPosition(Game)[0], Position.ConvertToScreenPosition(Game)[1]);
                spriteBatch.DrawString(TextFont, Content, ConvertedPosition, TextColor, Rotation, Vector2.Zero, TextScale, SpriteEffects.None, 0);
            }

            base.Draw(gameTime, spriteBatch);
        }
        #endregion
    }
}