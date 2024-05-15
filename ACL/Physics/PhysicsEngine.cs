using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ACL.UI;

namespace ACL.Physics
{
    public class PhysicsEngine
    {
        readonly GameInstance Game;
        protected ComponentManager ComponentManager => Game.ComponentManager;
        protected SpriteBatch Spritebatch => Game.SpriteBatch;

        // Debug
        public bool DebugMode = false;
        public SpriteFont? DebugFont {get; set;}
        public Camera? DebugCamera {get; set;}

        // Objects
        protected List<PhysicsComponent> PhysicsObjects = new(); // Objects currently updated by the physics engine.
        protected List<PhysicsComponent> PendingObjects = new(); // Objects that will be added next Fixed Update call.
        protected List<PhysicsComponent> RemovableObjects = new(); // Objects that will be removed next Fixed Update call.
        private HashSet<int> CheckedPairs = new();

        public QuadTree RootQuadTree {get; set;}

        // Properties
        public int MaxComponentsPerNode = 4;
        public int MaxTreeDepth = 4;
        public int MaxIterations = 4;
        public int RootQuadtreeSize = (int)Math.Pow(2, 32);
        public bool SkipPreciseCollisionStep = false;

        public PhysicsEngine(GameInstance CurrentGame)
        {
            Game = CurrentGame;
            RootQuadTree = new(this, 0, CreateRootQuadtreeBounds());
        }

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

        public void Clear() // Remove all objects
        {
            PhysicsObjects.Clear();
        }

        int GetPairHash(PhysicsComponent objectA, PhysicsComponent objectB) // Get a hash value from a pair of physics objects
        {
            // Generate a unique hash value for the object pair
            int hash = objectA.GetHashCode() ^ objectB.GetHashCode();
            return hash;
        }

        private void CheckPair(PhysicsComponent objectA, PhysicsComponent objectB) // Check if a pair of objects was already checked
        {
            int pairHash = GetPairHash(objectA, objectB);
            if (!CheckedPairs.Contains(pairHash))
            {
                // Check pair for collisions
                BroadCollisionCheck(objectA, objectB);

                // Mark hash as checked.
                CheckedPairs.Add(pairHash);
            }
        }

        public void FixedUpdate(GameTime gameTime) // Repeats each cycle.
        {
            // Redo quadtree
            RootQuadTree.Clear();
            RootQuadTree.Bounds = CreateRootQuadtreeBounds();

            // Add pending physics objects.
            foreach (var Object in PendingObjects)
            {
                PhysicsObjects.Add(Object);
            }
            PendingObjects.Clear();

            // Remove unwanted physics objects.
            foreach (var Object in RemovableObjects)
            {
                PhysicsObjects.Remove(Object);
            }
            RemovableObjects.Clear();

            // Update all object positions
            foreach (var Object in PhysicsObjects)
            {
                if (Object.PhysicsEnabled) {
                    Object.FixedUpdate();
                    RootQuadTree.Insert(Object);
                }
            }

            // Clear checked pairs
            CheckedPairs.Clear();
            // Calculate collisions
            
        }

        public void Draw()
        {
            if (DebugCamera != null && DebugFont != null && RootQuadTree != null)
            {
                // Draw Quadtrees
                Spritebatch.Begin(samplerState: Game.SpritebatchSamplerState, transformMatrix: DebugCamera.GetTransform());
                RootQuadTree.Draw(Spritebatch, DebugFont);
                Spritebatch.End();
            }
        }

        private void BroadCollisionCheck(PhysicsComponent objectA, PhysicsComponent objectB) // AABB broad collision check
        {
            Rectangle BoundA = new((int)objectA.Position.X, (int)objectA.Position.Y, (int)objectA.Size.X, (int)objectA.Size.Y);
            Rectangle BoundB = new((int)objectB.Position.X, (int)objectB.Position.Y, (int)objectB.Size.X, (int)objectB.Size.Y);

            if (BoundA.Intersects(BoundB))
            {
                // AABB Collsion !!
                if (!SkipPreciseCollisionStep) {  }
                else
                {
                    // Do a precise collision check
                    bool Collision = PreciseCollisionCheck(objectA, objectB);
                    if (Collision) { ResolveCollision(); }
                }
            }
        }

        private bool PreciseCollisionCheck(PhysicsComponent objectA, PhysicsComponent objectB) // Phase 2 of collision checking
        {
            return false;
        }

        private void ResolveCollision()
        {
            // ..
        }

        private Rectangle CreateRootQuadtreeBounds()
        {
            int size = RootQuadtreeSize;
            return new(-size/2, -size/2, size, size);
        }
    }
}