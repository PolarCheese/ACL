using ACL;
using ACL.Values;
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
        public Texture2D ImageTexture {get; set;} = GameInstance.PlainTexture;

        // Outline/Inline
        public Color OutlineColor { get; set; } = Color.White;
        public Color InlineColor { get; set; } = Color.White;

        public int OutlineSize { get; set; } = 0;
        public int InlineSize { get; set; } = 0;

        // Texturing
        public Rectangle TextureSourceRectangle { get; set; }
        public PositionVector TextureSourcePosition { get; set; } = new PositionVector(0, 0, 0, 0);
        public PositionVector TextureSourceSize { get; set; } = new PositionVector(1, 1, 0, 0);

        #endregion

        public Rectangle ImageBounds { get; set; }
        public Rectangle Outline { get; set; }

        public Image(GameInstance game) : base(game)
        {
            ImageBounds = new Rectangle();
            Outline = new Rectangle();
            TextureSourceRectangle = new Rectangle();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            UpdateSourceRectangles();
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
                spriteBatch.Draw(ImageTexture, ImageBounds, TextureSourceRectangle, ImageColor);
            }
            base.Draw(gameTime, spriteBatch);
        }
        
        public void UpdateSourceRectangles()
        {
            Rectangle TextureBounds = new Rectangle(0, 0, ImageTexture.Width, ImageTexture.Height);
            TextureSourceRectangle = new Rectangle((int)TextureSourcePosition.ConvertToBound(TextureBounds)[0], (int)TextureSourcePosition.ConvertToBound(TextureBounds)[1], (int)TextureSourceSize.ConvertToBound(TextureBounds)[0], (int)TextureSourceSize.ConvertToBound(TextureBounds)[1]); 
        }
        #endregion
    }
}