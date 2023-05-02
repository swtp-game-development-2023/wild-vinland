using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int _inventorySize = 10;
    private List<Collectable> _inventory;
    
    

    public void Awake()
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

    private bool IsSlotEmpty(int index)
    {
        return _inventory[index] == null;
    }
    
    private void OnOpenInventory()
    {   
        //TODO real UI
        print(ToString());
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _inventory.Count; i++)
        {
            sb.Append(" | ").Append(!IsSlotEmpty(i)? _inventory.ToString() : "empty" );
            
            if (i % 2 == 1)
            {
                sb.Append(" |\n");
            }
        }
        return sb.ToString();
    }
}