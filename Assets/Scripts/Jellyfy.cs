using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfy : MonoBehaviour
{
    public GameObject particle;
    public GameObject midParticle;
    public float thresh = 1.0f;
    public float dampener = 1.0f;
    public float innerStrenght = 1f;

    public float startHingeHeight = 30;

    private Mesh mesh;
    private Vector3[] vertices;
    [HideInInspector]
    public GameObject[] particles;

    private Vector3 vert1Pos;
    private Vector3 vert2Pos;
    private Vector3 midPos;
    [HideInInspector]
    public GameObject midPoint;


    void AddSpring(int i0, int i1)
    {
        SpringJoint sj = particles[i0].AddComponent<SpringJoint>() as SpringJoint;
        sj.connectedBody = particles[i1].GetComponent<Rigidbody>();
        sj.spring = 100;
        sj.damper = dampener;
        sj.autoConfigureConnectedAnchor = true;
    }

    private void Awake()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        vertices = mesh.vertices;
        particles = new GameObject[vertices.Length];

        // Spawn particle in the middle of the mesh
        midPos = transform.TransformPoint(new Vector3((vertices[8].x + vertices[142].x) / 2, (vertices[8].y + vertices[142].y) / 2, (vertices[8].z + vertices[142].z) / 2));
        midPoint = Instantiate(midParticle, midPos, Quaternion.identity);
        midPoint.name = "MID PARTICLE"; // Only for debugging
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 pos = transform.TransformPoint(vertices[i]);
            GameObject go = Instantiate(particle, pos, Quaternion.identity);
            particles[i] = go;
        }

        for (int i = 0; i < particles.Length; i++)
        {
            float maxDist = 0;
            int maxDistIndex = 0;
            Vector3 pos0 = particles[i].transform.position;

            // Add joints between particles
            for (int j = 0; j < particles.Length; j++)
            {
                if (i == j) continue;

                Vector3 pos1 = particles[j].transform.position;
                float dist = (pos1 - pos0).magnitude;
                if (dist < thresh)
                    AddSpring(i, j);

                if (dist > maxDist)
                {
                    maxDist = dist;
                    maxDistIndex = j;
                }
            }
            AddSpring(i, maxDistIndex);


            // Add joint from mid particle to all other particles
            SpringJoint midJoint = midPoint.AddComponent<SpringJoint>();
            midJoint.connectedBody = particles[i].GetComponent<Rigidbody>();
            midJoint.spring = innerStrenght;
            midJoint.damper = dampener;
            midJoint.autoConfigureConnectedAnchor = true;
        }

        //Add hinge joint
        HingeJoint startJoint = midPoint.AddComponent<HingeJoint>();
        startJoint.anchor = new Vector3(0, startHingeHeight, 0);
        startJoint.axis = new Vector3(0, 0, 1);
        startJoint.autoConfigureConnectedAnchor = true;


        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.LeftArrow))
            foreach (GameObject p in particles)
                p.GetComponent<Rigidbody>().AddForce(-input_force, 0, 0);

        if (Input.GetKey(KeyCode.RightArrow))
            foreach (GameObject p in particles)
                p.GetComponent<Rigidbody>().AddForce(input_force, 0, 0);

        if (Input.GetKey(KeyCode.UpArrow))
            foreach (GameObject p in particles)
                p.GetComponent<Rigidbody>().AddForce(0, 0, input_force);

        if (Input.GetKey(KeyCode.DownArrow))
            foreach (GameObject p in particles)
                p.GetComponent<Rigidbody>().AddForce(0, 0, -input_force);

        if (Input.GetKey(KeyCode.Space))
            foreach (GameObject p in particles)
                p.GetComponent<Rigidbody>().AddForce(0, -input_force * 10, 0);

    */
        

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 pos = particles[i].transform.position;
            vertices[i] = pos;
        }

        // Calculate the middle of the mesh
        midPos = new Vector3((vertices[8].x + vertices[142].x) / 2, (vertices[8].y + vertices[142].y) / 2, (vertices[8].z + vertices[142].z) / 2);

        // Move mid particle to mid position
        midPoint.transform.position = midPos;


        // Move collision sphere to mid position
        GetComponent<SphereCollider>().center = midPos;



        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }


    // Debugging
    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(midPos, 0.1f);
    }
}
