using UnityEngine;

//Makes the camera gradually follow one position, while instantly looking at another. 
//Based in part on the camera script that came with the racecars, CameraFollow.cs

public class MoveCamera : MonoBehaviour
{
  [SerializeField] Transform goTo;
  [SerializeField] Transform lookAt;
  [SerializeField] float followSpeed = 2;
  void Start()
  {
    transform.position = goTo.position;
    transform.rotation = goTo.rotation;
  }
  
  // Update is called once per frame
  void Update()
  {
    transform.position = Vector3.Lerp(transform.position, goTo.position, followSpeed * Time.deltaTime);
    //transform.rotation = Vector3.RotateTowards(transform.rotation, goTo.position, 0.1f);
    transform.LookAt(lookAt.position);
  }
}
