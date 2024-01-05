using Microsoft.Xna.Framework;

namespace ACL.UI
{
    public class ScreenManager
    {
        GameInstance Game;
        ComponentManager ComponentManager => Game.componentManager;
        public List<Screen> LoadedScreens {get; private set;} = new List<Screen>();
        Screen? ActiveScreen;
        public ScreenManager(GameInstance CurrentGame)
        {
            Game = CurrentGame;
        }

        public void LoadScreen(Screen screen, bool autoactivation = true)
        {
            // Set screen as loaded.
            if (!LoadedScreens.Contains(screen))
            {
                screen.Game = Game;
                LoadedScreens.Add(screen);

                // Trigger screen OnLoad() method.
                screen.OnLoad();
            }

            // Check if the screen is set to automatically activate.
            if (autoactivation)
            {
                SetCurrentScreen(screen);
            }
        }
        public void UnloadScreen(Screen screen)
        {
            // Check if the screen is loaded.
            if (LoadedScreens.Contains(screen))
            {
                // Check if screen is currently active.
                if (ActiveScreen == screen)
                {
                    screen.OnUnactivation();
                    ComponentManager.RemoveComponentsRange(screen.ScreenComponents);
                    ActiveScreen = null;
                }

                // Unload the screen.
                screen.OnUnload();
                LoadedScreens.Remove(screen);
            }
        }
        public void SetCurrentScreen(Screen screen)
        {
            // Check if the screen is loaded and isn't already active.
            if (screen != null && LoadedScreens.Contains(screen) && ActiveScreen != screen)
            {
                // Trigger Unactivation method on already existing current screen.
                if (ActiveScreen != null)
                {
                    ActiveScreen.OnUnactivation();
                    ComponentManager.RemoveComponentsRange(ActiveScreen.ScreenComponents);
                }

                // Set the screen as the current.
                ActiveScreen = screen;
                ComponentManager.AddComponentsRange(screen.ScreenComponents);
                screen.OnActivation();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (ActiveScreen != null)
            {
                ActiveScreen.Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime)
        {
            if (ActiveScreen != null)
            {
                ActiveScreen.Draw(gameTime);
            }
        }
    }
}