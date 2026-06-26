using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void BackButton()
    {
	StaticData.currentLevel = 1; //Whenever you return to the main menu, set the level to 1. This is because we reuse this script for the button to return to main menu from the end of the game. 
        SceneManager.LoadScene(sceneName);
    }

    // TODO: sound/music functionality.
    // NOTE FOR ARTURO: sound/music functionality likely implemented in a separate class and imported in this file and in PauseMenu.cs.
}
