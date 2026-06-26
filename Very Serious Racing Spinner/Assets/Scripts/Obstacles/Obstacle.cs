using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    //[SerializeField] float slowDownFraction = 0.25f;
    [SerializeField] int slowDownFrames = 150;

    // Slow down the player if the player collides with a "SlowDownObstacle".
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag) && gameObject.tag.Equals("SlowDownObstacle"))
        {
            CarController car = other.gameObject.GetComponent<CarController>();
	    car.Slowdown = slowDownFrames;
            //Debug.Log("SPEED BEFORE: " + car.maxMotorSpeed);
            //car.maxMotorSpeed = car.maxMotorSpeed * slowDownFraction;
            Destroy(gameObject);
            //Debug.Log("SPEED AFTER: " + car.maxMotorSpeed);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
