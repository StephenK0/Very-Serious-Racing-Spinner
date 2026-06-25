using UnityEngine;

public class go : MonoBehaviour
{
    public float speed = 0.001f;
    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(speed, 0, 0));
    }
}
