using UnityEngine;
using UnityEngine.InputSystem;
public class BuildMenu : MonoBehaviour
{
    private bool isOpen;

    public GameObject buildMenu;
    private InputManager input;

    private void Awake()
    {
        input = new InputManager();

    }
    
    public void ToggleBuildMenu()
    {
        isOpen = !isOpen;
        buildMenu.SetActive(isOpen);
    }
    
}
