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

    #region Hash methods
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
    #endregion
    
    #region Logic Methods
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
        Vector2[] aPoints = Collisions.GetRectangleVertices(objectA); Vector2[] bPoints = Collisions.GetRectangleVertices(objectB); // Fetch corners from both objects
        if(Collisions.IntersectPolygons(aPoints, bPoints))
        {
            System.Diagnostics.Debug.WriteLine("Polygons intersected.");
            ResolveCollision(objectA, objectB);
        }
    }

    private void ResolveCollision(PhysicsComponent objectA, PhysicsComponent objectB)
    {
        // ..
    }
    #endregion

    #region Quadtree methods
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