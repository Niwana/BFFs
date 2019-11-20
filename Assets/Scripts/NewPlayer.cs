using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewPlayer : MonoBehaviour
{
    public float force_magn = 1.0f;
    public float maxSpeed = 20f;//Replace with your max speed
    public int connectedSpringForce = 10;

    private GameObject player;
    private HingeJoint joint;
    private Vector3 startPos;
    private bool hasCollided;
    private bool foundPartner;
    private bool inGoal;

    private GameMaster gameMaster;

    private GameObject[] meshParticles;


    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        joint = GetComponent<HingeJoint>();
        startPos = gameObject.transform.position;
        meshParticles = GetComponent<Jellyfy>().particles;
    }

    // Update is called once per frame
    void Update()
    {
        if (joint != null)
        {
            Debug.DrawLine(transform.position, joint.anchor + startPos); // Olny for debugging
        }


        // Cut the join if the mouse is dragged over it
        /*
        if (joint != null && Physics.Linecast(transform.position, joint.anchor + startPos))
        {
            Destroy(joint);
            GetComponent<SphereCollider>().enabled = true;
        }
        */


        if (!hasCollided && !inGoal)
        {
            // Swing Players
            if (gameObject.name == "Boy")
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Debug.Log(meshParticles.Length);
                    foreach (var particle in meshParticles)
                        particle.GetComponent<Rigidbody>().AddForce(-force_magn, 0, 0);
                }

                if (Input.GetKey(KeyCode.D))
                {

                    if (player.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed && joint != null)
                        player.GetComponent<Rigidbody>().AddForce(player.transform.right * force_magn);
                    else
                        player.GetComponent<Rigidbody>().AddForce(force_magn, 0, 0);
                }
            }
            if (gameObject.name == "Girl")
            {
                if (Input.GetKey(KeyCode.J))
                {

                    if (player.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed && joint != null)
                        player.GetComponent<Rigidbody>().AddForce(-player.transform.right * force_magn);
                    else
                        player.GetComponent<Rigidbody>().AddForce(-force_magn, 0, 0);
                }

                if (Input.GetKey(KeyCode.L))
                {

                    if (player.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed && joint != null)
                        player.GetComponent<Rigidbody>().AddForce(player.transform.right * force_magn);
                    else
                        player.GetComponent<Rigidbody>().AddForce(force_magn, 0, 0);
                }
            }

            // Cut joint
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Destroy(joint);
            }
        }


        // Check win state
        if (inGoal && foundPartner)
        {
            if (gameObject.name == "Girl") // Makes sure that the function only gets called once
            {
                gameMaster.StartLevelCountdown();
            }
        }
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

    private void OnTriggerEnter(Collider other)
    {
        // Restart level
        if (other.gameObject.tag == "Respawn")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
