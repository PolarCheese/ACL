namespace ACL.Animations;
public class TweenMember<T> where T : struct
{
    private readonly Func<T> _getter;
    private readonly Action<T> _setter;

    public TweenMember(Func<T> getter, Action<T> setter)
    {
        _getter = getter;
        _setter = setter;
    }

    public T Value
    {
        get => _getter();
        set => _setter(value);
    }
}