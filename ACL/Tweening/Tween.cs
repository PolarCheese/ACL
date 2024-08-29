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
    // Time & Delay
    public float Duration;
    public float ElapsedTime { get; private set; } = 0;
    public float DelayTime = 0;

    // Status
    public bool IsActive { get; private set; }
    public bool IsPaused { get; set; }
    public bool IsComplete { get; private set; }

    // Repetition
    public bool Repeating => RepeatCycles > 0 || RepeatForever;
    public uint RepeatCycles = 0;
    public bool RepeatForever = false;
    public bool AutoSwap = false; // if false, when repeated it goes back to the initial start instead of swapping both ends.

    public Func<float, float> EasingFunction { get; set; } = EasingFunctions.Linear;

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
        if (IsPaused || !IsActive) return;

        if (IsComplete) 
        {
            if (Repeating)
            {
                // Repeat the tween
                IsComplete = false;
                ElapsedTime = 0;
                if (AutoSwap) Swap();
                RepeatCycles = RepeatCycles > 0 ? RepeatCycles - 1 : 0;
            }
            else IsActive = false; // finish off the tween
        }

        // Apply delay
        if (DelayTime > 0)
        {
            DelayTime -= deltaTime;
            return;
        }
        else DelayTime = 0;

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

    public void Delay(float Time) { DelayTime += Time; }
    public void Repeat(int Times) { RepeatCycles += (uint)Times; }
    public void Pause() { IsPaused = true; }
    public void Unpause() { IsPaused = false; } 
}