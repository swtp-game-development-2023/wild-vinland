using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Collectables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Inventory : MonoBehaviour
{
    public class EmptySlot : Collectable
    {
    }
    [SerializeField] private int inventorySize = 10;

    public int InventorySize
    {
        get => inventorySize;
    }

    private List<Collectable> _inventory;

    ///<summary>
    /// Clears out the and resets whole Inventory to initialized state
    ///</summary>
    public void Awake()
    {
        _inventory = new List<Collectable>(inventorySize);
        for (int i = 0; i < inventorySize; i++)
        {
            _inventory.Add(ScriptableObject.CreateInstance<EmptySlot>());
        }
    }

    public int Contains(Collectable c)
    {
        return _inventory.FindIndex(c2 => c2 != null && c2.ID == c.ID && c2.Amount < c.MaxAmount);
    }

    public int Add(Collectable c)
    {
        Debug.Log(c.ID);
        int index = Contains(c);
        if (index != -1 && !_inventory[index].IsMaxAmount())
        {
            c.Amount = Add(c, index);
        }

        if (c.Amount <= 0) return 0;
        index = Contains(ScriptableObject.CreateInstance<EmptySlot>());
        
        if (index != -1)
        {
            var newSlot = c.copy();
            newSlot.Amount = 0;
            _inventory[index] = newSlot;
            return Add(c);
        }
        /*for (int i = 0; i < _inventory.Count; i++)
        {
            if (IsSlotEmpty(i))
            {
                
            }
        }*/
        return c.Amount;
    }

    public bool AllSlotsFull()
    {
        return _inventory.All(c => c.IsMaxAmount());
    }
    public int Add(Collectable c, int i)
    {
        return _inventory[i].Add(c.Amount);
    }
    public Collectable Get(int i)
    {
        return _inventory[i];
    }

    /*public void Remove(int index)
    {
        Remove(index, _inventory[index].Amount);
    }*/

    /*public void Remove(int index, int amount)
    {
        if (!IsSlotEmpty(index) && amount < _inventory[index].Amount)
        {
            _inventory[index].Amount -= amount;
        }
        else if (!IsSlotEmpty(index) && (amount >= _inventory[index].Amount))
        {
            _inventory[index] = null;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount,
                "Value has to be between 0 and the current amount of the Item: " +_inventory[index].Amount);
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    ///<summary>
    /// Function just to Test adding Coffee Items, calling ToString(), Deleting one calling ToString() again. Demo for Inventory. 
    ///</summary>
    public bool IsSlotEmpty(int index)
    {
        return _inventory[index].GetType() == typeof(EmptySlot);
    }

    public SerializedInventory Serialize()
    {
        List<int> inv = new List<int>();
        List<int> amount = new List<int>();
        for (int i = 0; i < _inventory.Count; i++)
        {
            Collectable collectable = _inventory[i];
            if (!collectable)
            {
                inv.Add(collectable.ID);
                amount.Add(collectable.Amount);
            }
        }

        return new SerializedInventory(inventorySize, inv, amount);
    }

    public void DeSerialize(SerializedInventory serializedInventory)
    {
        inventorySize = serializedInventory.inventorySize;
        for (int i = 0; i < serializedInventory.inventory.Count; i++)
        {
            int item = serializedInventory.inventory[i];
            switch (item)
            {
                case (int)CollectableName.Wood:
                    //TODO _inventory.Add(new Coffee(serializedInventory.amount[i]));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            // TODO other Items based on their Id
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _inventory.Count; i++)
        {
            sb.Append(" | ").Append(_inventory[i].ToString());

            if (i % 2 == 1)
            {
                sb.Append(" |\n");
            }
        }

        return sb.ToString();
    }
}