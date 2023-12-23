using Microsoft.Xna.Framework;

namespace ACL.Values
{
    public class Coordinates
    {
        public PositionVector StartPosition {get; set;} = new PositionVector(0,0,0,0);
        public PositionVector CurrentPosition {get; set;} = new PositionVector(0,0,0,0);
        public PositionVector EndPosition {get; set;} = new PositionVector(0,0,0,0);
        /*
        public float GetLength(int BoundsWidth, int BoundsHeight)
        {
            Vector2 StartVector = new Vector2(StartPosition.ConvertToScreenPosition()[0], StartPosition.ConvertToScreenPosition()[1]);
            Vector2 EndVector = new Vector2(EndPosition.ConvertToScreenPosition()[0], StartPosition.ConvertToScreenPosition()[1]);

            return (EndVector - StartVector).Length();
        }
        */
    }
}