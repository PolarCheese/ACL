using Microsoft.Xna.Framework.Graphics;
using ACL.Save;
using ACL.UI;


namespace ACL.Physics
{
    public class PhysicsEngine
    {
        GameInstance Game;
        ComponentManager componentManager => Game.ComponentManager;
        SaveManager saveManager => Game.SaveManager;
        SpriteBatch spritebatch => Game.SpriteBatch;
        private HashSet<int> CheckedPairs = new HashSet<int>();
        public PhysicsEngine(GameInstance CurrentGame)
        {
            Game = CurrentGame;
            Game.FixedUpdateEvent += FixedUpdate;
        }

        #region Processing Methods
        // Methods used for other processes (Checking hash sets, grouping objects etc.)

        private int GetPairHash(DynamicComponent objectA, DynamicComponent objectB) // getHashVal
        {
            // Generate a unique hash value for the object pair
            int hash = objectA.GetHashCode() ^ objectB.GetHashCode();
            return hash;
        }
        #endregion

        #region Physics Methods
        // Methods used for calculating physics, resolving collisions etc.
        public void FixedUpdate(object? sender, EventArgs e)
        {
            // ..
        }

        public void Draw(SpriteFont font)
        {
            // This method will be used for debugging later.
        }

        #endregion

        #region Texture Methods
        // Methods used for procesing Texture data (Getting texture format type, converting to lines etc.)
        #endregion
    }
}