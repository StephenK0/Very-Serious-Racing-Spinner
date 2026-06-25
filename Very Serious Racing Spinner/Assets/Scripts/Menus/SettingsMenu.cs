using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void BackButton()
    {
        SceneManager.LoadScene(sceneName);
    }

    // TODO: sound/music functionality.
    // NOTE FOR ARTURO: sound/music functionality likely implemented in a separate class and imported in this file and in PauseMenu.cs.
}
