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
            UI_WorldGen_DropBox.IsFilled = true;
            var seed = GetComponentInChildren<TMP_InputField>().text;
            UI_WorldGen_DropBox.UseSeed = !seed.Equals("");
            UI_WorldGen_DropBox.Seed =  UI_WorldGen_DropBox.UseSeed ? Int32.Parse(seed) : 0;
            Debug.Log(GetComponentInChildren<TMP_InputField>().text);
            
            //Left slider
            UI_WorldGen_DropBox.EdgeLength = (int) FindSliderByTag("SliderEdgeLength").value;
            UI_WorldGen_DropBox.PercentOfWood = FindSliderByTag("SliderTrees").value;
            UI_WorldGen_DropBox.PercentOfStone = FindSliderByTag("SliderStone").value;
            UI_WorldGen_DropBox.PercentOfOre =  FindSliderByTag("SliderOre").value;
            
            //Right slider
            UI_WorldGen_DropBox.PercentOfMountain = FindSliderByTag("SliderMountain").value;
            UI_WorldGen_DropBox.PercentOfLand = FindSliderByTag("SliderLand").value;;
            UI_WorldGen_DropBox.SmoothnessOfCoast = FindSliderByTag("SliderCoast").value;;
            UI_WorldGen_DropBox.PercentOfFlowers = FindSliderByTag("SliderFlowers").value;
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

