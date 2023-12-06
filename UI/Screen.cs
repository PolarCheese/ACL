

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public abstract class Screen
    {
        public Game? GameInstance;
        internal SpriteBatch? _spriteBatch;
    }
}