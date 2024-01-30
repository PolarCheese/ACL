using ACL.UI;
using Microsoft.Xna.Framework;

namespace ACL.Physics
{
    public abstract class PhysicsComponent : Component
    {
        // Physics
        public PhysicsEngine PhysicsEngine => Game.PhysicsEngine;
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public float Friction { get; set; }
        public bool PhysicsEnabled {get; set;} = true; // Tells the Physics Engine if object should have physics.

        public abstract void FixedUpdate();
        protected PhysicsComponent(GameInstance game) : base(game)
        {
            
        }
    }
}