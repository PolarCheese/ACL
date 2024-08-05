using ACL.Animations;

public class LinearTween : Tween
{
    public LinearTween(float StartValue, float EndValue, float Duration) : base(StartValue, EndValue, Duration) {}

    public override void Update(float deltaTime)
    {
        currentTime += deltaTime;
        float progress = currentTime / duration;
        currentValue = startValue + (progress * (endValue - startValue));
    }

    public override float GetValue()
    {
        if (currentTime >= duration) return endValue;
        return currentValue;
    }
}