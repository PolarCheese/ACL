using ACL;
using Microsoft.Xna.Framework;

namespace ACL.Physics
{
    public class Environment 
    {
        public List<PhysicsComponent> Subcomponents {get; set;} = new();
        public List<Vector2> Forces {get; set;} = new();
    }
}