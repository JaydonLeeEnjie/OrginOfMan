using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float movementSpeed = 10.0f; // Set this in the Inspector
    public float maxSpeed = 5.0f; // Set the maximum speed limit
    public float rotationSpeed = 5.0f; // Set the rotation speed
    private Rigidbody rb;
    private Animator animator; // Add a reference to the Animator component
    private Vector3 previousMousePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the movement vector in local space (relative to the player's rotation)
        Vector3 localMovement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Transform the local movement vector to world space
        Vector3 movement = transform.TransformDirection(localMovement);

        // Check if the player is moving
        bool isMoving = movement.sqrMagnitude > 0.01f; // Adjust the threshold value as needed

        if (isMoving)
        {
            // Calculate the desired velocity
            Vector3 desiredVelocity = movement * movementSpeed;

            // Limit the velocity to the maximum speed
            desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxSpeed);

            // Apply the force to reach the desired velocity
            rb.AddForce(desiredVelocity - rb.velocity, ForceMode.VelocityChange);
        }

        // Rotate the player based on the mouse position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Set the z-distance from the camera
        Vector3 lookDirection = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
        lookDirection.y = 0f; // Ignore the y-axis (up/down)

        // Check if the mouse position has changed
        if (mousePosition != previousMousePosition)
        {
            // Calculate the rotation amount based on the look direction and rotation speed
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            float step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, step);
        }

        // Update the previous mouse position
        previousMousePosition = mousePosition;

        // Set the isWalking boolean in the animator
        animator.SetBool("isWalking", isMoving);
    }
}