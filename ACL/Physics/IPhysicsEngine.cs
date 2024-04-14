using Microsoft.Xna.Framework;

namespace ACL.Physics;

public interface IPhysicsEngine
{
    public void Clear();
    public void AddComponent(params PhysicsComponent[] Objects);
    public void RemoveComponent(params PhysicsComponent[] Objects);
    public void FixedUpdate(GameTime gameTime);

}