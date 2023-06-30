using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BuildMenu : MonoBehaviour
{
    public bool isOpen;
    [SerializeField] private bool isDebug;
    public GameObject buildMenu;
    private List<BuildScript> buildScripts = new();
    private Inventory inventory;

    private readonly List<CollectableName> collectableNames =
        new List<CollectableName>((CollectableName[])Enum.GetValues(typeof(CollectableName)));

    private void Start()
    {
        isOpen = false;
        buildMenu.SetActive(false);
    }

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        foreach (var o in GameObject.FindGameObjectsWithTag("BuildBtn"))
        {
            var script = o.GetComponent<BuildScript>();
            if (script) buildScripts.Add(script);
        }


    }

    private void Update()
    {
        buildScripts.ForEach(b =>
        {
            b.gameObject.GetComponent<Button>().interactable =
                    collectableNames.All(cN => b.RequiredResources[cN] <= inventory.GetTotalAmount(cN));
        });
        
        if (isDebug)
        {
            buildScripts.ForEach(b =>
            {
                b.gameObject.GetComponent<Button>().interactable = true;
                collectableNames.ForEach(cN => b.RequiredResources[cN] = 0);
            });
        }
    }

    private void OnEnable()
    { 
        
    }

    public void ToggleBuildMenu()
    {
        isOpen = !isOpen;
        buildMenu.SetActive(isOpen);
    }
    
    public void CloseBuildMenu()
    {
        isOpen = false;
        buildMenu.SetActive(false);
    }
}