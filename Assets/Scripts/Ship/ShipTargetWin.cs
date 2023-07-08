using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTargetWin : MonoBehaviour
{
    [SerializeField]
    private GameObject endScreen;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Ship"))
        {
            endScreen.SetActive(true);
        }
    }
}
