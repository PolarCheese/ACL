using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ACL.Save;

namespace ACL.UI
{
    public class Screen
    {
        public GameInstance Game;
        internal SpriteBatch _spriteBatch = null!;
        public ContentManager Content => Game.Content;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public GameServiceContainer Services => Game.Services;
        public SaveManager SaveManager => Game.saveManager;
        public ComponentManager ComponentManager => Game.componentManager;

        public Screen(GameInstance CurrentGame)
        {
            Game = CurrentGame;
        }
        public void OnLoad(){}
        public void OnUnload(){}
    }
}