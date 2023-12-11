using Microsoft.Xna.Framework;

namespace ACL.UI
{
    public class ScreenManager
    {
        GameInstance Game;
        ComponentManager ComponentManager => Game.componentManager;
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
            ComponentManager.AddComponentsRange(CurrentScreen.ScreenComponents);

            // Trigger screen OnLoad() method.
            CurrentScreen.OnLoad();
        }
        public void UnloadScreen()
        {
            // Trigger screen OnUnload() method.
            if (CurrentScreen != null)
            {
                CurrentScreen.OnUnload();
            }

            // Unload the screen.
            CurrentScreen = null;
            ComponentManager.Clear();
        }
    }
}