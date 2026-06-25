using UnityEngine;

public class RaceManager 
{
  static string current;
  static string next;
  static bool isFL;

  const int LAPS_IN_RACE = 3;

  static int lap = 1; //One-indexed. Not zero-indexed. Race ends when you reach the end of lap 3, where lap 4 would begin. 

  public static void TryMoveCheckpoint(Checkpoint other) {
    if(current == null) {
      SetCurrent(other);
      return; //When you go through the finish line for the first time at the start of the race, it won't count as a lap. 
    }

    if(other.Next == current) { //If this is the previous checkpoint, move backwards. 
      if(isFL) AddLaps(-1);
      SetCurrent(other);
    }
    else if(other.name == next) { //If this is the next checkpoint, move forwards. 
      if(other.FinishLine) AddLaps(1);
      SetCurrent(other);
    }
    else return; //Remove this line when you remove the Debug.Log statement directly after it. 
    Debug.Log("lap " + lap + " at checkpoint " + current);
  }

  private static void AddLaps(int howMany = 1) {
    lap += howMany;

    if(lap > LAPS_IN_RACE) {
      EndRace();
    }
  }

  private static void SetCurrent(Checkpoint other) {
    current = other.Name;
    next = other.Next;
    isFL = other.FinishLine;
  }

  //TODO!!
  private static void EndRace(bool won = true) {
    current = null;
    next = null;
    isFL = false;
    lap = 1;

    if(won) {
	    Debug.Log("You won!");
    }
    else {
	    Debug.Log("You lost!");
    }
  }
}
