using UnityEngine;

//A simple, quick script that keeps the car from turning on its side. 
//TODO: Maybe make it from flipping over on other axis?

public class Stablize : MonoBehaviour
{
    void Update()
    {
        Vector3 oldRotation = transform.eulerAngles;
	oldRotation.z = 0;
	transform.eulerAngles = oldRotation;
    }
}
