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

    public static float ExpoIn(float value) => (float)Math.Pow(2, 10*(value-1));
    public static float ExpoOut(float value) => EasingFormulas.FuncOut(value, ExpoIn);
    public static float ExpoInOut(float value) => EasingFormulas.FuncInOut(value, ExpoIn);

    public static float SineIn(float value) => (float)Math.Sin(value*MathHelper.PiOver2 - MathHelper.PiOver2) + 1;
    public static float SineOut(float value) => (float)Math.Sin(value*MathHelper.PiOver2);
    public static float SineInOut(float value) => (float)(Math.Sin(value*MathHelper.Pi - MathHelper.PiOver2) + 1) / 2;

    public static float CircleIn(float value) => (float)-(Math.Sqrt(1 - value * value) - 1);
    public static float CircleOut(float value) => (float)Math.Sqrt(1 - (value - 1) * (value - 1));
    public static float CircleInOut(float value) => (float)(value <= .5f ? (Math.Sqrt(1 - value * value * 4) - 1) / 2 : (Math.Sqrt(1 - (value * 2 - 2) * (value * 2 - 2)) + 1) / 2);

    public static float BackIn(float value) => (float)(Math.Pow(value, 3) - value * Math.Sin(value * MathHelper.Pi));
    public static float BackIn(float value, float amplitude = 1f) => (float)(Math.Pow(value, 3) - value * amplitude * Math.Sin(value * MathHelper.Pi));
    public static float BackOut(float value) => EasingFormulas.FuncOut(value, BackIn);
    public static float BackOut(float value, float amplitude = 1f) => EasingFormulas.FuncOut(value, v => BackIn(v, amplitude));
    public static float BackInOut(float value) => EasingFormulas.FuncInOut(value, BackIn);
    public static float BackInOut(float value, float amplitude = 1f) => EasingFormulas.FuncInOut(value, v => BackIn(v, amplitude));

    public static float ElasticIn(float value) => (float)((Math.Exp(3f * value) - 1) / (Math.Exp(3f) - 1) * Math.Sin((MathHelper.PiOver2 + MathHelper.TwoPi) * value));
    public static float ElasticIn(float value, float springiness = 3f, float oscillations = 1f) => (float)((Math.Exp(springiness * value) - 1) / (Math.Exp(springiness) - 1) * Math.Sin((MathHelper.PiOver2 + MathHelper.TwoPi * oscillations) * value));
    public static float ElasticOut(float value) => EasingFormulas.FuncOut(value, ElasticIn);
    public static float ElasticOut(float value, float springiness = 3f, float oscillations = 1f) => EasingFormulas.FuncOut(value, v => ElasticIn(v, springiness, oscillations));
    public static float ElasticInOut(float value) => EasingFormulas.FuncInOut(value, ElasticIn);
    public static float ElasticInOut(float value, float springiness = 3f, float oscillations = 1f) => EasingFormulas.FuncInOut(value, v => ElasticIn(v, springiness, oscillations));
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

    public static float FuncOut(float value, Func<float, float> function)
    {
        return 1 - function(1 - value);
    }

    public static float FuncInOut(float value, Func<float, float> function)
    {
        if (value < .5f) return .5f * function(value * 2);

        return 1f - .5f * function(2 - value * 2);
    }
}