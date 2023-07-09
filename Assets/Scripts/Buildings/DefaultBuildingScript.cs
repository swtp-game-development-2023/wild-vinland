using UnityEngine;
using UnityEngine.Tilemaps;

namespace Buildings
{
    public class DefaultBuildingScript : BuildScript
    {
        //TODO
        private Tilemap seaMap;

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            seaMap = Grid.transform.Find("Sea").gameObject.GetComponent<Tilemap>();
        }

        protected override bool ProfBuildSpot(Vector3 v)
        {
            var gridPos = Grid.WorldToCell(v);
            return base.ProfBuildSpot(v) && !seaMap.GetTile(gridPos);
        }
    }
}