using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ACL.Save;

namespace ACL.UI
{
    public abstract class Screen
    {
        public GameInstance Game = GameInstance.CurrentGameInstance;
        internal SpriteBatch? _spriteBatch;
        public ContentManager Content => Game.Content;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public GameServiceContainer Services => Game.Services;
        public SaveManager ScreenSaveManager => Game.SaveManager;

        //public ComponentManager ComponentManager => GameInstance.ComponentManager;
    }
}