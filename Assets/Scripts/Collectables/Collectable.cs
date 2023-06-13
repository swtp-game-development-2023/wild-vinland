using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;


//This interface Collectable  guarantees that a player can collect an object.

public abstract class Collectable
{
    readonly int _id;
    public int maxAmount = 1;
    private int _amount = 0;

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
                    "Value has to be between 0 and" + maxAmount);
            }
        }
    }

    public int GetId()
    {
        return _id;
    }

    protected Collectable(int id)
    {
        _id = id;
    }
    protected Collectable(int id,int amount)
    {
        _id = id;
        _amount = amount;
    }

    ///<summary>
    /// Generates a String representation of the Collectable in Format: CollectableName [Collectable Amount]
    ///</summary>
    public override string ToString()
    {
        return ( (CollectableName) _id).ToString()+"["+_amount+"]";
    }
}