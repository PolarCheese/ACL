// This class exists for methods related to collisions between physics objects. All methods are static.

using ACL.UI;
using Microsoft.Xna.Framework;

namespace ACL.Physics;

public class Collisions
{
    public static Vector2[] GetRectangleVertices(Component Object)
    {
        // Get bound points (works if component properties have been correctly implemented)
        var Points = new Vector2[4]; Vector2 origin = Object.ActualPosition + Object.Size * Object.Origin;
        Points[0] = new(Object.ActualPosition.X, Object.ActualPosition.Y);  // top-left
        Points[1] = new(Object.ActualPosition.X + Object.Size.X, Object.ActualPosition.Y);  // top-right
        Points[2] = new(Object.ActualPosition.X, Object.ActualPosition.Y + Object.Size.Y);  // bottom-left
        Points[3] = new(Object.ActualPosition.X + Object.Size.X, Object.ActualPosition.Y + Object.Size.Y);  // bottom-right

        // Check if points are rotated by a number indivisible by 90 degrees
        if (Object.ActualRotation % 90 == 0)
        {
            // Rotate points round object origin (so it's actually OBB and not AABB)
            var RotatedPoints = new Vector2[4];
            RotatedPoints[0] = RotatePoint(Points[0], origin, Object.ActualRotation);
            RotatedPoints[1] = RotatePoint(Points[1], origin, Object.ActualRotation);
            RotatedPoints[2] = RotatePoint(Points[2], origin, Object.ActualRotation);
            RotatedPoints[3] = RotatePoint(Points[3], origin, Object.ActualRotation);
            return RotatedPoints;
        }

        return Points;
    }

    private static Vector2 RotatePoint(Vector2 point, Vector2 origin, float rotation)
    {
        Vector2 rotatedPoint = new(origin.X + (point.X - origin.X) * MathF.Cos(rotation) - (point.Y - origin.Y) * MathF.Sin(rotation), origin.Y + (point.X - origin.X) * MathF.Sin(rotation) + (point.Y - origin.Y) * MathF.Cos(rotation));
        return rotatedPoint;
    }
}