using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public bool isInventoryOpen;
    public GameObject inventoryMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        isInventoryOpen = false;
        inventoryMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory()
    {
        isInventoryOpen = true;
        inventoryMenu.SetActive(true);
    }
    
    public void CloseInventory()
    {
        isInventoryOpen = false;
        inventoryMenu.SetActive(false);
    }
    
}
