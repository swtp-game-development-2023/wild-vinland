using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCells : MonoBehaviour
{
    public GameObject CellPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject obj = Instantiate(CellPrefab);
            obj.transform.SetParent(this.gameObject.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
