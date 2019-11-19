using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Girl;
    public float force_magn = 1.0f;
    public float maxSpeed = 20f;//Replace with your max speed
    public int connectedSpringForce = 10;

    private HingeJoint joint;
    private Vector3 startPos;
    private bool hasCollided;
    private bool foundPartner;
    private bool inGoal;


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
            Debug.DrawLine(transform.position, joint.anchor + startPos); // Olny for debugging
        }

        // Cut the join if the mouse is dragged over it
        if (joint != null && Physics.Linecast(transform.position, joint.anchor + startPos))
        {
            Debug.Log("blocked");

            Destroy(joint);
            GetComponent<SphereCollider>().enabled = true;
        }


        if (!hasCollided && !inGoal)
        {
            // Swing Players
            if (Input.GetKey("left"))
            {

                if (Girl.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed)
                {
                    Girl.GetComponent<Rigidbody>().AddForce(-force_magn, 0, 0);
                    //Debug.Log("Speed" + Girl.GetComponent<Rigidbody>().velocity.magnitude);
                }
            }

            if (Input.GetKey("right"))
            {

                if (Girl.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed)
                {
                    Girl.GetComponent<Rigidbody>().AddForce(force_magn, 0, 0); //gånger cos(vinkel)
                    //Debug.Log("Speed" + Girl.GetComponent<Rigidbody>().velocity.magnitude);
                }
            }
        }


        //Debug.Log("Object: " + hasCollided);
        //Debug.Log("Goal: " + inGoal);
        Debug.Log("Player: " + foundPartner);
    }


    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Collision " + other.gameObject.tag);

        if (other.gameObject.tag == "Object")
        {
            hasCollided = true;
        }

        if (other.gameObject.tag == "Goal")
        {
            inGoal = true;
        }

        if (other.gameObject.tag == "Player")
        {
            // Add spring between players
            if (!foundPartner)
            {
                SpringJoint springJoint = gameObject.AddComponent<SpringJoint>();
                springJoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                springJoint.spring = connectedSpringForce;
            }




            foundPartner = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Object")
        {
            hasCollided = false;
        }

        if (other.gameObject.tag == "Goal")
        {
            inGoal = false;
        }

        if (other.gameObject.tag == "Player")
        {
            //foundPartner = false;
        }
    }
}
