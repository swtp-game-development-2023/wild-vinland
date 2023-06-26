using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMenu : MonoBehaviour
{
    public bool isShipMenuOpen;
    public bool isCollided;
    public GameObject shipMenu;

    // Start is called before the first frame update
    void Start()
    {
        isCollided = false;
        isShipMenuOpen = false;
        shipMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleShipMenu()
    {
        isShipMenuOpen = !isShipMenuOpen;
        shipMenu.SetActive(isShipMenuOpen);
    }
    
    public void CloseShipMenu()
    {
        isShipMenuOpen = false;
        shipMenu.SetActive(false);
    }
}
