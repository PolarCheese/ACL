using Microsoft.Xna.Framework;

namespace ACL.UI
{
    public class ScreenManager
    {
        GameInstance Game;
        ComponentManager ComponentManager => Game.componentManager;
        List<Component> ScreenComponents = new List<Component>();
        Screen? CurrentScreen;
        public ScreenManager(GameInstance CurrentGame)
        {
            Game = CurrentGame;
        }

        public void LoadScreen(Screen screen)
        {
            // Set the screen as the current.
            CurrentScreen = screen;

            // Get the components from the screen.
            UnloadScreen();
            ComponentManager.AddComponentsRange(ScreenComponents);
        }
        public void UnloadScreen()
        {
            CurrentScreen = null;
            ComponentManager.Clear();
        }
    }
}