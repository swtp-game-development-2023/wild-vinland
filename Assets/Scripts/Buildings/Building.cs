using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        private InputManager input;
        private GameObject objBuilding;
        private bool canDisable;

        // Start is called before the first frame update
        void Start()
        {
        }

        private void OnEnable()
        {
            objBuilding = Instantiate(prefab, CalcGridPos(), Quaternion.identity);

        }

        void LateUpdate()
        {
            objBuilding.transform.position = CalcGridPos();

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                enabled = false;
                canDisable = false;
            }
        }
        private static Vector3 CalcGridPos()
        {
            Grid grid = FindObjectOfType<Grid>();
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.transform.position.z * -1;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            var x = Mathf.Round((worldPos.x - grid.cellSize.x / 2f) / grid.cellSize.x) * grid.cellSize.x +
                    grid.cellSize.x / 2f;
            var y = Mathf.Round((worldPos.y - grid.cellSize.y / 2f) / grid.cellSize.y) * grid.cellSize.y +
                    grid.cellSize.y / 2f;
            return new Vector3(x, y, 0);
        }
    }
}