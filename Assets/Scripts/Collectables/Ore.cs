using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : Ressource
{
    private void Awake() {
        ID = (int) CollectableName.Ore;
    }
}
