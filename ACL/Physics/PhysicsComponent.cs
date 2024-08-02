using ACL.UI;
using Microsoft.Xna.Framework;

namespace ACL.Physics;
public abstract class PhysicsComponent : Component
{
    // Physics
    public bool PhysicsEnabled {get; set;} = true; // Tells the Physics Engine if object should have physics.
    public Environment? Environment { get; set; } // Physics object's environment.
    public List<Vector2> Forces {get; set;} = new(); // Forces applied specifically to this component.
    public event EventHandler<TouchEventArgs>? Touched; // Event for when the object is touched by another object.

    // Velocity & Acceleration
    public Vector2 LinearVelocity {get; set;} = Vector2.Zero;
    public float RotationalVelocity {get; set;} = 0f;
    public Vector2 Acceleration {get; set;} = Vector2.Zero;

    // Properties
    public float Mass {get; set;} = 0f;
    public float Friction {get; set;} = 0f;
    public ShapeType CollisionType {get; set;} = ShapeType.Box;
        
    protected PhysicsComponent(GameInstance game) : base(game) 
    {
        Environment = null;
    }
    protected PhysicsComponent(GameInstance game, Environment environment) : base(game) 
    {
        Environment = environment;
    }

    public virtual void FixedUpdate() 
    {
        // Calculate acceleration from all forces.
        Acceleration = GetAcceleration();

        // Calculate velocity from acceleration
        LinearVelocity += Acceleration;
        Position += LinearVelocity;
        Rotation += RotationalVelocity;
    }

    public virtual Vector2 GetAcceleration()
    {
        Vector2 Sum = Vector2.Zero;
        if (Environment != null)
        {
            // Apply environment forces.
            foreach(Vector2 Force in Environment.Forces)
            {
                Sum += Force;
            }
        }
        // Apply component specific forces.
        foreach(Vector2 Force in Forces)
        {
            Sum += Force;
        }

        return Sum/Mass; // This is the acceleration.
    }
}

public enum ShapeType
{
    Box = 0,
    Circle = 1,
    Polygon = 2
}