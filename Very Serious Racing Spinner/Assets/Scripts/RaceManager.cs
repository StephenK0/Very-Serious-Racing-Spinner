using UnityEngine;

public class RaceManager : MonoBehaviour
{
  string current;
  string next;
  bool isFL;
  public static RaceManager main;
  int timer = 0;
  [SerializeField] LevelNavigation navigator;
  [SerializeField] Checkpoint starting;

  const int LAPS_IN_RACE = 1;

  int lap = 1; //One-indexed. Not zero-indexed. Race ends when you reach the end of lap 3, where lap 4 would begin. 
  void Start() {
    if(main == null) main = this;
    else {
      Debug.Log("Race Manager already exists. Destroying this!");
      Destroy(this);
    }
    TryMoveCheckpoint(starting);
  }

  void FixedUpdate() {
    timer++;
  }

  public void TryMoveCheckpoint(Checkpoint other) {
    Debug.Log("Reaching checkpoint " + other.Name);
    if(current == null) {
      SetCheckpoint(other);
      return; //When you go through the finish line for the first time at the start of the race, it won't count as a lap. 
    }

    if(other.Next == current) { //If this is the previous checkpoint, move backwards. 
      if(isFL) AddLaps(-1);
      SetCheckpoint(other);
    }
    else if(other.name == next) { //If this is the next checkpoint, move forwards. 
      if(other.FinishLine) AddLaps(1);
      SetCheckpoint(other);
    }
    else return; //Remove this line when you remove the Debug.Log statement directly after it. 
    Debug.Log("lap " + lap + " at checkpoint " + current);
  }

  private void AddLaps(int howMany = 1) {
    lap += howMany;

    if(lap > LAPS_IN_RACE) {
      EndRace();
    }
  }

  private void SetCheckpoint(Checkpoint other) {
    current = other.Name;
    next = other.Next;
    isFL = other.FinishLine;
  }

  //TODO!!
  private void EndRace(bool won = true) {
    current = null;
    next = null;
    isFL = false;
    lap = 1;

    if(won) {
	    Debug.Log("You won!");
	    LeaderboardCalculator.mostRecentScore = timer;
	    navigator.NavigateToLevelCompleteScene();
    }
    else {
	    Debug.Log("You lost!");
	    navigator.NavigateToLevelFailedScene();
    }
  }
}
