using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Collectables;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventorySize = 10;
    [SerializeField] private Sprite[] itemIcons;
    public class EmptySlot : Collectable
    {
        private void Awake()
        {
            ID = -1;
        }
    }

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

    public int ContainsStackableSlot(Collectable c)
    {
        return _inventory.FindIndex(c2 => c2 != null && c.ID == c2.ID && c2.Amount < c2.MaxAmount);
    }

    public int Contains(CollectableName collectableName)
    {
        return _inventory.FindIndex(c => c != null && c.ID == (int)collectableName);
    }

    public int Add(Collectable c)
    {
        int index = ContainsStackableSlot(c);
        if (index != -1 && !_inventory[index].IsMaxAmount())
        {
            c.Amount = Add(c, index);
        }

        if (c.Amount <= 0) return 0;
        index = ContainsStackableSlot(ScriptableObject.CreateInstance<EmptySlot>());

        if (index != -1)
        {
            var newSlot = c.copy();
            newSlot.Amount = 0;
            _inventory[index] = newSlot;
            return Add(c);
        }

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

    public int GetTotalAmount(CollectableName collectableName)
    {
        return _inventory.FindAll(c => c.ID == (int)collectableName).Sum(c => c.Amount);
    }

    public void Remove(CollectableName collectableName, int amount)
    {
        int index = Contains(collectableName);
        if (index == -1 || _inventory[index].Amount <= 0)
            throw new Exception("Try to remove more from Inventory than it contains");
        var rest = Remove(amount, index);

        if ( _inventory[index].Amount <= 0)
        {
            _inventory[index] = ScriptableObject.CreateInstance<EmptySlot>();
        }
        
        if (0 < rest)
        {
            Remove(collectableName, rest);
        }


    }

    public int Remove(int amount, int index)
    {
        return _inventory[index].Remove(amount);
    }

    public void ClearSlots()
    {
        if (null != _inventory)
        {
            _inventory.Clear();
            for (int i = 0; i < inventorySize; i++)
            {
                _inventory.Add(ScriptableObject.CreateInstance<EmptySlot>());
            }   
        }
    }

    ///<summary>
    /// returns true if slot at index is empty 
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
            if ( collectable.Amount >= 1 )
            {
                inv.Add(collectable.ID);
                amount.Add(collectable.Amount);
            }
        }

        return new SerializedInventory(inventorySize, inv, amount);
    }

    public void DeSerialize(SerializedInventory serializedInventory)
    {
        GameObject inventoryUiParent = GameObject.Find("ItemsParent");
        inventorySize = serializedInventory.inventorySize;
        Resource resource;
        resource = ScriptableObject.CreateInstance<Resource>();
        for (int i = 0; i < serializedInventory.inventory.Count; i++)
        {
            resource.ID = serializedInventory.inventory[i];
            resource.Amount = serializedInventory.amount[i];
            switch (resource.ID)
            {
                case (int)CollectableName.Wood:
                    resource.Sprite = itemIcons[0];
                    break;
                case (int)CollectableName.Stone:
                    resource.Sprite =  itemIcons[1];
                    break;
                case (int)CollectableName.Ore:
                    resource.Sprite =  itemIcons[2];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.Add(resource.copy());
            resource = ScriptableObject.CreateInstance<Resource>();
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