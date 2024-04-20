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
        
        // If any of the bools below are true, the subcomponents of this component will change their properties relative to their parent component.
        public bool PositionSubcomponents {get; set;} = true;
        public bool RotateSubcomponents {get; set;} = true;
        
        public Camera? Bound {get; set;} = null; // If not null, component will take the cursor's position from the camera it is bound to.

        // Logic Toggles
        public bool ToUpdate {get; set;} = true;
        public bool ToDraw {get; set;} = true;

        // Positioning, Size, Rotation and Depth
        public Vector2 Origin {get; protected set;} = Vector2.Zero;
        public Vector2 Position {get; protected set;} = Vector2.Zero;
        public Vector2 Size {get; protected set;} = Vector2.Zero;
        public float Rotation {get; protected set;} = 0f;
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

            // Update Position/Rotation
            if (PositionSubcomponents) { Subcomponent.Position += Position - Size*Origin; }
            if (RotateSubcomponents) { Subcomponent.Rotation += Rotation; }
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

        #region Set methods

        public void SetPosition(Vector2 NewPosition)
        {
            UpdateSubcomponentsPosition(NewPosition - Position); Position = NewPosition;
        }
        public void SetRotation(float NewRotation)
        {
            UpdateSubcomponentsRotation(NewRotation - Rotation); Rotation = NewRotation;
        }
        // Updating subcomponents after setting size and origin is not implemented yet.
        public void SetSize(Vector2 NewSize) 
        {
            Size = NewSize;
        }
        public void SetOrigin(Vector2 NewOrigin)
        {
            Origin = NewOrigin; 
        }
        
        protected void UpdateSubcomponentsPosition(Vector2 RelativePositionChange)
        {
            if (RotateSubcomponents)
            {
                foreach (Component Subcomponent in Subcomponents) { Subcomponent.Position += RelativePositionChange; }
            }
        }

        protected void UpdateSubcomponentsRotation(float RelativeRotationChange)
        {
            if (RotateSubcomponents) 
            {
                foreach (Component Subcomponent in Subcomponents) { Subcomponent.Rotation += RelativeRotationChange; }
            }
        }

        public void SetSubcomponentPositioning(bool Value = true)
        {
            if (PositionSubcomponents != Value)
            {
                if (Value)
                {
                    // Update subcomponents
                }
                else
                {
                    // Set back to initial state
                }
            }
            PositionSubcomponents = Value;
        }
        public void SetSubcomponentRotating(bool Value = true)
        {
            if (RotateSubcomponents != Value)
            {
                if (Value)
                {
                    // Update subcomponents
                }
                else
                {
                    // Set back to initial state
                }
            }
            RotateSubcomponents = Value;
        }
        #endregion
    }
}