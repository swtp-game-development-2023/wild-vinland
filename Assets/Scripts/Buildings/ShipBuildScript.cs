using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShipBuildScript : BuildScript
{
    private Tilemap seaMap;

    protected new void OnEnable()
    {
        base.OnEnable();
        seaMap = Grid.transform.Find("Sea").gameObject.GetComponent<Tilemap>();
    }

    protected override bool ProfBuildSpot(Vector3 v)
    {
        var gridPos = Grid.WorldToCell(v);
        return base.ProfBuildSpot(v) && seaMap.GetTile(gridPos);    //TODO && dockBuilding is above
    }
}
