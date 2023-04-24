using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private const float up = 1;
    private const float down = -1;
    private const float left = -1;
    private const float right = 1;
    
    enum MovementTypes
    {
        Idle,
        Walk,
        Crouch,
        Run
    }
    enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }
    
    [SerializeField]
    private float moveSpeed = 1f;
    private const float DefaultMovementSpeed = 5f;
    
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 _movementInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    private Directions _currentDirection = Directions.Right;
    private MovementTypes _currentMovementType = MovementTypes.Idle; 
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsCrouching = Animator.StringToHash("isCrouching");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
    private void Update()
    {
        if (_movementInput != Vector2.zero)
        {
            getDirection();
            faceDirection();
            animator.SetFloat(Speed, _smoothMovementInput.sqrMagnitude);
            
            animator.SetBool(IsMoving, true);

        }
        else
        {
            animator.SetBool(IsMoving, false);
        }
        
    }
    
    //default Method for movement keys
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
        _currentMovementType = MovementTypes.Walk;
    }

    private void OnRun_Start()
    {
        animator.SetBool(IsRunning, true);
        moveSpeed = 10f;
        _currentMovementType = MovementTypes.Run;
    }
    
    private void OnRun_Stop()
    {
        animator.SetBool(IsRunning, false);
        moveSpeed = DefaultMovementSpeed;
        _currentMovementType = MovementTypes.Walk;
    }
    
    private void OnCrouch_Start()
    {
        animator.SetBool(IsCrouching, true);
        moveSpeed = 2.5f;
        _currentMovementType = MovementTypes.Crouch;
    }
    private void OnCrouch_Stop()
    {
        animator.SetBool(IsCrouching, false);
        moveSpeed = DefaultMovementSpeed;
        _currentMovementType = _movementInput != Vector2.zero ? MovementTypes.Walk : MovementTypes.Idle;
    }

    //note the direction by the current user input
    void getDirection()
    {
        if (_movementInput.x == left)
        {
            _currentDirection = Directions.Left;
        }
        else if (_movementInput.x == right)
        {
            _currentDirection = Directions.Right;
        }
        else if (_movementInput.y == up)
        {
            _currentDirection = Directions.Up;
        }
        else if (_movementInput.y == down)
        {
            _currentDirection = Directions.Down;
        }
    }

    //let the character in the walking direction
    void faceDirection()
    {
        Vector3 scale = transform.localScale;
        Quaternion rotation = transform.rotation;
        //TODO: reduce magic numbers
        switch (_currentDirection)
        {
                case Directions.Right:
                {
                    scale.x = 1;
                    rotation.z = 0f;
                    break;
                }
                case Directions.Left:
                {
                    scale.x = -1;
                    rotation.z = 0f;
                    break;
                }
                case Directions.Up:
                {
                    scale.x = 1;
                    // to rotate the player 90° if pressed Up, set rotation.z to 0.7f 
                    rotation.z = 0f;
                    break;
                }
                case Directions.Down:
                {
                    scale.x = 1;
                    // to rotate the player 90° if pressed Down, set rotation.z to -0.7f
                    rotation.z = 0f;
                    break;
                }
        }
        transform.localScale = scale;
        transform.rotation = rotation;
    }
}