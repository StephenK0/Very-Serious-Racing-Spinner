using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void BackButton()
    {
        SceneManager.LoadScene(sceneName);
    }
}
