using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class Container : Component
    {

        #region Properties

        // Background
        public Color BackgroundColor { get; set; } = new Color(0, 0, 0, 255);
        public Texture2D? BackgroundTexture {get; set;} = null;

        // Outline/Inline
        public Color OutlineColor { get; set; } = Color.White;
        public Color InlineColor { get; set; } = Color.White;

        public int OutlineSize { get; set; } = 0;
        public int InlineSize { get; set; } = 0;

        #endregion

        public Rectangle Outline { get; set; }
        public Rectangle Body { get; set; }

        public Container()
        {
            Outline = new Rectangle();
            Body = new Rectangle();
        }

        #region Update/Draw

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Body.Width != 0 && Body.Height != 0)
            {
                // Draw outline
                Outline = new Rectangle(Body.X - OutlineSize/2, Body.Y - OutlineSize/2, Body.Width + 2*OutlineSize, Body.Height + 2*OutlineSize);
                spriteBatch.Draw(GameInstance.PlainTexture, Outline, OutlineColor);

                // Draw body
                if (BackgroundTexture == null)
                {
                    spriteBatch.Draw(GameInstance.PlainTexture, Body, BackgroundColor);
                }
                else { spriteBatch.Draw(BackgroundTexture, Body, BackgroundColor); }
            }
            base.Draw(gameTime, spriteBatch);
        }
        
        #endregion
    }
}