using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 _movementInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //change the players position based on input
    private void FixedUpdate()
    {
        //for smoother movement
        _smoothMovementInput = Vector2.SmoothDamp(
            _smoothMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.1f);
        rb.velocity = _smoothMovementInput * moveSpeed;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Input
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        
        animator.SetFloat("Horizontal", _smoothMovementInput.x);
        animator.SetFloat("Vertical", _smoothMovementInput.y);
        animator.SetFloat("Speed", _smoothMovementInput.sqrMagnitude);
        print("hallo");
       
    }
    
    //default Method for movement keys
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
