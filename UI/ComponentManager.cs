using ACL.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class ComponentManager
    {
        public GameInstance Game;
        internal SpriteBatch SpriteBatch => Game.SpriteBatch;
        internal PhysicsEngine PhysicsEngine => Game.PhysicsEngine;
        public ComponentManager(GameInstance GameInstance)
        {
            Game = GameInstance;
        }
        internal List<Component> ActiveComponents {get; private set;} = new();
        internal List<Component> PendingAdditions {get; set;} = new();
        internal List<Component> PendingRemovals {get; set;} = new();
        public List<Camera> Cameras {get; set;} = new();

        #region List Methods
        // Methods for adding components to the manager. 
        public void AddComponents(params Component[] Components)
        {
            foreach (var component in Components)
            {
                PendingAdditions.Add(component);
            }
        }
        public void AddComponentsRange(IEnumerable<Component> Components)
        {
            PendingAdditions.AddRange(Components);
        }

        // Methods for removing components from the manager. 
        public void RemoveComponents(params Component[] Components)
        {
            foreach (var component in Components)
            {
                PendingRemovals.Add(component);
            }
        }
        public void RemoveComponentsRange(IEnumerable<Component> Components)
        {
            foreach (var component in Components)
            {
                PendingRemovals.Add(component);
            }
        }

        public void Clear() // Completely clear ComponentManager of any components.
        {
            ActiveComponents.Clear();
            PhysicsEngine.PhysicsObjects.Clear(); // Remove remaining components in the physics engine.
        }
        #endregion

        #region Manage Methods

        public void DisableComponents(params Component[] Components) // Disable Update+Draw for the given components.
        {
            foreach (var component in Components)
            {
                component.ToDraw = false; component.ToUpdate = false;
            }
        }

        public void EnableComponents(params Component[] Components) // Enable Update+Draw for the given components.
        {
            foreach (var component in Components)
            {
                component.ToDraw = true; component.ToUpdate = true;
            }
        }

        public void ModifyComponents(IEnumerable<Component> Components, Action<Component> modificationAction)
        {
            // Go through every component and change a property given as a parameter to a value also given as a parameter.
            foreach(var component in Components) 
            {
                modificationAction(component);
            }
        }
        #endregion

        #region Logic Methods
        public void Update(GameTime gameTime) // Update components.
        {
            // Add pending components.
            foreach (var component in PendingAdditions)
            {
                ActiveComponents.Add(component);
                if (component is PhysicsComponent PhysicsObject)
                {
                    PhysicsEngine.AddComponent(PhysicsObject);
                }
            }
            PendingAdditions.Clear();
            
            // Update active components.
            foreach (var component in ActiveComponents)
            {
                if (component.ToUpdate == true)
                {
                    component.Update(gameTime);
                }
            }

            // Remove unwanted components.
            foreach (var component in PendingRemovals)
            {
                ActiveComponents.Remove(component);
                if (component is PhysicsComponent PhysicsObject)
                {
                    PhysicsEngine.RemoveComponent(PhysicsObject);
                }
            }
            PendingRemovals.Clear();
        }

        public void Draw(GameTime gameTime) // Draw components.
        {
            // Draw all components.
            SpriteBatch.Begin(samplerState: Game.SpritebatchSamplerState);
            foreach (var component in ActiveComponents)
            {
                if (component.ToDraw)
                {
                    component.Draw(gameTime, SpriteBatch);
                }
            }
            SpriteBatch.End();

            // Draw camera bound components.
            foreach (Camera Camera in Cameras)
            {
                SpriteBatch.Begin(samplerState: Game.SpritebatchSamplerState, transformMatrix: Camera.Transform);
                foreach (var component in Camera.SubComponents)
                {
                    if (component.ToDraw)
                    {
                        component.Draw(gameTime, SpriteBatch);
                    }
                }
                SpriteBatch.End();
            }
        }
        #endregion
    }
}