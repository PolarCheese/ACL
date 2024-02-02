using Microsoft.Xna.Framework;

namespace ACL.Values
{
    public class QuadVector
    {   
        // Scale
        public float RelativeX {get; set;} = 0;
        public float RelativeY {get; set;} = 0;
        public float AbsoluteX {get; set;} = 0;
        public float AbsoluteY {get; set;} = 0;
        public static QuadVector Zero { get; } = new QuadVector(0, 0, 0, 0);

        public QuadVector(float relativeX, float relativeY, float absoluteX, float absoluteY)
        {
            RelativeX = relativeX; RelativeY = relativeY;
            AbsoluteX = absoluteX; AbsoluteY = absoluteY;
        }
        public QuadVector(QuadVector quadVector)
        {
            RelativeX = quadVector.RelativeX; RelativeY = quadVector.RelativeY;
            AbsoluteX = quadVector.AbsoluteX; AbsoluteY = quadVector.AbsoluteY;
        }

        public Vector2 ToVector2(int BoundsWidth, int BoundsHeight)
        {
            float x = (float)(BoundsWidth * RelativeX) + AbsoluteX;
            float y = (float)(BoundsHeight * RelativeY) + AbsoluteY;

            return new Vector2(x, y);
        }

        public Vector2 ToVector2(int BoundsWidth, int BoundsHeight, int OffsetX, int OffsetY)
        {
            float x = OffsetX + (float)(BoundsWidth * RelativeX) + AbsoluteX;
            float y = OffsetY + (float)(BoundsHeight * RelativeY) + AbsoluteY;

            return new Vector2(x, y);
        }

        public Vector2 ToVector2(GameInstance Game)
        {
            int[] GameBounds = Game.GetWindowResolution();
            float x = (float)(GameBounds[0] * RelativeX) + AbsoluteX;
            float y = (float)(GameBounds[1] * RelativeY) + AbsoluteY;

            return new Vector2(x, y);
        }

        public Vector2 ToVector2(Rectangle Bounds)
        {
            float x = (float)(Bounds.Width * RelativeX) + AbsoluteX;
            float y = (float)(Bounds.Height * RelativeY) + AbsoluteY;

            return new Vector2(x, y);
        }
        
        public Vector2 ToVector2(Vector2 Vector2)
        {
            float x = (float)(Vector2.X * RelativeX) + AbsoluteX;
            float y = (float)(Vector2.Y * RelativeY) + AbsoluteY;

            return new Vector2(x, y);
        }

        public bool IsEqual(QuadVector quadvector)
        {
            if ((RelativeX == quadvector.RelativeX) && (RelativeY == quadvector.RelativeY) && (AbsoluteX == quadvector.AbsoluteX) && (AbsoluteY == quadvector.AbsoluteY)) 
            {return true;} return false;
        }
    }
}