// This class exists for methods related to collisions between physics objects. All methods are static.

using ACL.UI;
using Microsoft.Xna.Framework;

namespace ACL.Physics;

public class Collisions
{
    public static Vector2[] GetRectangleVertices(Component Object)
    {
        // Get rectangle vertices (works if component properties have been correctly implemented)
        var Points = new Vector2[4]; Vector2 origin = Object.ActualPosition + Object.Size * Object.Origin;
        Points[0] = new(Object.ActualPosition.X, Object.ActualPosition.Y);  // top-left
        Points[1] = new(Object.ActualPosition.X + Object.Size.X, Object.ActualPosition.Y);  // top-right
        Points[2] = new(Object.ActualPosition.X + Object.Size.X, Object.ActualPosition.Y + Object.Size.Y);  // bottom-right
        Points[3] = new(Object.ActualPosition.X, Object.ActualPosition.Y + Object.Size.Y);  // bottom-left

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

    public static Vector2 RotatePoint(Vector2 point, Vector2 origin, float rotation)
    {
        Vector2 rotatedPoint = new(origin.X + (point.X - origin.X) * MathF.Cos(rotation) - (point.Y - origin.Y) * MathF.Sin(rotation), origin.Y + (point.X - origin.X) * MathF.Sin(rotation) + (point.Y - origin.Y) * MathF.Cos(rotation));
        return rotatedPoint;
    }

    public static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
    {
        min = float.MaxValue;
        max = float.MinValue;

        for(int i = 0; i < vertices.Length; i++)
        {
            Vector2 vertex = vertices[i];
            float projection = Vector2.Dot(vertex, axis);

            if (projection < min) min = projection;
            if (projection > max) max = projection;
        }
    }

    public static bool IntersectPolygons(Vector2[] verticesA, Vector2[] verticesB)
    {  
        // This is a SAT test for polygons. (p is for the polygons in the vertices array, i for the vertex and j is i++)
        Vector2[][] vertices = new Vector2[][] { verticesA, verticesB };

        // Iterate through both polygons
        for (int p = 0; p < 2; p++)
        {
            for (int i = 0; i < vertices[p].Length; i++)
            {
                // Get axis
                int j = (i + 1) % vertices[p].Length;
                Vector2 edge = vertices[p][j] - vertices[p][i];
                Vector2 axis = new(-edge.Y, edge.X);

                // Project vertices
                ProjectVertices(vertices[0], axis, out float minA, out float maxA);
                ProjectVertices(vertices[1], axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA) return false;
            }
        }

        return true;
    }

    // to be implemented
    //public bool IntersectPolygons()
    //public bool IntersectCircles()
}