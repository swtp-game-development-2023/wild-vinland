using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DockBuildScript : BuildScript
{
    private Tilemap beachMap;
    private Tilemap seaMap;
    protected new void OnEnable()
    {
        base.OnEnable();
        beachMap = Grid.transform.Find("Beach").gameObject.GetComponent<Tilemap>();
        seaMap = Grid.transform.Find("Sea").gameObject.GetComponent<Tilemap>();
    }

    protected new void OnDisable()
    {
        base.OnDisable();
    }
    
    protected override bool ProfBuildSpot(Vector3 v)
    {
        var gridPos = Grid.WorldToCell(v);
        var oneBelow = new Vector3Int(gridPos.x , gridPos.y -1, gridPos.z);
        var towBelow = new Vector3Int(gridPos.x , gridPos.y -2, gridPos.z);
        return base.ProfBuildSpot(v) /*&& beachMap.GetTile(gridPos) && seaMap.GetTile(oneBelow) && seaMap.GetTile(towBelow)*/;
    }
}
