using ACL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class Image : Component
    {
        #region Properties

        // Image
        public Color ImageColor { get; set; } = Color.White;
        public Color ImageBackgroundColor { get; set; } // If the image is transparent, this will be the color shown behind.
        public Texture2D? ImageTexture {get; set;} = null;

        // Outline/Inline
        public Color OutlineColor { get; set; } = Color.White;
        public Color InlineColor { get; set; } = Color.White;

        public int OutlineSize { get; set; } = 0;
        public int InlineSize { get; set; } = 0;

        #endregion

        public Rectangle ImageBounds { get; set; }
        public Rectangle Outline { get; set; }

        public Image(GameInstance game) : base(game)
        {
            ImageBounds = new Rectangle();
            Outline = new Rectangle();
        }

        #region Update/Draw

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ImageBounds = new Rectangle((int)Position.ConvertToScreenPosition(Game)[0], (int)Position.ConvertToScreenPosition(Game)[1], (int)Size.ConvertToScreenPosition(Game)[0], (int)Size.ConvertToScreenPosition(Game)[1]);
            if (ImageBounds.Width != 0 && ImageBounds.Height != 0)
            {
                if (OutlineSize > 0)
                {
                    // Draw outline
                    Outline = new Rectangle(ImageBounds.X - OutlineSize/2, ImageBounds.Y - OutlineSize/2, ImageBounds.Width + OutlineSize, ImageBounds.Height + OutlineSize);
                    spriteBatch.Draw(GameInstance.PlainTexture, Outline, OutlineColor);
                }
                if (ImageBackgroundColor.A > 0)
                {
                    // Draw Background
                    spriteBatch.Draw(GameInstance.PlainTexture, ImageBounds, ImageBackgroundColor);
                }
                // Draw Image
                if (ImageTexture == null)
                {
                    spriteBatch.Draw(GameInstance.PlainTexture, ImageBounds, ImageColor);
                }
                else { spriteBatch.Draw(ImageTexture, ImageBounds, ImageColor); }
            }
            base.Draw(gameTime, spriteBatch);
        }
        
        #endregion
    }
}