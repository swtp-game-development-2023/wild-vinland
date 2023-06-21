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
        private bool isColliding;

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
            }
            else if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                Destroy(objBuilding);
                enabled = false;
            }
        }

        protected virtual bool ProfBuildSpot(Vector3 v)
        {
            /*var boxCollider2D = objBuilding.GetComponent<BoxCollider2D>();
            var polygonCollider2D = objBuilding.GetComponent<PolygonCollider2D>();
            Collider2D collider2 = Physics2D.OverlapBox(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f);
            if (collider2.gameObject == gameObject) collider2 = null;
            /*Collider2D[] results = new Collider2D[] { };
            var size = Physics2D.OverlapBoxNonAlloc(collider.bounds.center, collider.bounds.size, 0f, results);
            #1#
            Debug.Log(collider2 ? collider2.gameObject.name : "None");
            return collider2 != null;*/
            /*BoxCollider2D boxCollider = objBuilding.GetComponent<BoxCollider2D>();
            Debug.Log("b " + boxCollider.gameObject.name);
            Debug.Log("b " + boxCollider.gameObject.transform.position);
            Collider2D[] colliders = Physics2D.OverlapBoxAll( boxCollider.offset, boxCollider.size, 0f , LayerMask.GetMask("Water"));
            // Überprüfe, ob der Collider kollidiert
            foreach (Collider2D collider in colliders)
            {
                Debug.Log(collider.gameObject.name);
                Debug.Log(collider.gameObject.transform.position);
                    //return false; // Kollision gefunden
            }
            */


            return true; // Keine Kollision gefunden
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