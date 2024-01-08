using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class Container : Component
    {

        #region Properties

        // Background
        public Color BackgroundColor { get; set; } = new Color(0, 0, 0, 255);
        public Texture2D BackgroundTexture {get; set;} = GameInstance.PlainTexture;

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

        public Rectangle Body { get; set; }
        public Rectangle Outline { get; set; }

        public Container(GameInstance game) : base(game)
        {
            Body = new Rectangle();
            Outline = new Rectangle();
        }

        #region Methods

        public override void Update(GameTime gameTime)
        {
            UpdateSourceRectangles();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Convert position, size and rotation.
            Vector2 ConvertedPosition = new Vector2(); Vector2 ConvertedSize = new Vector2();

            if (Parent != null)
            {
                if (Parent.PositionChildrenToParent) {ConvertedPosition = Position.ConvertToBound(Parent.GetBounds());}
                if (Parent.SizeChildrenToParent)  {ConvertedSize = Size.ConvertToBound(Parent.GetBounds());}
                if (Parent.RotateChildrenToParent) {Rotation = Rotation + Parent.Rotation;};
            }
            // Use game as bounds. 
            else {ConvertedPosition = Position.ConvertToScreenPosition(Game); ConvertedSize = Size.ConvertToScreenPosition(Game);}

            Body = new Rectangle((int)(ConvertedPosition.X - ConvertedSize.X * Origin.X), (int)(ConvertedPosition.Y - ConvertedSize.Y * Origin.Y), (int)ConvertedSize.X, (int)ConvertedSize.Y);
            if (Body.Width != 0 && Body.Height != 0)
            {
                if (OutlineSize > 0)
                {
                    // Draw outline
                    Outline = new Rectangle(Body.X - OutlineSize/2, Body.Y - OutlineSize/2, Body.Width + OutlineSize, Body.Height + OutlineSize);
                    spriteBatch.Draw(GameInstance.PlainTexture, Outline, OutlineColor);
                }
                // Draw body
                spriteBatch.Draw(BackgroundTexture, Body, TextureSourceRectangle, BackgroundColor);
            }
            base.Draw(gameTime, spriteBatch);
        }
        
        public void UpdateSourceRectangles()
        {
            Rectangle TextureBounds = new Rectangle(0, 0, BackgroundTexture.Width, BackgroundTexture.Height);
            Vector2 Position = TextureSourcePosition.ConvertToBound(TextureBounds); Vector2 Size = TextureSourceSize.ConvertToBound(TextureBounds);
            TextureSourceRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        #endregion
    }
}