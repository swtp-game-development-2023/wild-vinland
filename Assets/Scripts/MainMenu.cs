using System;
using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("Game Exit");
        Application.Quit();
    }
    
    public void ToggleSound()
    {
        Debug.Log("pre " + isSoundOn);

        isSoundOn = !isSoundOn;
        Debug.Log("toggel " + isSoundOn);
        AudioListener.pause = !isSoundOn; 
    }
}
