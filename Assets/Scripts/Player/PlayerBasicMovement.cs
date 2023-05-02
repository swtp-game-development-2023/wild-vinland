using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float moveSpeed = 20f;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    } 
    void Update() {
    } 
    // FixedUpdate is called for a fixed framerate (physik's should take place here)
    private void FixedUpdate() {
    }
}
