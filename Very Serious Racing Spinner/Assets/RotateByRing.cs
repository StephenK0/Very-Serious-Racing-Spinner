using UnityEngine;

public class RotateByRing : MonoBehaviour
{
    public static RotateByRing main;
    public int ringNumber; //Should be initialized when you start the level, since OnTriggerEnter doesn't fire if you start the scene inside a trigger. 
    [SerializeField] float[] speeds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(main == null) main = this;
	else Destroy(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      Rigidbody rb = gameObject.GetComponent<Rigidbody>();
      Quaternion rot = Quaternion.Euler(new Vector3(0, speeds[ringNumber], 0));
      rb.MoveRotation(rb.rotation * rot);
      Debug.Log("Ring: " + ringNumber);
    }
}
