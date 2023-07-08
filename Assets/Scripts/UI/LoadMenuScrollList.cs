using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class LoadMenuScrollList : MonoBehaviour
{
    public GameObject cellPrefab;

    private FileInfo[] _files;

    private DirectoryInfo _directory;

    // Start is called before the first frame update
    void Start()
    {
        RefreshFileList();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RefreshFileList()
    {
        _directory = new DirectoryInfo(Application.persistentDataPath + "/Saves");
        
        if (_directory.Exists)
        {
            _files = _directory.GetFiles("*.json").OrderBy(p => p.LastWriteTime).Reverse().ToArray();
            foreach (var file in _files)
            {
                var obj = Instantiate(cellPrefab, gameObject.transform, false);
                obj.transform.GetChild(0).GetComponent<TMP_Text>().text = file.Name;
                obj.transform.GetChild(1).GetComponent<TMP_Text>().text = file.LastWriteTime.ToShortTimeString() + " " + file.LastWriteTime.ToShortDateString();
                obj.transform.GetComponent<LoadGameBtn>().SaveGameName = file.Name;
            }
        }
    }
}