using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

///<summary>
/// Cantainer for ready to serialize Data for Json Conversion. Index of Inventory equals slot of Item, Index of amount refers to itemslot of item
///</summary>
[Serializable]
public struct SerializedInventory
{
    public int inventorySize;
    public List<int> inventory;
    public List<int> amount;

    public SerializedInventory(int inventorySize,List<int> itemlist,List<int> amount)
    {
        this.inventorySize = inventorySize;
        this.inventory = itemlist;
        this.amount = amount;
    }
}
