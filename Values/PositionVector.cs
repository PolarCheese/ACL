using System.ComponentModel;

namespace ACL.Values
{
    public class PositionVector
    {
        public float RelativeX {get; set;} = 0;
        public float RelativeY {get; set;} = 0;
        public float AbsoluteX {get; set;} = 0;
        public float AbsoluteY {get; set;} = 0;

        public PositionVector(float relativeX, float relativeY, float absoluteX, float absoluteY)
        {
            RelativeX = relativeX; RelativeY = relativeY;
            AbsoluteX = absoluteX; AbsoluteY = absoluteY;
        }

        public List<float> ConvertToScreenPosition()
        {
            List<float> ConvertedFloats = new List<float>();

            float x; float y;
            x = (float)(GameInstance.WindowWidth * RelativeX) + AbsoluteX;
            y = (float)(GameInstance.WindowWidth * RelativeY) + AbsoluteY;

            ConvertedFloats[0] = x; ConvertedFloats[1] = y;
            return ConvertedFloats;
        }
    }
}