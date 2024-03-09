using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.Physics
{
    public class QuadTree
    {
        #region Properties and Fields
        private const int MaxComponentsPerNode = 4;
        private const int MaxTreeDepth = 4;

        public int Depth;
        public Rectangle Bounds;
        public List<PhysicsComponent> Objects;
        public QuadTree[] Nodes;

        public QuadTree(int depth, Rectangle bounds)
        {
            Depth = depth;
            Bounds = bounds;
            Objects = new List<PhysicsComponent>();
            Nodes = new QuadTree[4]; // Parent quadtree nodes.
        }
        #endregion

        #region Methods

        public void Insert(PhysicsComponent dynamicComponent) // Adds a dynamic component to the quadtree.
        {
            if (Nodes[0] != null) // Check for nodes.
            {
                var hitbox = new Rectangle(0, 0, 1, 1); // requires replacement
                int index = GetIndex(hitbox); // Find where the hitbox fits.
                if (index != -1)
                {
                    Nodes[index].Insert(dynamicComponent);
                    Objects.Remove(dynamicComponent);
                }
            }

            Objects.Add(dynamicComponent);

            if (Objects.Count > MaxComponentsPerNode && Depth < MaxTreeDepth) 
            {
                Split(); // Limits reached, split this quadtree.
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

            foreach (PhysicsComponent dynamicComponent in Objects.ToList()) // Send the parent quadtree components to the nodes.
            {
                var hitbox = new Rectangle(0, 0, 1, 1); // requires replacement
                int index = GetIndex(hitbox);
                if (index != -1)
                {
                    Nodes[index].Insert(dynamicComponent);
                    Objects.Remove(dynamicComponent);
                }
            }
        }

        public List<PhysicsComponent> Retrieve(Rectangle bounds) // Get all the objects inside of a quadtree space, even if its part of a smaller quadtree.
        {
            List<PhysicsComponent> result = new List<PhysicsComponent>();
            int index = GetIndex(bounds);

            if (index != -1 && Nodes[0] != null)
            {
                result.AddRange(Nodes[index].Retrieve(bounds));
            }

            result.AddRange(Objects);
            return result;
        }

        private int GetIndex(Rectangle bounds) // Get the index of the node in which the bound is located.
        {
            int index = -1; // If the object is in multiple nodes, set the index is -1 as its part of the parent quadrant.
            //double MaxWidth = _bounds.X + _bounds.Width;
            double MaxHeight = Bounds.Y + Bounds.Height;
            double verticalMidpoint = Bounds.X + Bounds.Width / 2;
            double horizontalMidpoint = Bounds.Y + Bounds.Height / 2;

            bool topQuadrant = bounds.Y < horizontalMidpoint && bounds.Y + bounds.Height < horizontalMidpoint;
            bool bottomQuadrant = bounds.Y > horizontalMidpoint && bounds.Y + bounds.Height < MaxHeight;

            if (bounds.X < verticalMidpoint && bounds.X + bounds.Width < verticalMidpoint) // Check if bounds are contained in the left.
            {
                if (topQuadrant) // If top
                {
                    index = 0;
                }
                else if (bottomQuadrant) // If bottom
                {
                    index = 2;
                }
            }
            else if (bounds.X > verticalMidpoint) // If contained in the right
            {
                if (topQuadrant) // If top
                {
                    index = 1;
                }
                else if (bottomQuadrant) // If bottom
                {
                    index = 3;
                }
            }

            return index;
        }

        public void Clear() // Clear all components in the quadtree.
        {
            Objects.Clear();
        }

        public void DrawDebug(SpriteBatch spriteBatch, SpriteFont font) // Method used for Debugging
        {
            // Draw boundaries of this node.
            spriteBatch.Draw(GameInstance.PlainTexture, Bounds, Color.White);
            spriteBatch.Draw(GameInstance.PlainTexture, new Rectangle(Bounds.X + 1, Bounds.Y + 1, Bounds.Width - 2, Bounds.Height - 2), Color.Black);

            // Print properties of this node.
            if (Nodes[0] == null)
            {
                string properties = $"Depth: {Depth}{System.Environment.NewLine}Obj-Count: {Objects.Count}{System.Environment.NewLine}";
                spriteBatch.DrawString(font, properties, new Vector2(Bounds.X + 5, Bounds.Y + 5), Color.White);
            }

            // Recursively draw and print children nodes.
            if (Nodes[0] != null)
            {
                foreach (QuadTree node in Nodes)
                {
                    node.DrawDebug(spriteBatch, font);
                }
            }
        }
        #endregion
    }
}