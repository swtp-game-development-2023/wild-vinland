using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressource : Collectable {
    private Inventory inventory;

    //player can walk over collectable
    //collectible should be destroyed if collected
    //should be dropable
    
    private void Start() {
        inventory = FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("TEST");
        if (other.transform.parent.CompareTag("Player")) {
            Amount++;
            inventory.Add(this);
            Debug.Log(inventory.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
