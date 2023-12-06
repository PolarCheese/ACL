using Microsoft.Xna.Framework;
using ACL.Values;

namespace ACL.Values
{
    public class Coordinates
    {
        public PositionVector StartPosition {get; set;} = new PositionVector(0,0,0,0);
        public PositionVector CurrentPosition {get; set;} = new PositionVector(0,0,0,0);
        public PositionVector EndPosition {get; set;} = new PositionVector(0,0,0,0);
        /*
        public float GetLength()
        {
            float length;
            length = ().length;
            return length;
        } */
    }
}