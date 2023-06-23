using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using UnityEngine;
using UnityEngine.UI;


public class BuildMenu : MonoBehaviour
{
    private bool isOpen;

    public GameObject buildMenu;
    private List<BuildScript> buildScripts = new();
    private Inventory inventory;

    private readonly List<CollectableName> collectableNames =
        new List<CollectableName>((CollectableName[])Enum.GetValues(typeof(CollectableName)));

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
    }

    private void OnEnable()
    {
    }

    public void ToggleBuildMenu()
    {
        isOpen = !isOpen;
        buildMenu.SetActive(isOpen);
    }
}