using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Track : MonoBehaviour
{
    [SerializeField] float ring1Speed = 10000000f;
    [SerializeField] float ring2Speed = -100f;
    [SerializeField] float ring3Speed = 100f;
    [SerializeField] float ring4Speed = -150f;

    Vector3 direction = new Vector3(1, 0, 0);
    public List<GameObject> onTrack;


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
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag.Equals("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            Vector3 verticalVector = new Vector3(0, 0, 85);
            Vector3 playerToCenter = rb.position;

            float angle = GetAngle(verticalVector, playerToCenter);
            direction = new Vector3(-Mathf.Sin(angle), 0, Mathf.Cos(angle));

            Debug.Log("Player...");
        }


        if (other.gameObject.tag.Equals("Ring1"))
        {
            for (int i = 0; i < onTrack.Count; i++)
            {
                onTrack[i].GetComponent<Rigidbody>().linearVelocity = ring1Speed * direction * Time.deltaTime;
            }
        }

        else if (gameObject.tag.Equals("Ring2"))
        {
            for (int i = 0; i < onTrack.Count; i++)
            {
                onTrack[i].GetComponent<Rigidbody>().linearVelocity = ring2Speed * direction * Time.deltaTime;
            }
        }

        else if (gameObject.tag.Equals("Ring3"))
        {
            for (int i = 0; i < onTrack.Count; i++)
            {
                onTrack[i].GetComponent<Rigidbody>().linearVelocity = ring3Speed * direction * Time.deltaTime;
            }
        }

        else if (gameObject.tag.Equals("Ring4"))
        {
            for (int i = 0; i < onTrack.Count; i++)
            {
                onTrack[i].GetComponent<Rigidbody>().linearVelocity = ring4Speed * direction *Time.deltaTime;
            }
        }




        // OLD CODE:
        // Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        // Obtain the angle between the Player and the vertical plane/line.
        //Vector3 verticalVector = new Vector3(0, 0, 5);
        //Vector3 playerToCenter = rb.position;

        //float angle = GetAngle(verticalVector, playerToCenter);
        //Vector3 direction = new Vector3(-Mathf.Sin(angle), 0, Mathf.Cos(angle));
        //Debug.Log("ANGLE:" + angle);
        //rb.AddForce(direction);


        // 
        //Vector3 forceVector = Quaternion.AngleAxis(90, playerToCenter) * new Vector3(1, 1, 1);



        // Direction force field.
        //if (gameObject.tag.Equals("Ring1"))
        //{
        //    Debug.Log("FORCE RING1:" + forceVector * ring1Force);
        //    rb.AddForce(forceVector * 75);
        //}
        //else if (gameObject.tag.Equals("Ring2"))
        //{
        //    rb.AddForce(ring2Force * forceVector);
        //}
        //else if (gameObject.tag.Equals("Ring3"))
        //{
        //    rb.AddForce(ring3Force * forceVector);
        //}
        //else if (gameObject.tag.Equals("Ring4"))
        //{
        //    rb.AddForce(ring4Force * forceVector);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hello...");
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("TEST!");
            onTrack.Add(collision.gameObject);
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Goodbye...");
        if (collision.gameObject.tag.Equals("Player"))
        {
            onTrack.Remove(collision.gameObject);
        }
    }
}
