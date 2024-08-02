using ACL.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI;
public class ComponentManager
{
    public GameInstance Game;
    protected SpriteBatch SpriteBatch => Game.SpriteBatch;
    protected PhysicsEngine PhysicsEngine => Game.PhysicsEngine;
    public ComponentManager(GameInstance GameInstance)
    {
        Game = GameInstance;
        UpdateWindowSize();
    }
    protected List<Component> Components {get; private set;} = new();
    protected List<Component> PendingAdditions {get; set;} = new();
    protected List<Component> PendingRemovals {get; set;} = new();
    public List<Camera> Cameras {get; set;} = new();

    public int[] GameWindowSize {get; private set;} = null!; 

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
        Components.Clear();
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
        Components.Add(component);
        if (component is PhysicsComponent PhysicsObject)
        {
                PhysicsEngine.AddComponent(PhysicsObject);
            }
        }
        PendingAdditions.Clear();

        // Remove unwanted components.
        foreach (var component in PendingRemovals)
        {
            Components.Remove(component);
            if (component is PhysicsComponent PhysicsObject)
            {
                PhysicsEngine.RemoveComponent(PhysicsObject);
            }
        }
        PendingRemovals.Clear();
            
        // Update active components.
        foreach (var component in Components)
        {
            if (component.ToUpdate)
            {
                component.Update(gameTime);
            }
        }

        // Update cameras.
        foreach (Camera camera in Cameras)
        {
            camera.Update(); // Update camera.

            if (camera.Enabled) {
                foreach (Component component in camera.PendingAdditions) // Add subcomponents.
                {
                    camera.Subcomponents.Add(component);
                }
                camera.PendingAdditions.Clear();

                foreach (Component component in camera.PendingRemovals) // Remove unwanted subcomponents.
                {
                    camera.Subcomponents.Remove(component);
                }
                camera.PendingRemovals.Clear();

                foreach (Component component in camera.Subcomponents) // Update subcomponents
                {
                    if (component.ToUpdate)
                    {
                        component.Update(gameTime);
                    }
                }
            }
        }
    }

    public void Draw(GameTime gameTime) // Draw components.
    {   
        // Order components based off depth.
        var SortedComponents = Components.OrderBy(c => c.Depth);

        // Draw all components.
        SpriteBatch.Begin(samplerState: Game.SpritebatchSamplerState);
        foreach (var component in SortedComponents)
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
                camera.Recenter(); // This is here to avoid the camera lagging/stuttering behind the target's position.
                var SortedCameraComponents = camera.Subcomponents.OrderBy(c => c.Depth);

                SpriteBatch.Begin(samplerState: Game.SpritebatchSamplerState, transformMatrix: camera.GetTransform());
                foreach (var component in SortedCameraComponents)
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
        // Get the new Window Size.
        int[] newWindowSize = Game.GetWindowSize();
        float xDifference = (float)newWindowSize[0]/GameWindowSize[0];
        float yDifference = (float)newWindowSize[1]/GameWindowSize[1];
        Vector2 resizeVector = new(xDifference, yDifference);

        // Resize components in the component manager.
        foreach (var component in Components)
        {
            if (component.AllowResizing)
            {
                // Resize the component.
                component.Position *= resizeVector; component.Size *= resizeVector;
            }
        }

        // Resize components in camera instances.
        foreach (var camera in Cameras)
        {
            if (camera.AllowComponentResize)
            {
                foreach(var component in camera.Subcomponents)
                {
                    // Resize the component.
                    component.Position *= resizeVector; component.Size *= resizeVector;
                }
            }
        }

        GameWindowSize = newWindowSize;
    }

    public void UpdateWindowSize()
    {
        // This method is for forcefully updating the component manager's GameWindowSize values (because using SetBufferSize does not trigger the Window_ClientSizeChanged method in GameInstance)
        GameWindowSize = Game.GetWindowSize();
    }
    #endregion
}