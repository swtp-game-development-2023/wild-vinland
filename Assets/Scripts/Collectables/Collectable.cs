using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;


//This interface Collectable  guarantees that a player can collect an object.

public abstract class Collectable : MonoBehaviour
{
    private int _id = -1;
    public int maxAmount = 1;
    private int _amount = 0;
    
    [SerializeField]
    private Sprite sprite;

    public Sprite Sprite
    {
        get => sprite;
        set => sprite = value;
    }

    public int Amount
    {
        get => _amount;
        set
        {
            if (_amount <= maxAmount)
            {
                _amount = value;
            }
            
            else
            {
                throw new ArgumentOutOfRangeException(nameof(_amount), value,
                    "Value has to be between 0 and " + maxAmount);
            }
            
        }
    }

    public int ID {
        get => _id;
        set
        {
            if (_id == -1) {
                _id = value;
            }
        }
    }
    
    ///<summary>
    /// Generates a String representation of the Collectable in Format: CollectableName [Collectable Amount]
    ///</summary>
    public override string ToString()
    {
        return ( (CollectableName) _id).ToString()+"["+_amount+"]";
    }
}