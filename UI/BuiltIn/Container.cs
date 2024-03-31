using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI.BuiltIn
{
    public class Container : Component
    {

        #region Properties

        // Background
        public Color BackgroundColor {get; set;} = new(0, 0, 0, 255);
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
        public Rectangle Outline {get; set;}

        public Container(GameInstance game) : base(game)
        {
            Body = new();
            Outline = new();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            Body = new((int)(Position.X - Size.X * Origin.X), (int)(Position.Y - Size.Y * Origin.Y), (int)Size.X, (int)Size.Y);

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
                    Outline = new(Body.X - OutlineSize/2, Body.Y - OutlineSize/2, Body.Width + OutlineSize, Body.Height + OutlineSize);
                    spriteBatch.Draw(GameInstance.PlainTexture, Outline, OutlineColor);
                }
                // Draw body
                spriteBatch.Draw(BackgroundTexture, Body, TextureSourceRectangle, BackgroundColor);
            }
            base.Draw(gameTime, spriteBatch);
        }
        #endregion
    }
}