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
        // Handle Jump input in Update to make it responsive
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
        // Calculate movement vector
        Vector3 localMovement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 movement = transform.TransformDirection(localMovement);

        bool isMoving = movement.sqrMagnitude > 0.01f;
        bool isSprinting = isMoving && Input.GetKey(KeyCode.LeftShift);

        isGrounded = IsTouchingLayer("Ground");

        if (isGrounded)
        {
            // Full control when grounded
            HandleMovement(movement, isSprinting);
        }
        else
        {
            // Limited horizontal control while in air
            ApplyAirControl(movement);
        }

        // Handle rotation (works both on ground and in air)
        HandleRotation(moveHorizontal);

        // Set animator parameters
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isSprinting", isSprinting);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isLeft", isLeft);
        animator.SetBool("isRight", isRight);
    }

    void Jump()
    {
        // Reset vertical velocity to ensure consistent jump height
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        rb.velocity = velocity;

        // Apply the jump force
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void HandleMovement(Vector3 movement, bool isSprinting)
    {
        float speedMultiplier = isSprinting ? SprintSpeed : nonSprintSpeed;
        Vector3 desiredVelocity = movement * movementSpeed * speedMultiplier;

        // Limit velocity to maxSpeed
        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxSpeed * speedMultiplier);

        // Apply force to achieve desired velocity
        rb.AddForce(desiredVelocity - rb.velocity, ForceMode.VelocityChange);
    }

    void ApplyAirControl(Vector3 movement)
    {
        // Only apply horizontal movement in the air (gravity handles the vertical)
        Vector3 airMovement = new Vector3(movement.x, 0.0f, movement.z);

        // Apply a reduced force for air control to avoid full control while airborne
        rb.AddForce(airMovement * movementSpeed * 0.2f, ForceMode.Acceleration); // Adjust 0.2f for more or less air control
    }

    void HandleRotation(float moveHorizontal)
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            // Rotate player based on A/D input
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                float rotationAmount = moveHorizontal * rotationSpeed * Time.deltaTime;
                transform.Rotate(0f, rotationAmount, 0f);
            }
        }

        // Rotate the player based on mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float rotationAmountMouse = mouseX * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationAmountMouse, 0f);
    }

    bool IsTouchingLayer(string layerName)
    {
        bool isTouching = Physics.Raycast(transform.position + raycastOffset, Vector3.down, 1.3f, LayerMask.GetMask(layerName));
        Debug.DrawRay(transform.position + raycastOffset, Vector3.down * 1.3f, isTouching ? Color.green : Color.red);
        return isTouching;
    }

}
