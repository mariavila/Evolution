using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float sensitivity = 10f;
    public float speed = 20f;

    private Transform c;

    private void Start()
    {
        c = Camera.main.transform;
    }
    void Update()
    {
        /**
        if(!Input.GetMouseButton(0))
        {
            c.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
        }
        else
        {
            float rotCameraY = c.rotation.eulerAngles.y;
            
            Vector3 posZ = new Vector3(Mathf.Cos(rotCameraY), 0f, Mathf.Sin(rotCameraY));
            
            posZ = posZ.normalized * sensitivity * Input.GetAxis("Mouse Y");
            
            Vector3 posX = new Vector3(Mathf.Cos((rotCameraY+90)%360), 0f, Mathf.Sin((rotCameraY+90)%360));
            posX = posX.normalized * sensitivity * Input.GetAxis("Mouse X");
            c.position = c.position + posZ + posX;
            //c.position = c.position + new Vector3(Input.GetAxis("Mouse Y") * sensitivity, 0, 0);
            //c.position = c.position + new Vector3(0, 0, -Input.GetAxis("Mouse X") * sensitivity);
        }
        */
        if (Input.GetMouseButton(0))
        {
            c.position = c.position + new Vector3(Input.GetAxis("Mouse X") * sensitivity, 0, 0);
            c.position = c.position + new Vector3(0, 0, Input.GetAxis("Mouse Y") * sensitivity);

        }
    }
}