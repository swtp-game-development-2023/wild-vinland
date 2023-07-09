using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseMenu;
    public GameObject loadMenu;
    public GameObject saveMenu;
    public bool isPaused;
    public InputManager input;
    
    void Start()
    {
        input = GameObject.FindGameObjectWithTag("World").GetComponent<MapSaveSystem>().Input;
        pauseMenu.SetActive(false);
        loadMenu.SetActive(false);
        saveMenu.SetActive(false);
        isPaused = false;
    }
    
    public void PauseGame()
    {
        loadMenu.SetActive(false);
        pauseMenu.SetActive(true);
        saveMenu.SetActive(false);
        Time.timeScale = 0f;
        input.UI.save.Disable();
        input.UI.load.Disable();
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        loadMenu.SetActive(false);
        saveMenu.SetActive(false);
        input.UI.save.Enable();
        input.UI.load.Enable();
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void OpenLoadMenu()
    {
        pauseMenu.SetActive(false);
        loadMenu.SetActive(true);
        saveMenu.SetActive(false);
    }
    
    public void OpenSaveMenu()
    {
        pauseMenu.SetActive(false);
        loadMenu.SetActive(false);
        saveMenu.SetActive(true);
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
