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

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        buildMenu.SetActive(isOpen);
    }
    
    public void ToggleBuildMenu()
    {
        isOpen = !isOpen;
        buildMenu.SetActive(isOpen);
    }
    
}
