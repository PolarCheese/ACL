namespace ACL.Animations;

public abstract class Tween
{
    protected float startValue;
    protected float endValue;
    public float duration { get; protected set; }
    public float currentTime { get; protected set; }
    public float currentValue { get; protected set; }

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