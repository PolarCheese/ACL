using ACL.Values;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public abstract class Component
    {
        public Component(){}

        #region properties
        public Component? Parent { get; private set;}
        public List<Component> Children { get;} = new List<Component>();
        public bool PositionChildrenToParent {get; set;} = true;
        public bool SizeChildrenToParent {get; set;} = true;
        public bool RotateChildrenToParent {get; set;} = true;

        public bool ToUpdate {get; set;} = true;
        public bool ToDraw {get; set;} = true;

        public PositionVector? Position {get; set;}
        public PositionVector? Size {get; set;}
        public float Rotation {get; set;} = 0f;
        #endregion

        #region nodes
        public void AddChildren(params Component[] Components)
        {
            foreach (var Child in Components)
            {
                Children.Add(Child);
            }
        }

        public void RemoveChildren(params Component[] Components)
        {
            foreach (var Child in Components)
            {
                Children.Add(Child);
            }
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