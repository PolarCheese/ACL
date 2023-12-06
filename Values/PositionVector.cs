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
    }
}