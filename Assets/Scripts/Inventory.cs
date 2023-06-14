using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int _inventorySize = 10;

    public int InventorySize
    {
        get => _inventorySize;
    }

    private List<Collectable> _inventory;

    ///<summary>
    /// Clears out the and resets whole Inventory to initialized state
    ///</summary>
    public void Awake()
    {
        _inventory = new List<Collectable>(_inventorySize);
        for (int i = 0; i < _inventorySize; i++)
        {
            _inventory.Add(null);
        }
    }

    public int Contains(Collectable c) 
    {
        return _inventory.FindIndex(c2 => c2 != null && c2.ID == c.ID && c2.Amount <= c.maxAmount);
    }

    public bool Add(Collectable c) {
        int index = Contains(c);
        if (index != -1) 
        {
            _inventory[index].Amount += c.Amount;
            return true;
        }
        for (int i = 0; i < _inventory.Count; i++) {
            if (IsSlotEmpty(i))
            {
                Add(c, i);
                return true;
            }
        }
        return false;
    }

    public Collectable Get(int i)
    {
        return _inventory[i];
    }
    
    public void Add(Collectable c, int index)
    {
        if (IsSlotEmpty(index)) _inventory[index] = c;
    }
    
    public void Remove(int index)
    {
        Remove(index, _inventory[index].Amount);
    }

    public void Remove(int index, int amount)
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

    }
    
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
        if ( _inventory != null && index >= 0 && index < _inventorySize )
        {
            return _inventory[index] == null;
        }
        return true;
    }

    public SerializedInventory Serialize()
    {
        List<int> inv = new List<int>();
        List<int> amount = new List<int>();
        for (int i = 0; i < _inventory.Count; i++)
        {
            Collectable collectable = _inventory[i];
            if ( collectable != null ) {
                inv.Add(collectable.ID);
                amount.Add(collectable.Amount);
            }
        }
        return new SerializedInventory(_inventorySize,inv,amount);
    }

    public void DeSerialize(SerializedInventory serializedInventory)
    {
        _inventorySize = serializedInventory.inventorySize;
        for (int i = 0; i < serializedInventory.inventory.Count; i++)
        {
            int item = serializedInventory.inventory[i];
            switch (item)
            {
                case (int) CollectableName.Wood:
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
            sb.Append(" | ").Append(!IsSlotEmpty(i)? _inventory[i].ToString() : "empty" );
            
            if (i % 2 == 1)
            {
                sb.Append(" |\n");
            }
        }
        return sb.ToString();
        
    }
}