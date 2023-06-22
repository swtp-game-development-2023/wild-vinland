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


    private void Update()
    {
        Debug.Log(IsColliding);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isColliding = true;
    }

    
    void OnTriggerExit2D(Collider2D other)
    {
        isColliding = false;
        Debug.Log(isColliding +" " + other.gameObject.name);
    }
}
