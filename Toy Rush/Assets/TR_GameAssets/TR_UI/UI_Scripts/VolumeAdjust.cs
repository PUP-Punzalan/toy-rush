using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeAdjust : MonoBehaviour
{
    // Serialize creates variable private but still shows at the editor
    [SerializeField] Slider volumeSlider;
    void Start()
    {
        // If there is no saved data from the previous game...
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            // ...The volume will default at 100%...
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        //... else, the "Load()" function will load
        else 
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        // Volume of the game = Volume setted on the slider
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    public void Load()
    {
        // Set the value of the volume slider
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    public void Save()
    {
        // Stores the value of the volume slider
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
