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

        public List<float> ConvertToScreenPosition(GameInstance Game)
        {
            List<float> ConvertedFloats = new List<float>();

            float x; float y;
            x = (float)(Game.GetWindowResolution()[0] * RelativeX) + AbsoluteX;
            y = (float)(Game.GetWindowResolution()[1] * RelativeY) + AbsoluteY;

            ConvertedFloats.Add(x); ConvertedFloats.Add(y); return ConvertedFloats;
        }
        public List<float> ConvertToScreenPosition(int BoundsWidth, int BoundsHeight)
        {
            List<float> ConvertedFloats = new List<float>();

            float x; float y;
            x = (float)(BoundsWidth * RelativeX) + AbsoluteX;
            y = (float)(BoundsHeight * RelativeY) + AbsoluteY;

            ConvertedFloats.Add(x); ConvertedFloats.Add(y); return ConvertedFloats;
        }

        public List<float> ConvertToScreenPosition(int OffsetX, int OffsetY, int BoundsWidth, int BoundsHeight)
        {
            List<float> ConvertedFloats = new List<float>();

            float x; float y;
            x = OffsetX + (float)(BoundsWidth * RelativeX) + AbsoluteX;
            y = OffsetY + (float)(BoundsHeight * RelativeY) + AbsoluteY;

            ConvertedFloats.Add(x); ConvertedFloats.Add(y); return ConvertedFloats;
        }
    }
}