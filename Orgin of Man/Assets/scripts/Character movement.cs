using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float movementSpeed = 10.0f; // Set this in the Inspector
    public float maxSpeed = 5.0f; // Set the maximum speed limit
    public float rotationSpeed = 5.0f; // Set the rotation speed
    public float nonSprintSpeed = 1;
    public float SprintSpeed = 5;
    private Rigidbody rb;
    private Animator animator; // Add a reference to the Animator component
    private Vector3 previousMousePosition;
    private float rotationAmount;
    private bool isGrounded;
    public float jumpForce = 10f;

    public Vector3 raycastOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }
    void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        bool isLeft = moveHorizontal < 0 && moveVertical == 0;
        bool isRight = moveHorizontal > 0 && moveVertical == 0;

        // Calculate the movement vector in local space (relative to the player's rotation)
        Vector3 localMovement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Transform the local movement vector to world space
        Vector3 movement = transform.TransformDirection(localMovement);

        // Check if the player is moving
        bool isMoving = movement.sqrMagnitude > 0.01f; // Adjust the threshold value as needed

        // Check if the player is sprinting (left shift key is pressed)
        bool isSprinting = isMoving && Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S));

        if (isMoving)
        {
            // Calculate the desired velocity
            float speedMultiplier = isSprinting ? SprintSpeed : nonSprintSpeed; // adjust the sprint speed multiplier as needed
            Vector3 desiredVelocity = movement * movementSpeed * speedMultiplier;

            // Limit the velocity to the maximum speed
            desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxSpeed * speedMultiplier);

            // Apply the force to reach the desired velocity
            rb.AddForce(desiredVelocity - rb.velocity, ForceMode.VelocityChange);

    


        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            // If the player is pressing A or D, rotate instead of moving horizontally
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                rotationAmount = moveHorizontal * rotationSpeed * Time.deltaTime;
                transform.Rotate(0f, rotationAmount, 0f);
                moveHorizontal = 0f; // reset horizontal input to prevent movement
            }
        }



        // Rotate the player based on the mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        rotationAmount = mouseX * rotationSpeed * Time.deltaTime;

        // Rotate the player
        transform.Rotate(0f, rotationAmount, 0f);

       

        isGrounded = IsTouchingLayer("Ground");

     
        Debug.Log(isGrounded);
        // Set the isWalking boolean in the animator
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isSprinting", isSprinting);
        animator.SetBool("isLeft", isLeft);
        animator.SetBool("isRight", isRight);
        animator.SetBool("isGrounded", isGrounded);
    }

    bool IsTouchingLayer(string layerName)
    {

        bool isTouching = Physics.Raycast(transform.position + raycastOffset, Vector3.down, 1.3f, LayerMask.GetMask(layerName));
        Debug.DrawRay(transform.position + raycastOffset, Vector3.down * 1.3f, isTouching ? Color.green : Color.red);
        return isTouching;
    }

    void Jump()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            rb.AddForce(Vector3.up * jumpForce * 10, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
