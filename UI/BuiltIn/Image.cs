using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI.BuiltIn
{
    public class Image : Component
    {
        #region Properties

        // Image
        public Color ImageColor {get; set;} = Color.White;
        public Color ImageBackgroundColor {get; set;} // If the image is transparent, this will be the color shown behind.
        public Texture2D ImageTexture {get; set;} = GameInstance.PlainTexture;

        // Outline/Inline
        public Color OutlineColor {get; set;} = Color.White;
        public Color InlineColor {get; set;} = Color.White;

        public int OutlineSize {get; set;} = 0;
        public int InlineSize {get; set;} = 0;

        // Texturing
        public Rectangle TextureSourceRectangle {get; set;}
        public Vector2 TextureSourcePosition {get; set;} = Vector2.Zero;
        public Vector2 TextureSourceSize {get; set;} = Vector2.One;

        #endregion

        public Rectangle ImageBounds {get; set;}
        public Rectangle Outline {get; set;}

        public Image(GameInstance game) : base(game)
        {
            ImageBounds = new();
            Outline = new();
            TextureSourceRectangle = new();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            ImageBounds = new((int)(Position.X - Size.X * Origin.X), (int)(Position.Y - Size.Y * Origin.Y), (int)Size.X, (int)Size.Y);
            
            // Update texture source rectangle
            TextureSourceRectangle = new((int)TextureSourcePosition.X, (int)TextureSourcePosition.Y, (int)TextureSourceSize.X, (int)TextureSourceSize.Y);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (ImageBounds.Width != 0 && ImageBounds.Height != 0)
            {
                if (OutlineSize > 0)
                {
                    // Draw outline
                    Outline = new(ImageBounds.X - OutlineSize, ImageBounds.Y - OutlineSize, ImageBounds.Width + 2*OutlineSize, ImageBounds.Height + 2*OutlineSize);
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
        #endregion
    }
}