using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class LoadMenuScrollList : MonoBehaviour
{
    public GameObject cellPrefab;

    private FileInfo[] _files;

    private DirectoryInfo _directory;

    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo _directory = new DirectoryInfo(Application.dataPath + "/Saves");

        _files = Array.Empty<FileInfo>();
        if (_directory.Exists)
        {
            _files = _directory.GetFiles("*.json");
            foreach (var file in _files)
            {
                var obj = Instantiate(cellPrefab, gameObject.transform, false);
                obj.transform.GetChild(0).GetComponent<TMP_Text>().text = file.Name;
                obj.transform.GetChild(1).GetComponent<TMP_Text>().text = file.LastWriteTime.ToShortTimeString() + " " + file.LastWriteTime.ToShortDateString();
                obj.transform.GetComponent<LoadGameBtn>().SaveGameName = file.Name;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}