using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseMenu;
    public bool isPaused;
    private InputManager input;
    
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }
    
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void SaveGame()
    {
        //TODO open save Function
        print("TODO save Game");
    }
    
    public void LoadGame()
    {
        //TODO open load Function
        print("TODO load Game");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Debug.Log("Game Exit"); //We should keep this because in development there is no other way to detect exit.
        Application.Quit();
    }
}
