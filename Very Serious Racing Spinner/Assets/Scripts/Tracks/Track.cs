using UnityEngine;
using UnityEngine.UIElements;

public class Track : MonoBehaviour
{
    [SerializeField] float ring1Force = 2.5f;
    [SerializeField] float ring2Force = 5f;
    [SerializeField] float ring3Force = 5f;
    [SerializeField] float ring4Force = -10f;

    public float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }


    public float Length(Vector3 a)
    {
        return Mathf.Sqrt(DotProduct(a, a));
    }
    

    public float GetAngle(Vector3 a, Vector3 b)
    {
        return Mathf.Acos(DotProduct(a, b) / (Length(a) * Length(b)));
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            // Obtain the angle between the Player and the vertical plane/line.
            Vector3 verticalVector = new Vector3(0, 0, 5);
            Vector3 playerToCenter = rb.position;

            float angle = GetAngle(verticalVector, playerToCenter);
            Vector3 direction = new Vector3(-Mathf.Sin(angle), 0, Mathf.Cos(angle));
            Debug.Log("ANGLE:" + angle);
            rb.AddForce(direction);

            // Direction force field.
            if (gameObject.tag.Equals("Ring1"))
            {
                //rb.AddForce(ring1Force * direction);
                //Debug.Log(direction);
                //Debug.Log("POSITION:" + rb.position);
            }
            //if (gameObject.tag.Equals("Ring2"))
            //{
            //    rb.AddForce(ring2Force * direction * -1);
            //}
            //if (gameObject.tag.Equals("Ring3"))
            //{
            //    rb.AddForce(ring3Force * direction);
            //}
            //if (gameObject.tag.Equals("Ring4"))
            //{
            //    rb.AddForce(ring4Force * direction * -1);
            //}
        }
    }
}
