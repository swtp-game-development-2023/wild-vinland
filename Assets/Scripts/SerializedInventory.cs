using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public struct SerializedInventory
{
    public int inventorySize;
    public List<int> inventory;

    public SerializedInventory(int inventorySize,List<int> itemlist)
    {
        this.inventorySize = inventorySize;
        this.inventory = itemlist;
    }
}
