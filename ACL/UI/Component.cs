using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public abstract class Component : INode
    {
        public GameInstance Game;

        public Component(GameInstance game)
        {
            Game = game;
        }

        #region Properties
        public Component? Parent {get; set;}
        public List<Component> Subcomponents {get;} = new();
        public bool AllowResizing {get; set;} = true; // If true, the component manager will resize the component when the game window is resized.
        public bool ApplyParentPosition {get; set;} = true; // If true, the component will position itself relative to the parent component.
        public bool ApplyParentRotation {get; set;} = true; // If true, the component will rotate itself relative to the parent component.
        
        public Camera? Bound {get; set;} = null; // If not null, component will take the cursor's position from the camera it is bound to.

        // Logic Toggles
        public bool ToUpdate {get; set;} = true;
        public bool ToDraw {get; set;} = true;

        // Positioning, Size, Rotation and Depth
        public Vector2 Origin {get; set;} = Vector2.Zero;
        public Vector2 Position {get; set;} = Vector2.Zero;
        public Vector2 ActualPosition 
        {
            get 
            {
                Vector2 ActualPosition;
                if (Parent != null && ApplyParentPosition)
                {
                    // Calculate absolute position using parent absolute position.
                    ActualPosition = Position + Parent.ActualPosition - Size * Origin;
                }
                else
                {
                    // No parent, calculate absolute position normally.
                    ActualPosition = Position - Size * Origin;
                }

                return ActualPosition;
            }
        }
        public Vector2 Size {get; set;} = Vector2.Zero;
        public float Rotation {get; set;} = 0f;
        public float ActualRotation 
        {
            get
            {
                float ActualRotation;
                if (Parent != null && ApplyParentRotation)
                {
                    // Add parent rotation.
                    ActualRotation = Rotation + Parent.ActualRotation;
                }
                else
                {
                    ActualRotation = Rotation;
                }

                return ActualRotation;
            }
        }
        public float Depth {get; set;} = 0f;

        // Cursor
        public Rectangle Cursor {get; set;}
        public MouseState MouseState {get; set;}
        protected Rectangle _previousCursor;
        protected MouseState _previousMouseState;
        #endregion

        #region Nodes
        public void AddSubcomponents(params Component[] Components)
        {
            foreach (var Child in Components)
            {
                SubcomponentAddition(Child);
            }
        }
        public void AddSubcomponents(IEnumerable<Component> Components)
        {
            foreach (var Child in Components)
            {
                SubcomponentAddition(Child);
            }
        }

        public void RemoveSubcomponents(params Component[] Components)
        {
            foreach (var Child in Components)
            {
                SubcomponentRemoval(Child);
            }
        }
        public void RemoveSubcomponents(IEnumerable<Component> Components)
        {
            foreach (var Child in Components)
            {
                SubcomponentRemoval(Child);
            }
        }

        protected void SubcomponentAddition(Component Subcomponent)
        {
            // Set node status
            Subcomponents.Add(Subcomponent);
            Subcomponent.Parent = this;
        }
        protected void SubcomponentRemoval(Component Subcomponent)
        {
            // Remove node status
            Subcomponents.Remove(Subcomponent);
            Subcomponent.Parent = null;
        } 
        #endregion

        #region Update/Draw
        public virtual void Update(GameTime gameTime)
        {
            // Update cursor properties
            _previousMouseState = MouseState;
            MouseState = Mouse.GetState();

            _previousCursor = Cursor;
            Cursor = Bound == null ? Game.Cursor : Bound.Cursor;
            foreach (Component Child in Subcomponents)
            {
                if (Child.ToUpdate)
                {
                    Child.Update(gameTime);
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component Child in Subcomponents)
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