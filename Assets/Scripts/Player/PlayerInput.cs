using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float crouchSpeed = 20f;
    [SerializeField] private float walkSpeed = 50f;
    [SerializeField] private float runSpeed = 100f;
    [SerializeField] private float smoothRate = 0.05f;
    private Vector2 smoothedInput;
    private Vector2 smoothedInputVelocity;
    private Rigidbody2D rb;
    private InputManager input;
    private Vector2 movementVector = Vector2.zero;
    private InputAction movementInput;
    private InputAction runInput;
    private InputAction crouchInput;
    private Animator animator;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsCrouching = Animator.StringToHash("isCrouching");
    private static readonly int attackTrigger = Animator.StringToHash("attackTrigger");
    private Vector2 currentInputVector;

    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private BuildMenu buildMenu;

    private void Awake()
    {
        input = new InputManager();
    }

    private void OnEnable()
    {
        movementInput = input.Player.Move;
        movementInput.Enable();
        runInput = input.Player.Run_Start;
        runInput.Enable();
        crouchInput = input.Player.Crouch_Start;
        crouchInput.Enable();
        input.Player.Fire.Enable();
        input.UI.BuildMenu.Enable();
        input.Player.PauseMenu.Enable();
    }

    private void OnDisable()
    {
        movementInput.Disable();
        runInput.Disable();
        crouchInput.Disable();
        input.Player.Fire.Disable();

        input.Player.PauseMenu.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    private void Update()
    {
        movementVector = movementInput.ReadValue<Vector2>();
        animator.SetBool(IsRunning, runInput.IsPressed());
        animator.SetBool(IsCrouching, crouchInput.IsPressed());
        if (input.Player.Fire.IsPressed())
        {
            if (!pauseMenu.isPaused)
            {
                Fire();
            }
        }

        if (input.Player.PauseMenu.WasPressedThisFrame())
        {
            PauseMenu();
        }
        if (input.UI.BuildMenu.WasPressedThisFrame())
        {
            buildMenu.ToggleBuildMenu();
        }
    }

    //FixedUpdate() is called a fixed framerate
    private void FixedUpdate()
    {
        Movement();
    }

    private void PauseMenu()
    {
        if (pauseMenu.isPaused)
        {
            pauseMenu.ResumeGame();
        }
        else
        {
            pauseMenu.PauseGame();
        }
    }


    private void Fire()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Punch"))
        {
            animator.SetTrigger(attackTrigger);
        }
    }

    private void Movement()
    {
        if (movementVector != Vector2.zero &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Punch"))
        {
            animator.SetBool(IsMoving, true);

            var tempSpeed = 1f;
            if (animator.GetBool(IsRunning))
            {
                tempSpeed = runSpeed;
            }
            else
            {
                if (animator.GetBool(IsCrouching))
                {
                    tempSpeed = crouchSpeed;
                }
                else
                {
                    tempSpeed = walkSpeed;
                }
            }

            smoothedInput = Vector2.SmoothDamp(
                smoothedInput,
                movementVector,
                ref smoothedInputVelocity,
                smoothRate);
            rb.velocity = smoothedInput * tempSpeed * Time.fixedDeltaTime;
        }
        else
        {
            animator.SetBool(IsMoving, false);
            smoothedInput = Vector2.SmoothDamp(
                smoothedInput,
                Vector2.zero,
                ref smoothedInputVelocity,
                smoothRate);
            rb.velocity = smoothedInput * Time.fixedDeltaTime;
        }

        transform.rotation = movementVector.x switch
        {
            < 0 => Quaternion.Euler(0f, 180f, 0f),
            > 0 => Quaternion.Euler(0f, 0, 0f),
            _ => transform.rotation
        };
    }
}