using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public GameObject target;

    private HingeJoint joint;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        startPos = gameObject.transform.position;
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
    }
}
