using UnityEngine;
using TMPro;

public class HeadsUpDisplay : MonoBehaviour
{
  [SerializeField] CarController car;
  [SerializeField] TMP_Text slowdown;
  [SerializeField] TMP_Text speed;
  [SerializeField] TMP_Text lap;
  void Start()
  {
    
  }
  // Update is called once per frame
  void Update()
  {
    if(true) speed.text = "";
    if(car.Slowdown > 0) slowdown.text = "Slowed! --" + car.Slowdown;
    else slowdown.text = "";
  }
}
