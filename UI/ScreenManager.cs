using Microsoft.Xna.Framework;

namespace ACL.UI
{
    public class ScreenManager
    {
        GameInstance Game;
        ComponentManager ComponentManager => Game.componentManager;
        public List<Screen> LoadedScreens {get; private set;} = new List<Screen>();
        Screen? CurrentScreen;
        public ScreenManager(GameInstance CurrentGame)
        {
            Game = CurrentGame;
        }

        public void LoadScreen(Screen screen, bool autoactivation = true)
        {
            // Set screen as loaded.
            if (LoadedScreens.Contains(screen) == false)
            {
                screen.Game = Game;
                LoadedScreens.Add(screen);
            }

            // Trigger screen OnLoad() method.
            screen.OnLoad();

            // Check if the screen is set to automatically activate.
            if (autoactivation == true)
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
                if (CurrentScreen == screen)
                {
                    CurrentScreen.OnUnactivation();
                    ComponentManager.RemoveComponentsRange(CurrentScreen.ScreenComponents);
                    CurrentScreen = null;
                }

                // Unload the screen.
                screen.OnUnload();
                LoadedScreens.Remove(screen);
            }
        }
        public void SetCurrentScreen(Screen screen)
        {
            // Check if the screen is loaded.
            if (screen != null && LoadedScreens.Contains(screen))
            {
                // Trigger Unactivation method on already existing current screen.
                if (CurrentScreen != null)
                {
                    CurrentScreen.OnUnactivation();
                }

                // Set the screen as the current.
                CurrentScreen = screen;
                ComponentManager.AddComponentsRange(CurrentScreen.ScreenComponents);
                CurrentScreen.OnActivation();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime)
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.Draw(gameTime);
            }
        }
    }
}