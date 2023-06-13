using System;
using System.Collections;
using System.Collections.Generic;
using SceneDropBoxes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameBtn : MonoBehaviour
{
    public string SaveGameName { get; set; }

    public void LoadSaveGame()
    {
        Debug.Log(SaveGameName);
        UIWorldGenDropBox.GenOnStart = false;
        UIWorldGenDropBox.IsFilled = true;
        UILoadGameDropBox.SaveGameName = SaveGameName;
        UILoadGameDropBox.IsFilled = true;
        if (SceneManager.GetActiveScene().buildIndex < 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}