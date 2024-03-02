using ACL.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public class ComponentManager
    {
        public GameInstance Game;
        protected SpriteBatch SpriteBatch => Game.SpriteBatch;
        protected PhysicsEngine PhysicsEngine => Game.PhysicsEngine;
        public ComponentManager(GameInstance GameInstance)
        {
            Game = GameInstance;
        }
        protected List<Component> SubComponents {get; private set;} = new();
        protected List<Component> PendingAdditions {get; set;} = new();
        protected List<Component> PendingRemovals {get; set;} = new();
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

        // Methods for managing cameras.
        public void AddCamera(Camera camera)
        {
            Cameras.Add(camera);
        }
        public void RemoveCamera(Camera camera)
        {
            Cameras.Remove(camera);
        }

        public void Clear() // Completely clear ComponentManager of any components and cameras.
        {
            SubComponents.Clear();
            PhysicsEngine.Clear(); // Remove remaining components in the physics engine.
            Cameras.Clear();
        }
        #endregion

        #region Manage Methods

        // Methods for disabling Update+Draw for components.
        public void DisableComponents(params Component[] Components)
        {
            foreach (var component in Components)
            {
                component.ToDraw = false; component.ToUpdate = false;
            }
        }

        public void DisableComponents(IEnumerable<Component> Components)
        {
            foreach (var component in Components)
            {
                component.ToDraw = false; component.ToUpdate = false;
            }
        }

        // Methods for enabling Update+Draw for components.
        public void EnableComponents(params Component[] Components)
        {
            foreach (var component in Components)
            {
                component.ToDraw = true; component.ToUpdate = true;
            }
        }

        public void EnableComponents(IEnumerable<Component> Components)
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
                SubComponents.Add(component);
                if (component is PhysicsComponent PhysicsObject)
                {
                    PhysicsEngine.AddComponent(PhysicsObject);
                }
            }
            PendingAdditions.Clear();
            
            // Update active components.
            foreach (var component in SubComponents)
            {
                if (component.ToUpdate)
                {
                    component.Update(gameTime);
                }
            }

            // Remove unwanted components.
            foreach (var component in PendingRemovals)
            {
                SubComponents.Remove(component);
                if (component is PhysicsComponent PhysicsObject)
                {
                    PhysicsEngine.RemoveComponent(PhysicsObject);
                }
            }
            PendingRemovals.Clear();

            // Update cameras.
            foreach (Camera camera in Cameras)
            {
                camera.Update(); // Update camera.

                if (camera.Enabled) {
                    foreach (Component component in camera.PendingAdditions) // Add subcomponents.
                    {
                        camera.SubComponents.Add(component);
                    }
                    camera.PendingAdditions.Clear();

                    foreach (Component component in camera.SubComponents) // Update subcomponents
                    {
                        if (component.ToUpdate)
                        {
                            component.Update(gameTime);
                        }
                    }

                    foreach (Component component in camera.PendingRemovals) // Remove unwanted subcomponents.
                    {
                        camera.SubComponents.Remove(component);
                    }
                    camera.PendingRemovals.Clear();
                }
            }
        }

        public void Draw(GameTime gameTime) // Draw components.
        {
            // Draw all components.
            SpriteBatch.Begin(samplerState: Game.SpritebatchSamplerState);
            foreach (var component in SubComponents)
            {
                if (component.ToDraw)
                {
                    component.Draw(gameTime, SpriteBatch);
                }
            }
            SpriteBatch.End();

            // Draw camera bound components.
            foreach (Camera camera in Cameras)
            {
                if (camera.Enabled) {
                    camera.Draw(); // Calculates the position before drawing. This is to avoid the camera "stuttering" behind the target's position.
                    SpriteBatch.Begin(samplerState: Game.SpritebatchSamplerState, transformMatrix: camera.GetTransform());
                    foreach (var component in camera.SubComponents)
                    {
                        if (component.ToDraw)
                        {
                            component.Draw(gameTime, SpriteBatch);
                        }
                    }
                    SpriteBatch.End();
                }
            }
        }

        public void Resize()
        {
            // Resize all components which need to be rescaled.
            /*
            foreach (var component in SubComponents)
            {
                // Not Implemented.
            }
            */
        }
        #endregion
    }
}