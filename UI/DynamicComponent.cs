using ACL.Values;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI
{
    public abstract class DynamicComponent : Component
    {
        public bool FixedUpdatesEnabled {get; set;} = true;
        // Determines if DynamicComponent gets FixedUpdates.
        internal bool IgnoreObjmap { get; set; } = true; // Determines if this Object uses a box for collisions, ignoring the Objmap link.
        internal bool AllowCollisions { get; set; } = true; // Determines if this Object registers collisions.
        internal string ObjmapLink { get; set; } = string.Empty; // Objmap linked to this Dynamic Component.

        public abstract void FixedUpdate(object sender, EventArgs e);

        protected DynamicComponent()
        {
            GameInstance.CurrentGameInstance.FixedUpdateEvent += FixedUpdate;
        }
    }
}