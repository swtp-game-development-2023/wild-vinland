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
    private const int id = 13;

    public Coffee () : base(id)
    {
    }
}
