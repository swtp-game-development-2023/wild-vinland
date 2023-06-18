using System.Collections;
using System.Collections.Generic;
using Collectables;
using UnityEngine;

public class Wood : Ressource
{
    private void Awake() {
        ID = (int) CollectableName.Wood;
    }
}
