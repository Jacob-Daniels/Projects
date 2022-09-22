using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("Components:")]
    public CharacterController characterController;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Slider sprintSlider;

    [Header("Movement:")]
    public bool canMove = true;
    public float moveSpeed;
    public float walkSpeed = 10f;
    private float horizontalMovement;
    private float verticalMovement;

    [Header("Jumping:")]
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public Vector3 velocity;
    private bool grounded;
    public float groundDistance = 0.3f;

    [Header("Sprinting:")]
    public float sprintSpeed = 15f;
    public float sprintBarIncrease = 15f;
    private float sprintBar = 25f;
    public float sprintCooldown = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
        characterController = GetComponent<CharacterController>();
        groundCheck = transform.Find("GroundCheck").GetComponent<Transform>();
        sprintSlider = GameObject.Find("SprintBar").GetComponent<Slider>();
        sprintSlider.maxValue = sprintBar;
    }

    void Update()
    {
        // Grab input if player can move
        if (canMove)
        {
            Walk();
            Jump();
            Sprint();
        } else
        {
            SlowDown();
        }

        // Player movement
        Vector3 playerMovement = transform.right * horizontalMovement + transform.forward * verticalMovement;
        characterController.Move(playerMovement * moveSpeed * Time.deltaTime);
        // Player Jumping
        characterController.Move(velocity * Time.deltaTime);
    }

    private void SlowDown()
    {
        CheckGrounded();
        // Update sprint bar
        if (sprintCooldown <= 0f)
        {
            sprintBar += sprintBarIncrease * Time.deltaTime;
        }
        else
        {
            sprintCooldown -= Time.deltaTime;
        }
        sprintSlider.value = sprintBar;
        // Slow down walk from walk/sprinting at same rate
        if (moveSpeed > walkSpeed)
        {
            moveSpeed = walkSpeed;
        } else if (moveSpeed > 0)
        {
            moveSpeed -= sprintSpeed * Time.deltaTime;
        }
    }

    public void Sprint()
    {
        if (Input.GetKey(Controls.instance.sprint))
        {
            sprintCooldown = 1f;
            // Check if player can sprint
            if (sprintBar > 0)
            {
                sprintBar -= sprintBarIncrease * Time.deltaTime;
                moveSpeed = sprintSpeed;
            } else
            {
                moveSpeed = walkSpeed;
            }
        } else
        {
            // Sprint bar cooldown
            if (moveSpeed > walkSpeed)
            {
                moveSpeed -= walkSpeed * Time.deltaTime;
            }
            else
            {
                moveSpeed = walkSpeed;
            }
            if (sprintBar > 25)
            {
                sprintBar = 25f;
            }
            else
            {
                // Cooldown before sprint bar resets
                if (sprintCooldown <= 0f)
                {
                    sprintBar += sprintBarIncrease * Time.deltaTime;
                } else
                {
                    sprintCooldown -= Time.deltaTime;
                }
            }
        }
        // Update UI Slider
        sprintSlider.value = sprintBar;
    }

    public void Walk()
    {
        // Input from Controls script
        if (Input.GetKey(Controls.instance.movement[0])) // Forwards movement
        {
            verticalMovement = 1;
        } else if (Input.GetKey(Controls.instance.movement[1])) // Backwards movement
        {
            verticalMovement = -1;
        } else
        {
            verticalMovement = 0;
        }

        if (Input.GetKey(Controls.instance.movement[2])) // Left movement
        {
            horizontalMovement = -1;
        } else if (Input.GetKey(Controls.instance.movement[3])) // Right movement
        {
            horizontalMovement = 1;
        } else {
            horizontalMovement = 0;
        }
    }

    public void Jump()
    {
        CheckGrounded();

        // Jump Input
        if (Input.GetButton("Jump") && grounded)
        {
            characterController.slopeLimit = 90f;
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    private bool CheckGrounded()
    {
        // Check is player is grounded
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Jump Y velocity
        velocity.y += gravity * Time.deltaTime;
        if (grounded && velocity.y < 0f)
        {
            characterController.slopeLimit = 45f;
            velocity.y = -2f;
        }
        return grounded;
    }
}
