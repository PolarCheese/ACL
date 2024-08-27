using System;
using Microsoft.Xna.Framework;

namespace ACL.Animations;

public static class EasingFunctions
{
    public static float Linear(float value) => value;
    public static float QuadIn(float value) => EasingFormulas.In(value, 2);
    public static float QuadOut(float value) => EasingFormulas.Out(value, 2);
    public static float QuadInOut(float value) => EasingFormulas.InOut(value, 2);
    public static float CubicIn(float value) => EasingFormulas.In(value, 3);
    public static float CubicOut(float value) => EasingFormulas.Out(value, 3);
    public static float CubicInOut(float value) => EasingFormulas.InOut(value, 3);
    public static float QuartIn(float value) => EasingFormulas.In(value, 4);
    public static float QuartOut(float value) => EasingFormulas.Out(value, 4);
    public static float QuartInOut(float value) => EasingFormulas.InOut(value, 4);
    public static float QuintIn(float value) => EasingFormulas.In(value, 5);
    public static float QuintOut(float value) => EasingFormulas.Out(value, 5);
    public static float QuintInOut(float value) => EasingFormulas.InOut(value, 5);

    public static float SineIn(float value) => (float)Math.Sin(value*MathHelper.PiOver2 - MathHelper.PiOver2) + 1;
    public static float SineOut(float value) => (float)Math.Sin(value*MathHelper.PiOver2);
    public static float SineInOut(float value) => (float)(Math.Sin(value*MathHelper.Pi - MathHelper.PiOver2) + 1) / 2;

    public static float ExpoIn(float value) => (float)Math.Pow(2, 10*(value-1));
}
public static class EasingFormulas
{
    public static float In(double value, float power)
    {
        return (float)Math.Pow(value, power);
    }
    public static float Out(double value, float power)
    {
        var sign = (power % 2 == 0) ? -1 : 1;
        return (float)(sign * (Math.Pow(value - 1, power) + sign));
    }
    public static float InOut(double value, float power)
    {
        value *= 2;
        if (value < 1) return In(value, power)/2;

        var sign = (power % 2 == 0) ? -1 : 1;
        return (float)(sign/2f * (Math.Pow(value - 2, power) + sign * 2));
    }
}