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
        public Texture2D? ImageTexture {get; set;} = null;

        // Outline/Inline
        public Color OutlineColor { get; set; } = Color.White;
        public Color InlineColor { get; set; } = Color.White;

        public int OutlineSize { get; set; } = 0;
        public int InlineSize { get; set; } = 0;

        #endregion

        public Rectangle Body { get; set; }
        public Rectangle Outline { get; set; }

        public Image(GameInstance game) : base(game)
        {
            Body = new Rectangle();
        }

        #region Update/Draw

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Body = new Rectangle((int)Position.ConvertToScreenPosition(Game)[0], (int)Position.ConvertToScreenPosition(Game)[1], (int)Size.ConvertToScreenPosition(Game)[0], (int)Size.ConvertToScreenPosition(Game)[1]);
            if (Body.Width != 0 && Body.Height != 0)
            {
                if (OutlineSize > 0)
                {
                    // Draw outline
                    Outline = new Rectangle(Body.X - OutlineSize/2, Body.Y - OutlineSize/2, Body.Width + OutlineSize, Body.Height + OutlineSize);
                    spriteBatch.Draw(GameInstance.PlainTexture, Outline, OutlineColor);
                }
                // Draw body
                if (ImageTexture == null)
                {
                    spriteBatch.Draw(GameInstance.PlainTexture, Body, ImageColor);
                }
                else { spriteBatch.Draw(ImageTexture, Body, ImageColor); }
            }
            base.Draw(gameTime, spriteBatch);
        }
        
        #endregion
    }
}