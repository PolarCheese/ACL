using Microsoft.Xna.Framework;

namespace ACL.Values
{
    public class PositionVector
    {   
        // Scale
        public float RelativeX {get; set;} = 0;
        public float RelativeY {get; set;} = 0;
        public float AbsoluteX {get; set;} = 0;
        public float AbsoluteY {get; set;} = 0;
        public static PositionVector Zero { get; } = new PositionVector(0, 0, 0, 0);

        public PositionVector(float relativeX, float relativeY, float absoluteX, float absoluteY)
        {
            RelativeX = relativeX; RelativeY = relativeY;
            AbsoluteX = absoluteX; AbsoluteY = absoluteY;
        }

        public Vector2 ConvertToBound(Rectangle Bounds)
        {
            float x = Bounds.X + (float)(Bounds.Width * RelativeX) + AbsoluteX;
            float y = Bounds.Y + (float)(Bounds.Height * RelativeY) + AbsoluteY;

            return new Vector2(x, y);
        }
        public Vector2 ConvertToScreenPosition(GameInstance Game)
        {
            float x = (float)(Game.GetWindowResolution()[0] * RelativeX) + AbsoluteX;
            float y = (float)(Game.GetWindowResolution()[1] * RelativeY) + AbsoluteY;

            return new Vector2(x, y);
        }
        public Vector2 ConvertToBound(int BoundsWidth, int BoundsHeight)
        {
            float x = (float)(BoundsWidth * RelativeX) + AbsoluteX;
            float y = (float)(BoundsHeight * RelativeY) + AbsoluteY;

            return new Vector2(x, y);
        }

        public Vector2 ConvertToBound(int OffsetX, int OffsetY, int BoundsWidth, int BoundsHeight)
        {
            float x = OffsetX + (float)(BoundsWidth * RelativeX) + AbsoluteX;
            float y = OffsetY + (float)(BoundsHeight * RelativeY) + AbsoluteY;

            return new Vector2(x, y);
        }
    }
}