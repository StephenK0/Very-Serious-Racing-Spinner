using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNavigation : MonoBehaviour
{

    public void NavigateToLevelCompleteScene()
    {
        SceneManager.LoadScene("LevelCompleteMenu");
    }


    public void NavigateToLevelFailedScene()
    {
        SceneManager.LoadScene("LevelFailedMenu");
    }


    // Level navigation function (called whenever the player completes a level).
    public void NextLevel()
    {
        StaticData.currentLevel++;
        string nextLevelName = "Level" + StaticData.currentLevel.ToString();
        SceneManager.LoadScene(nextLevelName);
    }

    // Level failed function (called whenever the player fails a level).
    public void RestartLevel()
    {
        string currentLevelName = "Level" + StaticData.currentLevel.ToString();
        SceneManager.LoadScene(currentLevelName);
    }

    // TODO: save the player's data (position, stats, and so forth).
    public void PauseLevel()
    {
        StaticData.pausedLevelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("PauseMenu");
    }
}
