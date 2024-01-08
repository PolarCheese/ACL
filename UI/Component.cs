using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public abstract class Component
    {
        public GameInstance Game;

        public Component(GameInstance game)
        {
            Game = game;
        }

        #region properties
        public Component? Parent {get; set;}
        public List<Component> Children {get;} = new List<Component>();
        public bool PositionChildrenToParent {get; set;} = true;
        public bool SizeChildrenToParent {get; set;} = true;
        public bool RotateChildrenToParent {get; set;} = true;

        public bool ToUpdate {get; set;} = true;
        public bool ToDraw {get; set;} = true;

        public Vector2 Origin {get; set;} = Vector2.Zero;
        public PositionVector Position {get; set;} = PositionVector.Zero;
        public PositionVector Size {get; set;} = PositionVector.Zero;
        public float Rotation {get; set;} = 0f;
        
        #endregion

        #region nodes
        public void AddChildren(params Component[] Components)
        {
            foreach (var Child in Components)
            {
                Children.Add(Child);
                Child.Parent = this;
            }
        }

        public void RemoveChildren(params Component[] Components)
        {
            foreach (var Child in Components)
            {
                Children.Add(Child);
                Child.Parent = null;
            }
        }

        public Rectangle GetBounds()
        {
            Rectangle Bounds; Vector2 ConvertedPosition; Vector2 ConvertedSize;
            if (Parent != null) {ConvertedPosition = Position.ConvertToBound(Parent.GetBounds()); ConvertedSize = Size.ConvertToBound(Parent.GetBounds());}
            else {ConvertedPosition = Position.ConvertToScreenPosition(Game); ConvertedSize = Size.ConvertToScreenPosition(Game);}
            Bounds = new Rectangle((int)(ConvertedPosition.X - ConvertedPosition.X * Origin.X), (int)(ConvertedPosition.Y - ConvertedPosition.Y * Origin.Y), (int)ConvertedSize.X, (int)ConvertedSize.Y);
            return Bounds;
        }
        #endregion

        #region Update/Draw
        public virtual void Update(GameTime gameTime)
        {
            foreach (Component Child in Children)
            {
                if (Child.ToUpdate)
                {
                    Child.Update(gameTime);
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component Child in Children)
            {
                if (Child.ToDraw)
                {
                    Child.Draw(gameTime, spriteBatch);
                }
            }

        }
        #endregion
    }
}