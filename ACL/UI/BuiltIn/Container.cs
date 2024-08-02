using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI.BuiltIn;
public class Container : Component
{
    #region Properties
    // Background
    public Color BackgroundColor {get; set;} = Color.Black;
    public Texture2D BackgroundTexture {get; set;} = GameInstance.PlainTexture;

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

    public Rectangle Body {get; set;}

    public Container(GameInstance game) : base(game)
    {
        Body = new();
    }

    #region Methods
    public override void Update(GameTime gameTime)
    {
        Body = new((int)ActualPosition.X, (int)ActualPosition.Y, (int)Size.X, (int)Size.Y);

        // Update texture source rectangle
        TextureSourceRectangle = new((int)TextureSourcePosition.X, (int)TextureSourcePosition.Y, (int)TextureSourceSize.X, (int)TextureSourceSize.Y);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (Body.Width != 0 && Body.Height != 0)
        {
            if (OutlineSize > 0)
            {
                // Draw outline
                Rectangle TopOutline = new(Body.X - OutlineSize, Body.Y - OutlineSize, Body.Width + OutlineSize * 2, OutlineSize);
                Rectangle BottomOutline = new(Body.X - OutlineSize, Body.Y + Body.Height, Body.Width + OutlineSize * 2, OutlineSize);
                Rectangle LeftOutline = new(Body.X - OutlineSize, Body.Y, OutlineSize, Body.Height);
                Rectangle RightOutline = new(Body.X + Body.Width, Body.Y, OutlineSize, Body.Height);
                spriteBatch.Draw(GameInstance.PlainTexture, TopOutline, OutlineColor);
                spriteBatch.Draw(GameInstance.PlainTexture, BottomOutline, OutlineColor);
                spriteBatch.Draw(GameInstance.PlainTexture, LeftOutline, OutlineColor);
                spriteBatch.Draw(GameInstance.PlainTexture, RightOutline, OutlineColor);
            }

            // Draw body
            spriteBatch.Draw(BackgroundTexture, Body, TextureSourceRectangle, BackgroundColor);

            if (InlineSize > 0)
            {
                // Draw inline
                Rectangle TopInline = new(Body.X, Body.Y, Body.Width, InlineSize);
                Rectangle BottomInline = new(Body.X, Body.Y + Body.Height - InlineSize, Body.Width, InlineSize);
                Rectangle LeftInline = new(Body.X, Body.Y + InlineSize, InlineSize, Body.Height -  InlineSize * 2);
                Rectangle RightInline = new(Body.X + Body.Width - InlineSize, Body.Y + InlineSize, InlineSize, Body.Height -  InlineSize * 2);
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