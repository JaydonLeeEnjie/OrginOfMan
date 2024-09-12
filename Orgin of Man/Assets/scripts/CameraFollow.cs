//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraFollow : MonoBehaviour
//{
//    public Transform player; // Set this in the Inspector
//    public float distance = 5.0f; // Set this in the Inspector
//    public float height = 2.0f; // Set this in the Inspector
//    public float rotationSpeed = 5.0f; // Set this in the Inspector
//    public float xRotation = 30.0f; // Set this in the Inspector to control the x rotation of the camera

//    private float mouseX = 0;

//    void LateUpdate()
//    {
//        // Update the mouseX variable based on mouse movement
//        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;

//        // Calculate the camera's position
//        Vector3 cameraPosition = new Vector3(player.position.x + Mathf.Sin(mouseX) * distance, player.position.y + height, player.position.z + Mathf.Cos(mouseX) * distance);
//        transform.position = cameraPosition;


//        // Calculate the camera's rotation
//        Quaternion rotation = Quaternion.Euler(xRotation, mouseX * Mathf.Rad2Deg, 0);
//        transform.rotation = rotation;


//        // Make the camera look at the player
//        transform.LookAt(player);


//        transform.rotation = rotation;
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Set this in the Inspector
    public float distance = 5.0f; // Set this in the Inspector
    public float height = 2.0f; // Set this in the Inspector

    void LateUpdate()
    {
        // Calculate the camera's position
        Vector3 cameraPosition = new Vector3(player.position.x + distance, player.position.y + height, player.position.z);
        transform.position = cameraPosition;

        // Make the camera look at the player
        transform.LookAt(player);
    }
}