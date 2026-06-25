using UnityEngine;
using System.Collections.Generic;

public class StaticData : MonoBehaviour
{
    // Store all static data (data that persists throughout the game, regardless of level).
    public static string pausedLevelName;
    public static int currentLevel = 1;
    static List<List<int>> highScores;

    public static void AddHighScore(int score) {
	    if(highScores == null) {
		    highScores = new List<List<int>>();
		    highScores.Add(new List<int>());
		    highScores.Add(new List<int>());

		    highScores[0].Add(231);
		    highScores[0].Add(31);
		    highScores[0].Add(2094809384);
		    highScores[0].Add(2094809384);
		    highScores[0].Add(2094);

		    highScores[1].Add(231);
		    highScores[1].Add(31);
		    highScores[1].Add(2094809384);
		    highScores[1].Add(71);
		    highScores[1].Add(971);
	    }

	    highScores[currentLevel - 1].Add(score);
	    highScores[currentLevel - 1].Sort();
	    highScores[currentLevel - 1] = highScores[currentLevel - 1].GetRange(0, 5);
    }

    public static List<int> GetHighScores() {
         return highScores[currentLevel - 1];
    }
}
