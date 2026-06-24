using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] float ring1Force = 2.5f;
    [SerializeField] float ring2Force = 5f;
    [SerializeField] float ring3Force = 5f;
    [SerializeField] float ring4Force = -10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            // Direction force field.
            if (gameObject.tag.Equals("Ring1"))
            {
                var direction = transform.right;
                rb.AddForce(ring1Force * direction);
            }
            if (gameObject.tag.Equals("Ring2"))
            {
                var direction = transform.right;
                rb.AddForce(ring2Force * direction);
                Debug.Log(ring2Force * direction);
            }
            if (gameObject.tag.Equals("Ring3"))
            {
                var direction = transform.right;
                rb.AddForce(ring3Force * direction);
            }
            if (gameObject.tag.Equals("Ring4"))
            {
                var direction = transform.right;
                rb.AddForce(ring4Force * direction);
            }
        }
    }
}
