using ACL.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.Physics
{
    public class QuadTree
    {
        public PhysicsEngine PhysicsEngine {get; set;}
        #region Properties and Fields
        public int Depth;
        public Rectangle Bounds;
        public List<PhysicsComponent> Objects;
        public QuadTree[] Nodes;

        public QuadTree(PhysicsEngine physicsEngine, int depth, Rectangle bounds)
        {
            PhysicsEngine = physicsEngine;
            Depth = depth;
            Bounds = bounds;
            Objects = new();
            Nodes = new QuadTree[4]; // Parent quadtree nodes.
        }
        #endregion

        #region Methods
        public void Insert(PhysicsComponent physicsComponent) // Adds a physics component to the quadtree.
        {
            if (Nodes[0] != null) // Check for nodes.
            {
                var hitbox = new Rectangle(0, 0, 1, 1); // requires replacement
                int index = GetIndex(hitbox); // Find where the hitbox fits.
                if (index != -1)
                {
                    Nodes[index].Insert(physicsComponent);
                    Objects.Remove(physicsComponent);
                }
            }

            Objects.Add(physicsComponent);

            if (Objects.Count > PhysicsEngine.MaxComponentsPerNode && Depth < PhysicsEngine.MaxTreeDepth) 
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

            Nodes[0] = new QuadTree(PhysicsEngine, Depth + 1, new Rectangle(x, y, subWidth, subHeight)); //top-left
            Nodes[1] = new QuadTree(PhysicsEngine, Depth + 1, new Rectangle(x + subWidth, y, subWidth, subHeight)); //top-right
            Nodes[2] = new QuadTree(PhysicsEngine, Depth + 1, new Rectangle(x, y + subHeight, subWidth, subHeight)); //bottom-left
            Nodes[3] = new QuadTree(PhysicsEngine, Depth + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight)); //bottom-right

            foreach (PhysicsComponent physicsComponent in Objects.ToList()) // Send the parent quadtree components to the nodes.
            {
                var hitbox = new Rectangle((int)physicsComponent.ActualPosition.X, (int)physicsComponent.ActualPosition.Y, (int)physicsComponent.Size.X, (int)physicsComponent.Size.Y); // requires replacement
                int index = GetIndex(hitbox);
                if (index != -1)
                {
                    Nodes[index].Insert(physicsComponent);
                    Objects.Remove(physicsComponent);
                }
            }
        }

        public List<PhysicsComponent> Retrieve(Rectangle bounds) // Get all the objects inside of a quadtree space, even if its part of a smaller quadtree.
        {
            List<PhysicsComponent> result = new();
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

            if (bounds.X < verticalMidpoint && bounds.X + bounds.Width < verticalMidpoint) // if contained in the left.
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
        public void Draw(SpriteBatch spriteBatch, SpriteFont font) // Method used for Debugging
        {
            // Draw boundaries of this node.
            spriteBatch.Draw(GameInstance.PlainTexture, Bounds, new(0, 63, 0, 32));

            if (Nodes[0] == null)
            {
                // Print properties of this node.
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
}