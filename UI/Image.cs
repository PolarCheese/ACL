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
        public Texture2D ImageTexture { get; set; } = GameInstance.PlainTexture;

        // Outline/Inline
        public Color OutlineColor { get; set; } = Color.White;
        public Color InlineColor { get; set; } = Color.White;

        public int OutlineSize { get; set; } = 0;
        public int InlineSize { get; set; } = 0;

        // Texturing
        public Rectangle TextureSourceRectangle { get; set; }
        public PositionVector TextureSourcePosition { get; set; } = new(0, 0, 0, 0);
        public PositionVector TextureSourceSize { get; set; } = new(1, 1, 0, 0);

        #endregion

        public Rectangle ImageBounds { get; set; }
        public Rectangle Outline { get; set; }

        public Image(GameInstance game) : base(game)
        {
            ImageBounds = new();
            Outline = new();
            TextureSourceRectangle = new();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            UpdateSourceRectangles();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ImageBounds = new((int)ActualPosition.X, (int)ActualPosition.Y, (int)ActualSize.X, (int)ActualSize.Y);
            if (ImageBounds.Width != 0 && ImageBounds.Height != 0)
            {
                if (OutlineSize > 0)
                {
                    // Draw outline
                    Outline = new(ImageBounds.X - OutlineSize/2, ImageBounds.Y - OutlineSize/2, ImageBounds.Width + OutlineSize, ImageBounds.Height + OutlineSize);
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
        
        public virtual void UpdateSourceRectangles()
        {
            Rectangle TextureBounds = new(0, 0, ImageTexture.Width, ImageTexture.Height);
            Vector2 Position = TextureSourcePosition.ToVector2(TextureBounds); Vector2 Size = TextureSourceSize.ToVector2(TextureBounds);
            TextureSourceRectangle = new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
        #endregion
    }
}