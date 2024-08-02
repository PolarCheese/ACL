using ACL.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.Physics;
public class QuadTree
{
    #region Properties and Fields
    public int MaxComponents = 8;
    public int MaxTreeDepth = 4;
    public int Depth;
    public Rectangle Bounds;
    public List<PhysicsComponent> Objects;
    public QuadTree[] Nodes;

    public QuadTree(int depth, Rectangle bounds)
    {
        Depth = depth;
        Bounds = bounds;
        Objects = new();
        Nodes = new QuadTree[4];
    }
    #endregion

    #region Methods
    public void Insert(PhysicsComponent physicsComponent) // Adds a physics component to the quadtree.
    {
        if (Nodes[0] != null) // Node check
        {
            int index = GetIndex(physicsComponent); // Find where the object hitbox fits.
            if (index != -1)
            {
                Nodes[index].Insert(physicsComponent);
                return;
            }
        }
        
            Objects.Add(physicsComponent);

        if (Objects.Count > MaxComponents && Depth < MaxTreeDepth) 
        {
            if (Nodes[0] == null) Split(); // Limits reached, split this quadtree.

            int i = 0;
            while (i < Objects.Count)
            {
                int index = GetIndex(Objects[i]);
                if (index != -1)
                {
                    Nodes[index].Insert(Objects[i]);
                    Objects.RemoveAt(i);
                }
                else i++;
            }
        }
    }

    private void Split() // Split the quadtree into 4 different quadrants.
    {
        int subWidth = Bounds.Width / 2;
        int subHeight = Bounds.Height / 2;
        int x = Bounds.X;
        int y = Bounds.Y;

        Nodes[0] = new QuadTree(Depth + 1, new Rectangle(x, y, subWidth, subHeight)); //top-left
        Nodes[1] = new QuadTree(Depth + 1, new Rectangle(x + subWidth, y, subWidth, subHeight)); //top-right
        Nodes[2] = new QuadTree(Depth + 1, new Rectangle(x, y + subHeight, subWidth, subHeight)); //bottom-left
        Nodes[3] = new QuadTree(Depth + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight)); //bottom-right
        foreach (QuadTree Q in Nodes) { Q.MaxComponents = MaxComponents; Q.MaxTreeDepth = MaxTreeDepth; } 
    }

    public void Retrieve(List<PhysicsComponent> ReturnList, PhysicsComponent Obj) // Get objects that could collide with an object.
    {   
        foreach (PhysicsComponent quadtreeObj in Objects) if (quadtreeObj != Obj) ReturnList.Add(quadtreeObj); // Get objects in this quadtree except Obj

        // Go through other nodes if present.
        if (Nodes[0] != null)
        {
            var index = GetIndex(Obj);
            if (index != -1) Nodes[index].Retrieve(ReturnList, Obj); // Object is only in 1 split of the quadtree
            else // Object belongs to multiple splits
            {
                for (int i = 0; i < Nodes.Length; i++)
                {
                    Nodes[i].Retrieve(ReturnList, Obj);
                }
            }
        }
    }

    private int GetIndex(PhysicsComponent Object) // Get the index of the node in which the bound is located.
    {
        int index = -1; // If the index is -1, it means the object is located in multiple nodes.
        double verticalMidpoint = Bounds.X + Bounds.Width / 2;
        double horizontalMidpoint = Bounds.Y + Bounds.Height / 2;

        bool topQuadrant = Object.ActualPosition.Y < horizontalMidpoint && Bounds.Y + Object.Size.Y < horizontalMidpoint; // top
        bool bottomQuadrant = Object.ActualPosition.Y > horizontalMidpoint; // bottom

        if (Object.ActualPosition.X < verticalMidpoint && Bounds.X + Object.Size.X < verticalMidpoint) // if fully left
        {
            if (topQuadrant) // top-left
            {
                index = 0;
            }
            else if (bottomQuadrant) // bottom-left
            {
                index = 2;
            }
        }
        else if (Object.ActualPosition.X > verticalMidpoint) // if fully right
        {
            if (topQuadrant) // top-right
            {
                index = 1;
            }
            else if (bottomQuadrant) // bottom-right
            {
                index = 3;
            }
        }

        return index;
    }

    public void Clear() // Clear all components in the quadtree.
    {
        Objects.Clear();

        for (int i = 0; i < Nodes.Length; i++)
        {
            if (Nodes[i] != null)
            {
                Nodes[i].Clear();
                Nodes[i] = null;
            }
        }
    }
    public void Draw(SpriteBatch spriteBatch, SpriteFont font) // Method used for Debugging
    {
        // Draw boundaries of this node.
        spriteBatch.Draw(GameInstance.PlainTexture, Bounds, new(0, 63, 0, 32));

        if (Nodes[0] == null)
        {
            // Display properties of this node.
            string properties = $"Depth: {Depth}{System.Environment.NewLine}Obj-Count: {Objects.Count}{System.Environment.NewLine}";
            spriteBatch.DrawString(font, properties, new Vector2(Bounds.X + 5, Bounds.Y + 5), Color.White);
        }
        else
        {
            // Recursively draw and print children nodes.
            foreach (QuadTree node in Nodes)
            {
                node.Draw(spriteBatch, font);
            }
        }
    }
    #endregion
}