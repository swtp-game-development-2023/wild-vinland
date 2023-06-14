using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Farmable : MonoBehaviour {
    [SerializeField] private int health = 1;
    [SerializeField] private int resourceAmount = 1;
    private int _id = -1;
}
