﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float force_magn = 1.0f;
    public int connectedSpringForce = 10;
    public int dashPower = 400;
    public ParticleSystem hearts;
    public ParticleSystem dashPS;

    private GameObject player;
    private ConfigurableJoint joint;
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (!hasCollided && !inGoal)
        {
            // Swing Players
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (joint != null)
                    player.GetComponent<Rigidbody>().AddForce(-player.transform.right * force_magn);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (joint != null)
                    player.GetComponent<Rigidbody>().AddForce(player.transform.right * force_magn);
            }

            // Cut joint
            if (Input.GetKeyDown(KeyCode.Space) && gameMaster.shownTutorial)
                Destroy(joint);
        }

        // If player is on a speed boost
        if (onSpeedBoost)
            player.GetComponent<Rigidbody>().AddForce((GetComponent<Rigidbody>().velocity.normalized * 25));


        // Dash
        if (canDash && joint == null)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                player.GetComponent<Rigidbody>().AddForce(new Vector3(-dashPower, 0, 0));
                canDash = false;
                ParticleSystem dash = Instantiate(dashPS, transform.position, dashPS.gameObject.transform.rotation);
                dash.gameObject.transform.Rotate(0, 90, 0);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                player.GetComponent<Rigidbody>().AddForce(new Vector3(dashPower, 0, 0));
                canDash = false;
                ParticleSystem dash = Instantiate(dashPS, transform.position, dashPS.gameObject.transform.rotation);
                dash.gameObject.transform.Rotate(0, -90, 0);
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                player.GetComponent<Rigidbody>().AddForce(new Vector3(0, dashPower, 0));
                canDash = false;
                ParticleSystem dash = Instantiate(dashPS, transform.position, dashPS.gameObject.transform.rotation);
                dash.gameObject.transform.Rotate(90, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                player.GetComponent<Rigidbody>().AddForce(new Vector3(0, -dashPower, 0));
                canDash = false;
                ParticleSystem dash = Instantiate(dashPS, transform.position, dashPS.gameObject.transform.rotation);
                dash.gameObject.transform.Rotate(-90, 0, 0);
            }
        }


        // Check win state
        if (inGoal && foundPartner)
            gameMaster.StartLevelCountdown();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Object")
            hasCollided = true;

        if (other.gameObject.tag == "Goal")
            inGoal = true;

        if (other.gameObject.tag == "Player")
        {
            // Add spring between players
            if (!foundPartner)
            {
                SpringJoint springJoint = gameObject.AddComponent<SpringJoint>();
                springJoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                springJoint.spring = connectedSpringForce;
                springJoint.enableCollision = true;

                Instantiate(hearts, GameObject.Find("Boy").transform.position, hearts.transform.rotation);
            }
            foundPartner = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Object")
            hasCollided = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Restart level
        if (other.gameObject.tag == "Respawn")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (other.gameObject.tag == "Goal")
            inGoal = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SpeedBoost")
        {
            hasCollided = true;
            onSpeedBoost = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SpeedBoost")
            onSpeedBoost = false;
    }
}
