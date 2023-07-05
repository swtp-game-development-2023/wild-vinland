using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool isSoundOn = true;
    
    public void ClickedBtnExit()
    {
        Debug.Log("Game Exit"); //We should keep this because in development there is no other way to detect exit.
        Application.Quit();
    }
    
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        if (isSoundOn)
        { 
            AudioListener.volume = 0.5f; 
        }
        else
        { 
            AudioListener.volume = 0; 
        }
    }
}
