using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FarmableName {
    Tree,
    Stone,
    OreStone
}

public class Farmable : MonoBehaviour {
    [SerializeField] private int health = 1;
    [SerializeField] private int resourceAmount = 1;
    [SerializeField] private Resource resource;
    private int _id = -1;
    private SpriteRenderer _spriteRenderer;
    private bool coroutineCalled = false;

    public void Awake() {
        switch (tag) {
            case "Tree":
                ID = (int) FarmableName.Tree;
                break;
            case "Stone":
                ID = (int) FarmableName.Stone;
                break;
            case "OreStone":
                ID = (int) FarmableName.OreStone;
                break;
        }
    }

    public void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Hitbox") && !coroutineCalled) {
            StartCoroutine(damage());
        }
    }

    private void DropResource() {
        Transform tempTransform = transform;
        GameObject resource2 = Instantiate(resource.gameObject, tempTransform.position, tempTransform.rotation);
        resource2.GetComponent<Resource>().Amount = resourceAmount;
    }

    IEnumerator damage() {
        coroutineCalled = true;
        _spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.material.color = Color.white;
        if (--health == 0) {
            DropResource();
            Destroy(gameObject);
        }
        coroutineCalled = false;
    }

    public int ID {
        get => _id;
        set
        {
            if (_id == -1) {
                _id = value;
            }
        }
    }
}
