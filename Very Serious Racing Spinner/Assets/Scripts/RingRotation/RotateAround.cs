using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] float speed;
    void Update()
    {
        transform.Rotate(0, speed, 0);
    }
}
