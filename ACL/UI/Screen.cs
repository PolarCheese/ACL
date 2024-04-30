using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ACL.IO;

namespace ACL.UI
{
    public class Screen
    {
        public GameInstance Game;
        protected SpriteBatch SpriteBatch => Game.SpriteBatch;
        public ContentManager Content => Game.Content;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public GameServiceContainer Services => Game.Services;
        public ComponentManager ComponentManager => Game.ComponentManager;
        public ScreenManager ScreenManager => Game.ScreenManager;
        
        public List<Component> ScreenComponents {get; private set;} = new List<Component>();
        public void AddScreenComponents(params Component[] components)
        {
            ScreenComponents.AddRange(components);
        }

        public Screen(GameInstance CurrentGame)
        {
            Game = CurrentGame;
        }
        
        /// <summary> Triggers when this screen is loaded. </summary>
        public virtual void OnLoad(){}
        /// <summary> Triggers when this screen is unloaded. </summary>
        public virtual void OnUnload(){}
        /// <summary> Triggers when this screen set as active. </summary>
        public virtual void OnActivation(){}
        /// <summary> Triggers when this screen set as inactive. </summary>
        public virtual void OnUnactivation(){}

        public virtual void Update(GameTime gameTime){}
        public virtual void Draw(GameTime gameTime){}
    }
}