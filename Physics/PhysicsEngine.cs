using Microsoft.Xna.Framework.Graphics;
using ACL.IO;
using ACL.UI;

namespace ACL.Physics
{
    public class PhysicsEngine
    {
        readonly GameInstance Game;
        ComponentManager ComponentManager => Game.ComponentManager;
        FileManager FileManager => Game.FileManager;
        SpriteBatch Spritebatch => Game.SpriteBatch;
        public List<PhysicsComponent> PhysicsObjects = new();
        public List<PhysicsComponent> PendingObjects = new(); // Objects that will be added next Fixed Update call.
        public List<PhysicsComponent> RemovableObjects = new(); // Objects that will be removed next Fixed Update call.
        private HashSet<int> CheckedPairs = new();
        public PhysicsEngine(GameInstance CurrentGame)
        {
            Game = CurrentGame;
            Game.FixedUpdateEvent += FixedUpdate;
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

        private int GetPairHash(PhysicsComponent objectA, PhysicsComponent objectB) // Get a hash value from a pair of Dynamic Components.
        {
            // Generate a unique hash value for the object pair
            int hash = objectA.GetHashCode() ^ objectB.GetHashCode();
            return hash;
        }
        #endregion

        #region Physics Methods
        // Methods used for calculating physics, resolving collisions etc.
        public void FixedUpdate(object? sender, EventArgs e)
        {
            // Add pending physics objects.
            foreach (var Object in PendingObjects)
            {
                PhysicsObjects.Add(Object);
            }
            PendingObjects.Clear();

            // Calculate Physics ..

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