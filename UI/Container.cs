using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class Container : Component
    {

        #region Properties

        // Background
        public Color BackgroundColor { get; set; } = new(0, 0, 0, 255);
        public Texture2D BackgroundTexture { get; set; } = GameInstance.PlainTexture;

        // Outline/Inline
        public Color OutlineColor { get; set; } = Color.White;
        public Color InlineColor { get; set; } = Color.White;

        public int OutlineSize { get; set; } = 0;
        public int InlineSize { get; set; } = 0;

        // Texturing
        public Rectangle TextureSourceRectangle { get; set; }
        public QuadVector TextureSourcePosition { get; set; } = new(0, 0, 0, 0);
        public QuadVector TextureSourceSize { get; set; } = new(1, 1, 0, 0);

        #endregion

        public Rectangle Body { get; set; }
        public Rectangle Outline { get; set; }

        public Container(GameInstance game) : base(game)
        {
            Body = new();
            Outline = new();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            UpdateSourceRectangles();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Body = new((int)ActualPosition.X, (int)ActualPosition.Y, (int)ActualSize.X, (int)ActualSize.Y);
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
        
        public virtual void UpdateSourceRectangles()
        {
            Rectangle TextureBounds = new(0, 0, BackgroundTexture.Width, BackgroundTexture.Height);
            Vector2 Position = TextureSourcePosition.ToVector2(TextureBounds); Vector2 Size = TextureSourceSize.ToVector2(TextureBounds);
            TextureSourceRectangle = new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        #endregion
    }
}