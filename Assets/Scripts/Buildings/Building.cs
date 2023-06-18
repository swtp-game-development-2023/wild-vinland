using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private InputManager input;
    private bool wasBuild;

    // Start is called before the first frame update
    void Start()
    {
        input = new InputManager();
        input.Player.Fire.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !wasBuild)
        {
            Grid grid = FindObjectOfType<Grid>();
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.transform.position.z * -1;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Debug.Log(mousePos +"\t" + worldPos);
            // Runde die berechneten Weltkoordinaten auf die nächstgelegene Zelle
            var x = Mathf.Round((worldPos.x - grid.cellSize.x / 2f) / grid.cellSize.x) * grid.cellSize.x + grid.cellSize.x / 2f;
            var y = Mathf.Round((worldPos.y - grid.cellSize.y / 2f) / grid.cellSize.y) * grid.cellSize.y + grid.cellSize.y / 2f;
            var outV = new Vector3(x  , y, 0);
            Debug.Log(mousePos +"\t" + worldPos +"\t" + outV);
            var obj =Instantiate(prefab, outV, Quaternion.identity);
            //obj.transform.GetComponent<Building>().wasBuild = true;
            wasBuild = true;
        }
    }
}