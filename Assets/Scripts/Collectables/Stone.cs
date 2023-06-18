using System.Collections;
using System.Collections.Generic;
using Collectables;
using UnityEngine;

public class Stone : Ressource
{
    private void Awake() {
        ID = (int) CollectableName.Stone;
    }
}
