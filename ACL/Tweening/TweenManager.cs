namespace ACL.Animations;
public class TweenManager
{
    private List<Tween> _activeTweens = new();
    public int ActiveTweensCount => _activeTweens.Count;

    public void AddTween(Tween tween)
    {
        _activeTweens.Add(tween);
    }

    public void Update(float deltaTime)
    {        
        for (var i = _activeTweens.Count - 1; i >= 0; i--)
        {
            var tween = _activeTweens[i];
            tween.Update(deltaTime);
            if (!tween.IsActive) _activeTweens.RemoveAt(i);
        }
    }
}