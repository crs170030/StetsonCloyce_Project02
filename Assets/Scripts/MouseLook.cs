using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 300f;
    public bool timeToStop = false;

    public Transform playerBody;

    float xRotation = 0f;

    //Don't need to have script hide mouse: MenuController does it for us

    // Update is called once per frame
    void Update()
    {
        if (!timeToStop)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
    /*
    public void stopCamera(bool stop)
    {
        Debug.Log("Stopping mouse from moving camera...");
        timeToStop = stop;
    }*/
}
