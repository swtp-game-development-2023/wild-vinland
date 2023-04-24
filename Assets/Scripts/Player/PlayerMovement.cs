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

    //TODO: add different movement types
    enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 _movementInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private Directions currentDircetion = Directions.Right;

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
    private void Update()
    {
        getDirection();
        faceDirection();
        animator.SetFloat("Speed", _smoothMovementInput.sqrMagnitude);
    }

    //note the direction by the current user input
    void getDirection()
    {
        if (_movementInput.x == left)
        {
            currentDircetion = Directions.Left;
        }
        else if (_movementInput.x == right)
        {
            currentDircetion = Directions.Right;
        }
        else if (_movementInput.y == up)
        {
            currentDircetion = Directions.Up;
        }
        else if (_movementInput.y == down)
        {
            currentDircetion = Directions.Down;
        }
    }

    //let the character in the walking direction
    void faceDirection()
    {
        Vector3 scale = transform.localScale;
        Quaternion rotation = transform.rotation;
        //TODO: reduce magic numbers
        switch (currentDircetion)
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
                    rotation.z = 0.7f;
                    break;
                }
                case Directions.Down:
                {
                    scale.x = 1;
                    rotation.z = -0.7f;
                    break;
                }
        }
        transform.localScale = scale;
        transform.rotation = rotation;
    }

    //default Method for movement keys
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}