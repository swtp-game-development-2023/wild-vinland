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
    private List<Collectable> _inventory;
    
    public Inventory()
    {
        Clear();
    }

    public Inventory(int size)
    {
        this._inventorySize = size;
        Clear();
    }

    public void Awake()
    {
    }
    
    ///<summary>
    /// Clears out the and resets whole Inventory to initialized state
    ///</summary>
    public void Clear()
    {
        Collectable[] c = new Collectable[_inventorySize];
        _inventory = c.ToList();
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
        if (!IsSlotEmpty(index) & amount < _inventory[index].Amount)
        {
            _inventory[index].Amount -= amount;
        }
        else if (!IsSlotEmpty(index) & (amount == _inventory[index].Amount))
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
    public void Test()
    {
        if( _inventory.Count > 0 )
        {        
            Add(new Coffee(1),0);
            Add(new Coffee(2),1);
            Add(new Coffee(1),2);
            Debug.Log(ToString());
            Debug.Log("is slot 2 empty? "+IsSlotEmpty(2));
            Remove(2);
            Debug.Log("is slot 2 empty? "+IsSlotEmpty(2));
        }
        Debug.Log(ToString());

    }

    private bool IsSlotEmpty(int index)
    {
        if ( _inventory != null )
        {
            return _inventory[index] == null;
        }
        return true;
    } 
    
    private void OnOpenInventory()
    {   
        //TODO real UI
        print(ToString());
    }

    public SerializedInventory Serialize()
    {
        List<int> inv = new List<int>();
        List<int> amount = new List<int>();
        for (int i = 0; i < _inventory.Count; i++)
        {
            Collectable item = _inventory[i];
            if ( item != null )
            {
                inv.Add(item.GetId());
                amount.Add(item.Amount);
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
                case (int) CollectableName.Coffee:
                    _inventory.Add(new Coffee(serializedInventory.amount[i]));
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