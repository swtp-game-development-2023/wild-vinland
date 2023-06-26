using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DockScript : MonoBehaviour
{
    private ShipMenu shipObj;
    private InputManager input;

    private void OnEnable()
    {
        shipObj = GetComponentInChildren<ShipMenu>();
        input.UI.ShipMenu.Enable();

    }

    private void OnDisable()
    {
        input.UI.ShipMenu.Disable();
    }

    private void Awake()
    {
        input = new InputManager();
    }

    private void Update()
    {
        if (input.UI.ShipMenu.WasPressedThisFrame() && shipObj.isCollided)
        {
            shipObj.ToggleShipMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Body"))
        {
            shipObj.isCollided = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Body"))
        {
            shipObj.isCollided = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Body"))
        {
            shipObj.isCollided = false;
            shipObj.CloseShipMenu();
        }
    }
}
