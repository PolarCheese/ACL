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

        #endregion

        public Text(GameInstance game) : base(game) {}

        #region Methods

        public override void Update(GameTime gameTime)
        {
            // Check for any changes.
            if (TextFont != null && (Position != PreviousPosition || TextScale != PreviousScale))
            {
                // Update text bounds (removed for now)
            }

            PreviousPosition = Position; PreviousScale = TextScale;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (TextFont != null)
            {
                // Convert position, size and rotation.
                Vector2 ConvertedPosition = new Vector2(); Vector2 TextBoundsSize = new Vector2();
                TextBoundsSize = TextFont.MeasureString(Content) * TextScale;

                if (Parent != null)
                {
                    if (Parent.PositionChildrenToParent) {ConvertedPosition = Position.ConvertToBound(Parent.GetBounds());}
                    if (Parent.RotateChildrenToParent) {Rotation = Rotation + Parent.Rotation;};
                }
                // Use game as bounds. 
                else {ConvertedPosition = Position.ConvertToScreenPosition(Game);}

                ConvertedPosition = new Vector2(ConvertedPosition.X - TextBoundsSize.X * Origin.X, ConvertedPosition.Y - TextBoundsSize.Y * Origin.Y);
                spriteBatch.DrawString(TextFont, Content, ConvertedPosition, TextColor, Rotation, Vector2.Zero, TextScale, SpriteEffects.None, 0);
            }

            base.Draw(gameTime, spriteBatch);
        }
        #endregion
    }
}