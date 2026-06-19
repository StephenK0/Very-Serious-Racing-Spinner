using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNavigation : MonoBehaviour
{

    public void NavigateToLevelCompleteScene()
    {
        SceneManager.LoadScene("LevelCompleteMenu");
    }

    // Level navigation function (called whenever the player completes a level).
    public void NextLevel()
    {
        Debug.Log("TEST");
        StaticData.currentLevel++;
        string nextLevelName = "Level" + StaticData.currentLevel.ToString();
        Debug.Log(nextLevelName);
        SceneManager.LoadScene(nextLevelName);
    }

    public void PauseLevel()
    {
        StaticData.pausedLevelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("PauseMenu");
    }
}
