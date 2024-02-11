using System.Diagnostics;
using ACL.UI;
using Microsoft.Xna.Framework;

namespace ACL.Physics
{
    public abstract class PhysicsComponent : Component
    {
        // Physics
        public PhysicsEngine PhysicsEngine => Game.PhysicsEngine;
        public bool PhysicsEnabled {get; set;} = true; // Tells the Physics Engine if object should have physics.
        public Vector2 Velocity {get; set;} = new(0,0);
        public Vector2 Acceleration {get; set;} = new(0,0);
        public float Mass {get; set;} = 0f;
        public float Friction {get; set;} = 0f;
        
        public virtual void FixedUpdate() 
        {
            Velocity += Acceleration; // Update velocity
            Position.AbsoluteX += Velocity.X; Position.AbsoluteY += Velocity.Y; // Change Position
        }
        protected PhysicsComponent(GameInstance game) : base(game) {}
    }
}