using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI.BuiltIn;
public class Text : Component
{
    #region Properties
    public string? Content {get; set;}
    public Color TextColor {get; set;} = Color.White;
    public float TextScale {get; set;} = 1f;
    public SpriteFont? TextFont {get; set;}
    #endregion

    public Text(GameInstance game) : base(game) {}

    #region Methods
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (TextFont != null)
        {
            // Center text to origin
            Vector2 renderPosition = ActualPosition - TextFont.MeasureString(Content) * TextScale * Origin;

            spriteBatch.DrawString(TextFont, Content, renderPosition, TextColor, Rotation, Vector2.Zero, TextScale, SpriteEffects.None, 0);
        }

        base.Draw(gameTime, spriteBatch);
    }
    #endregion
}