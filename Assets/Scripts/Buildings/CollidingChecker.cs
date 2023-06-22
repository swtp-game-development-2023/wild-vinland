using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingChecker : MonoBehaviour
{
    private bool isColliding;
    private Stack<Collider2D> collider2Ds;
    public bool IsColliding
    {
        get => isColliding;
        set => isColliding = value;
    }

    private void Start()
    {
        collider2Ds = new Stack<Collider2D>();
    }

    private void FixedUpdate()
    {
        isColliding = 0 < collider2Ds.Count;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collider2Ds.Push(other);
    }
    

    void OnTriggerExit2D(Collider2D other)
    {
        collider2Ds.Pop();
    }
}
