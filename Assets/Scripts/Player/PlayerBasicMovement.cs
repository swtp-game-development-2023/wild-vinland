using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasicMovement : MonoBehaviour {
    
    [SerializeField] private float crouchSpeed = 20f;
    [SerializeField] private float walkSpeed = 50f;
    [SerializeField] private float runSpeed = 100f;
    [SerializeField] private float fastRunSpeed = 180f;
    private Rigidbody2D rb;
    private InputManager input;
    private Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction run;
    private InputAction crouch;
    private Animator animator;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsCrouching = Animator.StringToHash("isCrouching");

    private void Awake() {
        input = new InputManager();
    }

    private void OnEnable() {
        move = input.Player.Move;
        move.Enable();
        run = input.Player.Run_Start;
        run.Enable();
        crouch = input.Player.Crouch_Start;
        crouch.Enable();
    }

    private void OnDisable() {
        move.Disable();
        run.Disable();
        crouch.Disable();
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        moveDirection = move.ReadValue<Vector2>();
        animator.SetBool(IsRunning, run.IsPressed());
        animator.SetBool(IsCrouching, crouch.IsPressed());

        Debug.Log(moveDirection);
    }

    //FixedUpdate() is called a fixed framerate
    private void FixedUpdate() {
        float tempSpeed = 1f;
        if (moveDirection != Vector2.zero) {
            animator.SetBool(IsMoving, true);
            if (animator.GetBool(IsRunning)) {
                tempSpeed = runSpeed;
            }
            if (animator.GetBool(IsCrouching)) {
                tempSpeed = crouchSpeed;
            }
            else {
                tempSpeed = walkSpeed;
            }

            if (moveDirection.x < 0) {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else {
                transform.rotation = Quaternion.Euler(0f, 0, 0f);
            }
            rb.velocity = moveDirection * tempSpeed * Time.fixedDeltaTime; 
        }
        else {
            rb.velocity = moveDirection;
            animator.SetBool(IsMoving, false);
        }
    }
    
    //TODO Movement in Funktionen auslagern
    //TODO Movement smoother machen
    //TODO FastRun einbauen
}