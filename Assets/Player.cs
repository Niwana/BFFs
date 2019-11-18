using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Girl;
    public float force_magn = 1.0f;
    //public GameObject target;

    private HingeJoint joint;
    private Vector3 startPos;
    public float maxSpeed = 20f;//Replace with your max speed

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        startPos = gameObject.transform.position;

       // girlForce = Girl.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (joint != null)
        {
            Debug.DrawLine(transform.position, joint.anchor + startPos);
        }

        if (joint != null && Physics.Linecast(transform.position, joint.anchor + startPos))
        {
            Debug.Log("blocked");

            Destroy(joint);
            GetComponent<SphereCollider>().enabled = true;
        }



        if (Input.GetKey("left"))
        {

            if (Girl.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed)
            {
                Girl.GetComponent<Rigidbody>().AddForce(-force_magn, 0, 0);
                Debug.Log("Speed" + Girl.GetComponent<Rigidbody>().velocity.magnitude);
            }
        }

        if (Input.GetKey("right"))
        {

            if (Girl.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed)
            {
                Girl.GetComponent<Rigidbody>().AddForce(force_magn, 0, 0); //gånger cos(vinkel)
                Debug.Log("Speed" + Girl.GetComponent<Rigidbody>().velocity.magnitude);
            }
        }
    }
}
