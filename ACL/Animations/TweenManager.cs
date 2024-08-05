namespace ACL.Animations;
public class TweenManager
{
    private List<Tween> _tweens = new();

    public void AddTween(Tween tween)
    {
        _tweens.Add(tween);
    }

    public void Update(float deltaTime)
    {
        foreach (Tween tween in _tweens)
        {
	        tween.Update(deltaTime);
	    }
    }

    public void RemoveTween(Tween tween)
    {
        _tweens.Remove(tween);
    }
}