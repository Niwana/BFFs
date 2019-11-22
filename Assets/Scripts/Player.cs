﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float force_magn = 1.0f;
    public float maxSpeed = 20f;//Replace with your max speed
    public int connectedSpringForce = 10;
    public int dashPower = 400;

    private GameObject player;
    private ConfigurableJoint joint;
    private Vector3 startPos;
    private bool hasCollided;
    private bool foundPartner;
    private bool inGoal;
    private bool onSpeedBoost;
    private bool canDash = true;

    private GameMaster gameMaster;


    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        joint = GetComponent<ConfigurableJoint>();
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Cut the join if the mouse is dragged over it
        
        if (joint != null && Physics.Linecast(transform.position, joint.anchor + startPos))
        {
            Destroy(joint);
            GetComponent<SphereCollider>().enabled = true;
        }


        if (!hasCollided && !inGoal)
        {
            // Swing Players
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (player.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed && joint != null)
                    player.GetComponent<Rigidbody>().AddForce(-player.transform.right * force_magn);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (player.GetComponent<Rigidbody>().velocity.magnitude < maxSpeed && joint != null)
                    player.GetComponent<Rigidbody>().AddForce(player.transform.right * force_magn);
            }

            // Cut joint
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Destroy(joint);
            }

            // If player is on a speed boost
            if (onSpeedBoost)
            {
                player.GetComponent<Rigidbody>().AddForce(new Vector3(20, 0, 0));
            }
        }

        // Dash
        if (canDash && joint == null)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                player.GetComponent<Rigidbody>().AddForce(new Vector3(-dashPower, 0, 0));
                canDash = false;
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                player.GetComponent<Rigidbody>().AddForce(new Vector3(dashPower, 0, 0));
                canDash = false;
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                player.GetComponent<Rigidbody>().AddForce(new Vector3(0, dashPower, 0));
                canDash = false;
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                player.GetComponent<Rigidbody>().AddForce(new Vector3(0, -dashPower, 0));
                canDash = false;
            }
        }


        // Check win state
        if (inGoal && foundPartner)
        {
            gameMaster.StartLevelCountdown();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
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
        
        if (other.gameObject.tag == "SpeedBoost")
        {
            hasCollided = true;
            onSpeedBoost = true;
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

        if (other.gameObject.tag == "SpeedBoost")
        {
            hasCollided = false;
            onSpeedBoost = false;
        }
    }
}
