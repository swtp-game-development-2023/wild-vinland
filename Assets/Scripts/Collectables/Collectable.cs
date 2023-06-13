using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;


//This interface Collectable  guarantees that a player can collect an object.

public abstract class Collectable : MonoBehaviour
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

    private int GetId() {
        return _id;
    }

    protected Collectable(int id)
    {
        _id = id;
    }

    public override string ToString()
    {
        return _id.ToString();
    }
}