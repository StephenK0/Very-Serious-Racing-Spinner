using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string settingsScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SettingsMenu()
    {
        SceneManager.LoadScene(settingsScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
