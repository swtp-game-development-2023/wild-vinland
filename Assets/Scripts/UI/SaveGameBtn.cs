using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveGameBtn : MonoBehaviour
{

    private GameObject worldGrid;

    private TMP_InputField saveNameInput;
    // Start is called before the first frame update
    void Start()
    {
        worldGrid = FindObjectOfType<MapSaveSystem>().gameObject;
        saveNameInput = GetComponentInParent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Button>().interactable = !(string.IsNullOrWhiteSpace(saveNameInput.text));
    }

    public void OnSaveButtonPressed()
    {
        worldGrid.transform.GetComponent<MapSaveSystem>().SaveMap(saveNameInput.text);
    }
}
