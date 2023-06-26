using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Buildings
{
    public class WindmillBuildScript : BuildScript
    {
        private Tilemap beachMap;
        private Tilemap grassMap;

        protected new void OnEnable()
        {
            base.OnEnable();
            beachMap = Grid.transform.Find("Beach").gameObject.GetComponent<Tilemap>();
            grassMap = Grid.transform.Find("Grass").gameObject.GetComponent<Tilemap>();
        }

        protected new void OnDisable()
        {
            //buildingPrefab.gameObject.GetComponent<AudioSource>().;
            base.OnDisable();
        }

        protected override bool ProfBuildSpot(Vector3 v)
        {
            var gridPos = Grid.WorldToCell(v);
            var oneBelow = new Vector3Int(gridPos.x, gridPos.y - 1, gridPos.z);
            return base.ProfBuildSpot(v) && (beachMap.GetTile(oneBelow) || grassMap.GetTile(oneBelow));
        }
    }
}