using ACL.UI;
using Microsoft.Xna.Framework;

namespace ACL.Physics
{
    public abstract class PhysicsComponent : Component
    {
        // Physics
        public PhysicsEngine PhysicsEngine => Game.PhysicsEngine;
        public bool PhysicsEnabled {get; set;} = true; // Tells the Physics Engine if object should have physics.
        public Vector2 LinearVelocity {get; set;} = Vector2.Zero;
        public float RotationalVelocity {get; set;} = 0f;
        public Vector2 Acceleration {get; set;} = Vector2.Zero;
        public float Mass {get; set;} = 0f;
        public float Friction {get; set;} = 0f;
        public ShapeType CollisionType {get; set;} = ShapeType.Box;
        
        public virtual void FixedUpdate() 
        {
            LinearVelocity += Acceleration;
            Position += LinearVelocity;
            Rotation += RotationalVelocity;
        }
        protected PhysicsComponent(GameInstance game) : base(game) {}
    }

    public enum ShapeType
    {
        Box = 0,
        Circle = 1,
        Polygon = 2
    }
}