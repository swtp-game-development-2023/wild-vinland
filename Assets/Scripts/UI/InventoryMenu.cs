using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    
    public GameObject inventoryMenu;
    public GameObject cellPrefab;
    public bool isInventoryOpen;
    private Inventory inventory;
    public GameObject inventoryGrid;
    public Sprite emptySlot;
    
    // Start is called before the first frame update
    void Start()
    {
        isInventoryOpen = false;
        inventoryMenu.SetActive(false);
        inventory = FindObjectOfType<Inventory>();

        for (int i = 0; i < inventory.InventorySize; i++)
        {
            var obj = Instantiate(cellPrefab, inventoryGrid.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < inventory.InventorySize; i++)
        {
            var icon = inventoryGrid.transform.GetChild(i).transform.GetChild(0).Find("Icon").GetComponent<Image>();
            
            inventoryGrid.transform.GetChild(i).transform.GetChild(0).Find("AmountText").GetComponent<TMP_Text>().text = inventory.IsSlotEmpty(i)? "0" : inventory.Get(i).Amount.ToString();
            icon.sprite = inventory.IsSlotEmpty(i)? emptySlot : inventory.Get(i).Sprite;
            icon.color = inventory.IsSlotEmpty(i)? new Color32(255,255,225,100) : new Color32(255,255,225,255);
        }
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
