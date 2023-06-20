using System;
using System.Collections.Generic;
using System.Linq;
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
        private GameObject objBuilding;
        private List<SpriteRenderer> buildingSprites;
        protected Grid Grid;
        private Camera mainCamera;
        private Tilemap decoMap;
        private bool isBuild;


        protected void OnEnable()
        {
            Grid = FindObjectOfType<Grid>();
            mainCamera = Camera.main;
            objBuilding = Instantiate(buildingPrefab, CalcGridPos(), Quaternion.identity);
            buildingSprites = objBuilding.transform.GetComponentsInChildren<SpriteRenderer>().ToList();
            decoMap = Grid.transform.Find("Deco").gameObject.GetComponent<Tilemap>();
        }

        protected void OnDisable()
        {
            if(!isBuild) Destroy(objBuilding);
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
                enabled = false;
            }
            else if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                Destroy(objBuilding);
                enabled = false;
            }
        }

        protected abstract bool ProfBuildSpot(Vector3 v);
        

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