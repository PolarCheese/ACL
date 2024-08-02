using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ACL.UI;
public class ScreenManager
{
    GameInstance Game;
    SpriteBatch SpriteBatch => Game.SpriteBatch;
    ComponentManager ComponentManager => Game.ComponentManager;
    public List<Screen> LoadedScreens {get; private set;} = new();
    public Screen? ActiveScreen;
    public ScreenManager(GameInstance CurrentGame)
    {
        Game = CurrentGame;
    }

    /// <summary> Load a screen into the screen manager. </summary>
    /// <param name="screen"> The screen that will be loaded. </param>
    /// <param name="autoactivation"> Autoactivation. If true, the screen manager will set this screen as the active one. </param>
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
            SetActiveScreen(screen);
        }
    }

    /// <summary> Unload a screen from the screen manager. </summary>
    /// <param name="screen"> The screen that will be unloaded. </param>
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

    /// <summary> Unload a screen from the screen manager. </summary>
    /// <param name="screen"> The screen that will be unloaded. </param>
    public void SetActiveScreen(Screen screen)
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
        SpriteBatch.Begin(samplerState: Game.SpritebatchSamplerState);
        ActiveScreen?.Draw(gameTime);
        SpriteBatch.End();
    }
}