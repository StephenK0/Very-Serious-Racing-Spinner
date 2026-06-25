using UnityEngine;

//Keeps track of how far on the track the player has progressed. 

public class Checkpoint : MonoBehaviour
{
  public bool FinishLine; //Whether entering this zone should start a new lap. 
  public string Name; //Used to identify a Checkpoint if the game is paused and the objects themselves are destroyed. Generally, these should uniquely identify a checkpoint. 
  public string Next; //Used to identify the next checkpoint after this. 

  const string PLAYER_TAG_NAME = "Player";

  void Start() {
    if(string.IsNullOrEmpty(Name)) Name = gameObject.name; //If no name was provided, default initializes to the gameObject's name. 
  }

  void OnTriggerEnter(Collider other) {
    if(other.gameObject.tag == PLAYER_TAG_NAME) {
      RaceManager.TryMoveCheckpoint(this);
      Debug.Log("Reached Checkpoint: " + Name);
    }
  }

}
