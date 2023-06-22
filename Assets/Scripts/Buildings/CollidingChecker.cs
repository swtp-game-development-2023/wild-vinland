using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingChecker : MonoBehaviour
{
    private bool isColliding;

    public bool IsColliding
    {
        get => isColliding;
        set => isColliding = value;
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        isColliding = true;
    }

    
    void OnTriggerExit2D(Collider2D other)
    {
        isColliding = false;
    }
}
