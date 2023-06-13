using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressource : Collectable {
    private Inventory inventory;

    public Ressource(int id) : base(id) {
    }
    
    //player can walk over collectable
    //collectible should be destroyed if collected
    //should be dropable
    
    private void Awake() {
        inventory = GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            switch (tag) {
                case "Wood":
                    inventory.Add(this);
                    break;
                case "Stone":
                    break;
                case "Ore":
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
