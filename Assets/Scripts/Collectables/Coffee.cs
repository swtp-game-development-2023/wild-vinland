using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

///<summary>
/// An Example Item doing nothing so far besides existing. Item ID = 13
///</summary>
public class Coffee : Collectable
{
    public const double weight = 1.0;
    public const string name = "Coffee";
    private const int id = (int) CollectableName.Coffee;
    public int amount;

    public Coffee (int amount) : base(id,amount)
    {
        if (amount != null) amount = amount;
    }

    public Coffee () : base(id)
    {
        amount = 0;
    }
}
