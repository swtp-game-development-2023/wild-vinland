using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Scripting;
using UnityEngine;

public class saveGame : MonoBehaviour
{

    public GameObject findText;
    
    // Start is called before the first frame update
    void Start()
    {
        findText = FindObjectOfType<MapSaveSystem>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSaveButtonPressed()
    {
        var saveInput = GetComponentInParent<TMP_InputField>();

        findText.transform.GetComponent<MapSaveSystem>().SaveMap(saveInput.text);

        //Debug.Log(saveInput.text);  
    }
}
