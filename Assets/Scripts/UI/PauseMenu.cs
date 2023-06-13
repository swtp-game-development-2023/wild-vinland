using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseMenu;
    public GameObject loadMenu;
    public bool isPaused;
    private InputManager input;
    
    void Start()
    {
        pauseMenu.SetActive(false);
        loadMenu.SetActive(false);
        isPaused = false;
    }
    
    public void PauseGame()
    {
        loadMenu.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        loadMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void OpenLoadMenu()
    {
        pauseMenu.SetActive(false);
        loadMenu.SetActive(true);
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
