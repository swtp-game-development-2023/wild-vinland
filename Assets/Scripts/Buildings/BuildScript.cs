using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Buildings
{
    public abstract class BuildScript : MonoBehaviour
    {
        [SerializeField] protected GameObject buildingPrefab;
        protected GameObject objBuilding;
        private List<SpriteRenderer> buildingSprites;
        protected Grid Grid;
        private Camera mainCamera;
        private Tilemap decoMap;
        protected bool isBuild;
        protected CollidingChecker collidingChecker;
        private readonly List<CollectableName> collectableNames = new List<CollectableName>((CollectableName[])Enum.GetValues(typeof(CollectableName)));

        [SerializeField]
        [Range(0, 100)]
        private int requiredWood;
        [SerializeField]
        [Range(0, 100)]
        private int requiredStone;
        [SerializeField]
        [Range(0, 100)]
        private int requiredOre;
        private Inventory inventory;
        private Dictionary<CollectableName, int> requiredResources;

        public Dictionary<CollectableName, int> RequiredResources => requiredResources;

        public void SetRequiredResources()
        {
            requiredResources = new Dictionary<CollectableName, int> { { CollectableName.Wood, requiredWood  },
                { CollectableName.Ore, requiredOre }, { CollectableName.Stone, requiredStone } };
        }

        private void Awake()
        {
            inventory = FindObjectOfType<Inventory>();
            SetRequiredResources();
        }

        protected void OnEnable()
        {
            Grid = FindObjectOfType<Grid>();
            mainCamera = Camera.main;
            objBuilding = Instantiate(buildingPrefab, CalcGridPos(), Quaternion.identity);
            buildingSprites = objBuilding.transform.GetComponentsInChildren<SpriteRenderer>().ToList();
            decoMap = Grid.transform.Find("Deco").gameObject.GetComponent<Tilemap>();
            collidingChecker = objBuilding.GetComponent<CollidingChecker>();
        }

        protected virtual void OnDisable()
        {
            if (!isBuild) Destroy(objBuilding);
            enabled = false;
        }

        void Update()
        {
            objBuilding.transform.position = CalcGridPos();
            var canBeBuild = ProfBuildSpot(objBuilding.transform.position);
            if (canBeBuild)
            {
                buildingSprites.ForEach(s => s.color = new Color(0, 255, 0));
            }
            else
            {
                buildingSprites.ForEach(s => s.color = new Color(255, 0, 0));
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame && canBeBuild)
            {
                var gridPos = Grid.WorldToCell(objBuilding.transform.position);
                decoMap.SetTile(gridPos, null);
                buildingSprites.ForEach(s => s.color = new Color(255, 255, 255));
                isBuild = true;
                objBuilding.GetComponent<PolygonCollider2D>().enabled = true;
                enabled = false;
                foreach (var collectableName in collectableNames.Where(c => 0 < requiredResources[c] ))
                {
                    inventory.Remove(collectableName, requiredResources[collectableName]);
                }
            }
            else if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                Destroy(objBuilding);
                enabled = false;
            }
        }

        protected virtual bool ProfBuildSpot(Vector3 v)
        {
            return !collidingChecker.IsColliding;
        }


        private Vector3 CalcGridPos()
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = mainCamera.transform.position.z * -1;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            var cellSize = Grid.cellSize;
            var x = Mathf.Round((worldPos.x - cellSize.x / 2f) / cellSize.x) * cellSize.x +
                    Grid.cellSize.x / 2f;
            var y = Mathf.Round((worldPos.y - cellSize.y / 2f) / cellSize.y) * cellSize.y +
                    Grid.cellSize.y / 2f;
            return new Vector3(x, y, 0);
        }
    }
}