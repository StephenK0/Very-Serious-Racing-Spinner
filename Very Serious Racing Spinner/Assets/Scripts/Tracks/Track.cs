using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] float directionFieldForce = 2.5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            // Direction force field.
            var direction = transform.right;
            rb.AddForce(directionFieldForce * direction);

            Debug.Log(directionFieldForce * direction);
        }
    }
}
