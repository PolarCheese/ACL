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

        // Logic Toggles
        public bool ToUpdate {get; set;} = true;
        public bool ToDraw {get; set;} = true;

        // Positioning, Size and Rotation
        public Vector2 Origin {get; set;} = Vector2.Zero;
        public PositionVector Position {get; set;} = PositionVector.Zero;
        public PositionVector Size {get; set;} = PositionVector.Zero;
        protected PositionVector _previousPosition = PositionVector.Zero;
        protected PositionVector _previousSize = PositionVector.Zero;
        public Vector2 ActualPosition {get; protected set;} = Vector2.Zero;
        public Vector2 ActualSize {get; protected set;} = Vector2.Zero;
        
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
        #endregion

        #region Update/Draw
        public virtual void Update(GameTime gameTime)
        {
            UpdateProperties();
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

        public virtual void UpdateProperties()
        {
            // Update actual size/position etc.
            bool HasParent = Parent != null;
            if (_previousSize != Size) {
                // Size has changed. Recalculate actual size.
                if (HasParent && Parent.SizeChildrenToParent) {ActualSize = Size.ToVector2(Parent.ActualSize);}
                else {ActualSize = Size.ToVector2(Game);}
            }
            if (_previousPosition != Position) {
                // Position has changed. Recalculate actual position.
                if (HasParent && Parent.PositionChildrenToParent) {ActualPosition = Parent.ActualPosition + (Position.ToVector2(Parent.ActualPosition) - ActualSize * Origin);}
                else {ActualPosition = Position.ToVector2(Game) - Size.ToVector2(Game) * Origin;}
            }

            _previousSize = Size; _previousPosition = Position;
        }
        #endregion
    }
}