namespace ACL.Animations;

public abstract class Tween
{
    protected float startValue;
    protected float endValue;
    protected float duration;
    protected float currentTime;
    protected float currentValue;

    public Tween(float StartValue, float EndValue, float Duration)
    {
        startValue = StartValue;
        endValue = EndValue;
        duration = Duration;
        currentTime = 0;
        currentValue = startValue;
    }

    public abstract void Update(float deltaTime);

    public abstract float GetValue();
}