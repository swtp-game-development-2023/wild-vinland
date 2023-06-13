using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class NewGameMenu : MonoBehaviour
    {
        public void StartNewGame()
        {
            UIWorldGenDropBox.IsFilled = true;
            UIWorldGenDropBox.GenOnStart = true;
            var seed = GetComponentInChildren<TMP_InputField>().text;
            UIWorldGenDropBox.UseSeed = !seed.Equals("");
            UIWorldGenDropBox.Seed =  UIWorldGenDropBox.UseSeed ? Int32.Parse(seed) : 0;
            
            //Left slider
            UIWorldGenDropBox.EdgeLength = (int) FindSliderByTag("SliderEdgeLength").value;
            UIWorldGenDropBox.PercentOfWood = FindSliderByTag("SliderTrees").value;
            UIWorldGenDropBox.PercentOfStone = FindSliderByTag("SliderStone").value;
            UIWorldGenDropBox.PercentOfOre =  FindSliderByTag("SliderOre").value;
            
            //Right slider
            UIWorldGenDropBox.PercentOfMountain = FindSliderByTag("SliderMountain").value;
            UIWorldGenDropBox.PercentOfLand = FindSliderByTag("SliderLand").value;;
            UIWorldGenDropBox.SmoothnessOfCoast = FindSliderByTag("SliderCoast").value;;
            UIWorldGenDropBox.PercentOfFlowers = FindSliderByTag("SliderFlowers").value;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
 
        }
        
        private Slider FindSliderByTag(string tag)
        {
            Slider[] sliders = GetComponentsInChildren<Slider>();

            foreach (Slider slider in sliders)
            {
                if (slider.CompareTag(tag))
                {
                    return slider;
                }
            }

            return null;
        }
    }
    

}

