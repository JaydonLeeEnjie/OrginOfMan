using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Vector3 newPosition; // Set this in the Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.position = newPosition;
        }
    }
}