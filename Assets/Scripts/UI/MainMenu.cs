using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool isSoundOn = false;
    
    public void Start()
    {
        //TODO remove in final version
        AudioListener.pause = !isSoundOn;
    }

    public void ClickedBtnNewGame()
    {
        //TODO
    }
    public void ClickedBtnLoadGame()
    {
        //TODO
    }
    
    public void ClickedBtnCredits()
    {
        //TODO
    }
    public void ClickedBtnExit()
    {
        Debug.Log("Game Exit"); //We should keep this because in development there is no other way to detect exit.
        Application.Quit();
    }
    
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        AudioListener.pause = !isSoundOn; 
    }
}
