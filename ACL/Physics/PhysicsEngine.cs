using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ACL.UI;

namespace ACL.Physics;
public class PhysicsEngine
{
    readonly GameInstance Game;
    protected ComponentManager ComponentManager => Game.ComponentManager;
    protected SpriteBatch Spritebatch => Game.SpriteBatch;
    public QuadTree RootQuadTree {get; set;}

    // Objects
    protected List<PhysicsComponent> PhysicsObjects = new(); // Objects currently updated by the physics engine.
    protected List<PhysicsComponent> PendingObjects = new(); // Objects that will be added next Fixed Update call.
    protected List<PhysicsComponent> RemovableObjects = new(); // Objects that will be removed next Fixed Update call.
    private HashSet<int> CheckedPairs = new();

    // Properties
    public int Quadtree_Size = (int)Math.Pow(2, 12);
    public bool SkipPreciseCollisionStep = false; // may be useful later, does nothing for now.

    public bool DebugMode = false;
    public Camera? DebugCamera;
    public SpriteFont? DebugFont {get; set;}

    public PhysicsEngine(GameInstance CurrentGame)
    {
        Game = CurrentGame;
        RootQuadTree = new(0, CreateQuadtreeBounds(Quadtree_Size));
    }

    #region Component methods
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

    #endregion

    #region Physics methods

    int GetPairHash(PhysicsComponent objectA, PhysicsComponent objectB) // Generate a unique hash value for an object pair
    {
        int hashA = objectA.GetHashCode(); int hashB = objectB.GetHashCode();
        int hash = hashA > hashB ? hashA + hashB * 17 : hashB + hashA * 17;
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

    private void CheckQuadtreeNode(QuadTree Node)
    {
        List<PhysicsComponent> objs = new(); // List used to retrieve objects from node
        foreach (PhysicsComponent objA in Node.Objects)
        {
            Node.Retrieve(objs, objA); // Retrieve all objects that could collide with this one
            foreach(PhysicsComponent objB in objs)
            {
                // Check for collision
                CheckPair(objA, objB);
                //Debug.WriteLine($"Checking quadtree pair {GetPairHash(objA, objB)} (depth {Node.Depth} | {Node.Objects.Count} objects )");
            }
            objs.Clear();
        }
    }

    public void FixedUpdate(GameTime gameTime) // Repeats each cycle.
    {
        // Redo quadtree
        RootQuadTree.Clear();
        RootQuadTree.Bounds = CreateQuadtreeBounds(Quadtree_Size);

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

        // Check for collisions
        CheckQuadtreeNode(RootQuadTree);
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

    private void BroadCollisionCheck(PhysicsComponent objectA, PhysicsComponent objectB) // AABB
    {
        Vector2[] aPoints = GetAABB(objectA); Vector2[] bPoints = GetAABB(objectB); // Fetch corners from both objects
        
        // Implement SAT

        ResolveCollision(objectA, objectB);
    }

    private void ResolveCollision(PhysicsComponent objectA, PhysicsComponent objectB)
    {
        // ..
    }
    #endregion

    #region AABB methods

    public Vector2[] GetAABB(Component Object)
    {
        // Get bound points (works if component properties have been correctly implemented)
        var Points = new Vector2[4]; Vector2 origin = Object.ActualPosition + Object.Size * Object.Origin;
        Points[0] = new(Object.ActualPosition.X, Object.ActualPosition.Y);  // top-left
        Points[1] = new(Object.ActualPosition.X + Object.Size.X, Object.ActualPosition.Y);  // top-right
        Points[2] = new(Object.ActualPosition.X, Object.ActualPosition.Y + Object.Size.Y);  // bottom-left
        Points[3] = new(Object.ActualPosition.X + Object.Size.X, Object.ActualPosition.Y + Object.Size.Y);  // bottom-right

        // Check if points are rotated by a number indivisible by 90 degrees
        if (Object.ActualRotation % 90 == 0)
        {
            // Rotate points round object origin (so it's actually OBB and not AABB)
            var RotatedPoints = new Vector2[4];
            RotatedPoints[0] = RotatePoint(Points[0], origin, Object.ActualRotation);
            RotatedPoints[1] = RotatePoint(Points[1], origin, Object.ActualRotation);
            RotatedPoints[2] = RotatePoint(Points[2], origin, Object.ActualRotation);
            RotatedPoints[3] = RotatePoint(Points[3], origin, Object.ActualRotation);
            return RotatedPoints;
        }

        return Points;
    }

    private Vector2 RotatePoint(Vector2 point, Vector2 origin, float rotation) // rotate a point around an origin
    {
        Vector2 rotatedPoint = new(origin.X + (point.X - origin.X) * MathF.Cos(rotation) - (point.Y - origin.Y) * MathF.Sin(rotation), origin.Y + (point.X - origin.X) * MathF.Sin(rotation) + (point.Y - origin.Y) * MathF.Cos(rotation));
        return rotatedPoint;
    }

    #endregion

    #region Quadtree methods

    private Rectangle CreateQuadtreeBounds(int size)
    {
        return new(-size/2, -size/2, size, size);
    }

    public void Quadtree_SetMaxObjects(int maxObjects)
    {
        RootQuadTree.MaxComponents = maxObjects > 0 ? maxObjects : 1;
    }

    public void Quadtree_SetMaxDepth(int maxDepth)
    {
        RootQuadTree.MaxTreeDepth = maxDepth;
    }
    #endregion
}

public class TouchEventArgs : EventArgs
{
	public PhysicsComponent TouchingObject { get; set; } // The other object

    public TouchEventArgs(PhysicsComponent touchingObject)
    {
        TouchingObject = touchingObject;
    }
}