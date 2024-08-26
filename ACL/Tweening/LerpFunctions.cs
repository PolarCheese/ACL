using Microsoft.Xna.Framework;

namespace ACL.Animations;

public delegate T LerpFunction<T>(T start, T end, float progress);
public static class LerpFunctions
{
    public static LerpFunction<float> Float = (s, e, p) => s + (e - s) * p;
    public static LerpFunction<Vector2> Vector2 = (s, e, p) => { return Microsoft.Xna.Framework.Vector2.Lerp(s, e, p); };
    public static LerpFunction<Vector3> Vector3 = (s, e, p) => { return Microsoft.Xna.Framework.Vector3.Lerp(s, e, p); };
    public static LerpFunction<Vector4> Vector4 = (s, e, p) => { return Microsoft.Xna.Framework.Vector4.Lerp(s, e, p); };
    public static LerpFunction<Color> Color = (s, e, p) => { return Microsoft.Xna.Framework.Color.Lerp(s, e, p); };
}