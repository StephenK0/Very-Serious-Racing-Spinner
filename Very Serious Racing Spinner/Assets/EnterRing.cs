using UnityEngine;

//Detects when the player enters a ring and exits it. 

public class EnterRing : MonoBehaviour
{
  
  [SerializeField] int ring;

  void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag.Equals("Player")) RotateByRing.main.ringNumber = ring;
  }
  void OnTriggerExit(Collider other) {
    if (other.gameObject.tag.Equals("Player")) RotateByRing.main.ringNumber = ring - 1;
  }
}
