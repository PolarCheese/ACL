namespace ACL.Animations;

public abstract class Tween<T> : Tween where T : struct
{
    protected readonly TweenMember<T> Member;
    protected readonly LerpFunction<T> lerpFunction;
    protected T _endValue;
    protected T _startValue;
    
    public Tween(TweenMember<T> member, T endValue, LerpFunction<T> lerpFunction, float duration) : base(duration)
    {
        Member = member;
        this.lerpFunction = lerpFunction;
        _endValue = endValue;

        Initialize();
    }
    protected override void Initialize()
    {
        _startValue = Member.Value;
    }
    protected override void Swap()
    {
        _endValue = _startValue;
        Initialize();
    }
}
public abstract class Tween
{
    public float Duration;
    public float ElapsedTime { get; private set; } = 0;
    public bool IsActive { get; private set; }
    public bool IsPaused { get; set; }
    public bool IsComplete { get; private set; }
    public Func<float, float> EasingFunction { get; set; } = EasingFunctions.Linear;

    // Repetition (to be implemented soon)
    public bool Repeating => RepeatCycles > 0 || RepeatForever;
    public float RepeatCycles = 0;
    public bool RepeatForever = false;
    public bool AutoSwap = false; // if false, when repeated it goes back to the initial start instead of swapping both ends.

    internal Tween(float duration)
    {
        Duration = duration;

        IsActive = true;
    }

    protected abstract void Initialize();
    protected abstract void Interpolate(float n);
    protected abstract void Swap();

    public void Update(float deltaTime)
    {
        if (IsComplete && !Repeating) IsActive = false; // finish off the tween
        if (IsPaused || !IsActive) return;

        ElapsedTime += deltaTime;
        var n = ElapsedTime / Duration;
        if (EasingFunction != null) n = EasingFunction(n);

        if (ElapsedTime >= Duration)
        {
            IsComplete = true;
            n = 1;
        }

        Interpolate(n);
    }
}