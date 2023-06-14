using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Collectable {
    private Inventory inventory;

    //player can walk over collectable
    //collectible should be destroyed if collected
    //should be dropable

    private void Awake() {
        switch (tag) {
            case "Wood":
                ID = (int) CollectableName.Wood;
                break;
            case "Stone":
                ID = (int) CollectableName.Stone;
                break;
            case "Ore":
                ID = (int) CollectableName.Ore;
                break;
        }
    }

    private void Start() {
        inventory = FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.parent.CompareTag("Player")) {
            Amount++;
            inventory.Add(this);
            Destroy(gameObject);
            Debug.Log(inventory.ToString());
        }
    }
}
