using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class ComponentManager
    {
        public GameInstance Game;
        internal SpriteBatch _spriteBatch => Game.SpriteBatch;
        public ComponentManager(GameInstance CurrentGame)
        {
            Game = CurrentGame;
        }
        internal List<Component> activeComponents {get; private set;} = new List<Component>();
        internal List<Component> pendingAdditions {get; set;} = new List<Component>();
        internal List<Component> pendingRemovals {get; set;} = new List<Component>();

        #region List Methods
        // Methods for adding/removing from the list. 
        public void AddComponents(params Component[] Paramcomponents)
        {
            foreach (var component in Paramcomponents)
            {
                pendingAdditions.Add(component);
            }
        }

        public void AddComponentsRange(IEnumerable<Component> Components)
        {
            pendingAdditions.AddRange(Components);
        }

        public void RemoveComponents(params Component[] Paramcomponents)
        {
            foreach (var component in Paramcomponents)
            {
                pendingRemovals.Add(component);
            }
        }
        public void RemoveComponentsRange(IEnumerable<Component> Components)
        {
            foreach (var component in Components)
            {
                pendingRemovals.Add(component);
            }
        }

        public void Clear()
        {
            activeComponents.Clear();
        }
        #endregion

        #region Manage Methods

        public void DisableComponents(params Component[] Paramcomponents)
        {
            foreach (var component in Paramcomponents)
            {
                component.ToDraw = false; component.ToUpdate = false;
            }
        }

        public void EnableComponents(params Component[] Paramcomponents)
        {
            foreach (var component in Paramcomponents)
            {
                component.ToDraw = true; component.ToUpdate = true;
            }
        }

        public void ModifyComponents(IEnumerable<Component> Components, Action<Component> modificationAction)
        {
            // Go through every Paracomponent and change a property given as a parameter to a value also given as a parameter.
            foreach(var component in Components) 
            {
                modificationAction(component);
            }
        }
        #endregion

        #region Logic Methods
        public void Update(GameTime gameTime)
        {
            // Add pending components.
            foreach (var component in pendingAdditions)
            {
                activeComponents.Add(component);
            }
            pendingAdditions.Clear();
            
            // Update active components.
            foreach (var component in activeComponents)
            {
                if (component.ToUpdate == true)
                {
                    component.Update(gameTime);
                }
            }

            // Remove unwanted components.
            foreach (var component in pendingRemovals)
            {
                activeComponents.Remove(component);
            }
            pendingRemovals.Clear();
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var component in activeComponents)
            {
                if (component.ToDraw == true)
                {
                    component.Draw(gameTime, _spriteBatch);
                }
            }
        }
        #endregion
    }
}