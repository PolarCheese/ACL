using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI.BuiltIn;
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
    Rectangle TextureSourceRectangle {get; set;}
    public Vector2 TextureSourcePosition {get; set;} = Vector2.Zero;
    public Vector2 TextureSourceSize {get; set;} = Vector2.One;

    #endregion

    public Rectangle ImageBounds {get; set;}

    public Image(GameInstance game) : base(game)
    {
        ImageBounds = new();
        TextureSourceRectangle = new();
    }

    #region Methods

    public override void Update(GameTime gameTime)
    {
        ImageBounds = new((int)ActualPosition.X, (int)ActualPosition.Y, (int)Size.X, (int)Size.Y);
            
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
                Rectangle TopOutline = new(ImageBounds.X - OutlineSize, ImageBounds.Y - OutlineSize, ImageBounds.Width + OutlineSize * 2, OutlineSize);
                Rectangle BottomOutline = new(ImageBounds.X - OutlineSize, ImageBounds.Y + ImageBounds.Height, ImageBounds.Width + OutlineSize * 2, OutlineSize);
                Rectangle LeftOutline = new(ImageBounds.X - OutlineSize, ImageBounds.Y, OutlineSize, ImageBounds.Height);
                Rectangle RightOutline = new(ImageBounds.X + ImageBounds.Width, ImageBounds.Y, OutlineSize, ImageBounds.Height);
                spriteBatch.Draw(GameInstance.PlainTexture, TopOutline, OutlineColor);
                spriteBatch.Draw(GameInstance.PlainTexture, BottomOutline, OutlineColor);
                spriteBatch.Draw(GameInstance.PlainTexture, LeftOutline, OutlineColor);
                spriteBatch.Draw(GameInstance.PlainTexture, RightOutline, OutlineColor);
            }

            if (ImageBackgroundColor.A > 0)
            {
                // Draw Background
                spriteBatch.Draw(GameInstance.PlainTexture, ImageBounds, ImageBackgroundColor);
            }

            // Draw Image
            spriteBatch.Draw(ImageTexture, ImageBounds, TextureSourceRectangle, ImageColor);

            if (InlineSize > 0)
            {
                // Draw inline
                Rectangle TopInline = new(ImageBounds.X, ImageBounds.Y, ImageBounds.Width, InlineSize);
                Rectangle BottomInline = new(ImageBounds.X, ImageBounds.Y + ImageBounds.Height - InlineSize, ImageBounds.Width, InlineSize);
                Rectangle LeftInline = new(ImageBounds.X, ImageBounds.Y + InlineSize, InlineSize, ImageBounds.Height -  InlineSize * 2);
                Rectangle RightInline = new(ImageBounds.X + ImageBounds.Width - InlineSize, ImageBounds.Y + InlineSize, InlineSize, ImageBounds.Height -  InlineSize * 2);
                spriteBatch.Draw(GameInstance.PlainTexture, TopInline, InlineColor);
                spriteBatch.Draw(GameInstance.PlainTexture, BottomInline, InlineColor);
                spriteBatch.Draw(GameInstance.PlainTexture, LeftInline, InlineColor);
                spriteBatch.Draw(GameInstance.PlainTexture, RightInline, InlineColor);
            }
        }
        base.Draw(gameTime, spriteBatch);
    }
    #endregion
}