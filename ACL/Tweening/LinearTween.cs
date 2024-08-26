namespace ACL.Animations;

public class LinearTween<T> : Tween<T> where T : struct
{
    public LinearTween(TweenMember<T> member, T endValue, LerpFunction<T> lerpFunction, float duration) : base(member, endValue, lerpFunction, duration) {}

    protected override void Interpolate(float n)
    {
        var interpolatedValue = lerpFunction(_startValue, _endValue, n);
        Member.Value = interpolatedValue;
    }
}