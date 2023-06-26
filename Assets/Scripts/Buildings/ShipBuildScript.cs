using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ShipBuildScript : MonoBehaviour
{
    private GameObject dock;
    protected Grid Grid;
    private bool isSpawned;
    
    [SerializeField]
    private GameObject ship;

    private void Update()
    {
        gameObject.GetComponent<Button>().interactable = !isSpawned;
    }

    private void OnEnable()
    {
        dock = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        Grid = FindObjectOfType<Grid>();
    }
    
    private Vector3 gridToWorldVector(Vector3Int v)
    {
        return (Grid.GetCellCenterWorld(v)) - new Vector3(0f, 0.08f, 0); // 0.08 half the grid size (0.16)
    }

    public void BuildShip()
    {
        var gridPos = Grid.WorldToCell(dock.transform.position);
        var twoBelow = new Vector3Int(gridPos.x , gridPos.y -1, gridPos.z);

        var x = gridToWorldVector(twoBelow);
        
        ship = Instantiate(ship, x, Quaternion.identity);
        isSpawned = true;
    }
}
