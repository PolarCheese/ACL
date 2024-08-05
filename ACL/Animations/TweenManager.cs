namespace ACL.Animations;
public class TweenManager
{
    private List<Tween> _tweens = new();
    private List<Tween> _finishedTweens = new();

    public void AddTween(Tween tween)
    {
        _tweens.Add(tween);
    }

    public void Update(float deltaTime)
    {
        foreach (Tween finishedTween in _finishedTweens)
        {
            _tweens.Remove(finishedTween);
        }
        _finishedTweens.Clear();

        foreach (Tween tween in _tweens)
        {
	        tween.Update(deltaTime);
            if (tween.currentTime >= tween.duration) _finishedTweens.Add(tween);
	    }
        
    }

    public void RemoveTween(Tween tween)
    {
        _tweens.Remove(tween);
    }
}