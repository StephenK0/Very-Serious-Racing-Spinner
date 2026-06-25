using UnityEngine;
using TMPro;

public class LeaderboardCalculator : MonoBehaviour
{
  [SerializeField] TMP_Text leaderboard;
  public static int mostRecentScore;

  void Start()
  {
    StaticData.AddHighScore(mostRecentScore);
    leaderboard.text = "Clear Time: " + mostRecentScore + "\n\n" + "Best: ";
    foreach(int i in StaticData.GetHighScores()) {
      leaderboard.text += "\n- " + i;
    }
    mostRecentScore = 0;
  }
}
