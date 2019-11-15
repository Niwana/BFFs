using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject mousePointer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 10.0f; //distance of the plane from the camera
           // transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

            mousePointer.transform.position = Camera.main.ScreenToWorldPoint(screenPoint); ;
        }
    }
}
