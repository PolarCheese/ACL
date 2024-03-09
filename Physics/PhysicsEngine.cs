using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ACL.IO;
using ACL.UI;

namespace ACL.Physics
{
    public class PhysicsEngine
    {
        readonly GameInstance Game;
        protected ComponentManager ComponentManager => Game.ComponentManager;
        protected FileManager FileManager => Game.FileManager;
        protected SpriteBatch Spritebatch => Game.SpriteBatch;

        // Objects
        protected List<PhysicsComponent> PhysicsObjects = new(); // Objects currently updated by the physics engine.
        protected List<PhysicsComponent> PendingObjects = new(); // Objects that will be added next Fixed Update call.
        protected List<PhysicsComponent> RemovableObjects = new(); // Objects that will be removed next Fixed Update call.
        private HashSet<int> CheckedPairs = new();

        // Properties
        public PhysicsEngine(GameInstance CurrentGame)
        {
            Game = CurrentGame;
        }

        #region Processing Methods
        // Methods used for processing (Checking hash sets, grouping objects etc.)
        public void AddComponent(params PhysicsComponent[] Objects) // Add from list
        {
            foreach (var Object in Objects)
            {
                PendingObjects.Add(Object);
            }
        }

        public void RemoveComponent(params PhysicsComponent[] Objects) // Remove from list
        {
            foreach (var Object in Objects)
            {
                RemovableObjects.Add(Object);
            }
        }

        public void Clear()
        {
            PhysicsObjects.Clear();
        }

        int GetPairHash(PhysicsComponent objectA, PhysicsComponent objectB) // Get a hash value from a pair of Dynamic Components.
        {
            // Generate a unique hash value for the object pair
            int hash = objectA.GetHashCode() ^ objectB.GetHashCode();
            return hash;
        }
        #endregion

        #region Physics Methods
        // Methods used for calculating physics, resolving collisions etc.
        public void FixedUpdate(GameTime gameTime)
        {
            // Add pending physics objects.
            foreach (var Object in PendingObjects)
            {
                PhysicsObjects.Add(Object);
            }
            PendingObjects.Clear();

            // Update all object positions
            foreach (var Object in PhysicsObjects)
            {
                if (Object.PhysicsEnabled) {
                    Object.FixedUpdate();
                }
            }

            // Remove unwanted physics objects.
            foreach (var Object in RemovableObjects)
            {
                PhysicsObjects.Remove(Object);
            }
            RemovableObjects.Clear();
        }

        public void Draw(SpriteFont font)
        {
            // This method will be used for debugging later.

        }

        #endregion

        #region Texture Methods
        // Methods used for procesing Texture data (Getting texture format type, converting to lines etc.)

        #endregion
    }
}