using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class Text : Component
    {
        #region Fields

        protected QuadVector PreviousPosition { get; set; } = QuadVector.Zero;
        protected float PreviousScale { get; set; } = 1f;

        #endregion

        #region Properties
        
        public string? Content { get; set; }
        public Color TextColor { get; set; } = Color.White;
        public float TextScale { get; set; } = 1f;
        public SpriteFont? TextFont { get; set; }
        public Vector2 TextBoundsSize { get; private set; }

        #endregion

        public Text(GameInstance game) : base(game) {}

        #region Methods

        public override void Update(GameTime gameTime)
        {
            // Check for any changes.
            if (TextFont != null && (Position != PreviousPosition || TextScale != PreviousScale))
            {
                TextBoundsSize = TextFont.MeasureString(Content) * TextScale;
            }

            PreviousPosition = Position; PreviousScale = TextScale;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (TextFont != null)
            {
                // Center text to origin
                spriteBatch.DrawString(TextFont, Content, ActualPosition, TextColor, Rotation, Vector2.Zero, TextScale, SpriteEffects.None, 0);
            }

            base.Draw(gameTime, spriteBatch);
        }

        public override void UpdateProperties()
        {
            // Update actual size/position etc.
            bool HasParent = Parent != null;
            if (_previousPosition != Position) {
                // Position has changed. Recalculate actual position.
                if (HasParent && Parent.PositionChildrenToParent) {ActualPosition = Parent.ActualPosition + Position.ToVector2(Parent.ActualPosition) - new Vector2(TextBoundsSize.X * Origin.X, TextBoundsSize.Y * Origin.Y);}
                else {ActualPosition = Position.ToVector2(Game) - new Vector2(TextBoundsSize.X * Origin.X, TextBoundsSize.Y * Origin.Y);}
            }
            if (_previousSize != Size) {
                // Size has changed. Recalculate actual position.
                if (HasParent && Parent.SizeChildrenToParent) {ActualSize = Size.ToVector2(Parent.ActualSize);}
                else {ActualSize = Size.ToVector2(Game);}
            } 

            _previousPosition = Position; _previousSize = Size;
        }
        #endregion
    }
}