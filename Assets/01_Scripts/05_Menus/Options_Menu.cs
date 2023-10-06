using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Options_Menu : MonoBehaviour
{
    public AudioMixer audio_mixer;
    private List<Resolution> resolutions = new();
    public TMP_Dropdown resolution_dropdown;
    public Toggle fullscreen_toogle;
    public Slider volume_slider;
    int resolution_index;

    private void Start() {
        
        Application.targetFrameRate = 60;

        GetResolutionsOptions();

        if (PlayerPrefs.GetInt ("SavedPreferences") == 1){
        
            LoadPreferences();
        }else{

            resolution_dropdown.value = resolution_index;
            PlayerPrefs.SetFloat("VolumeValue", 100);
        }
        resolution_dropdown.RefreshShownValue();
    }

    public void LoadPreferences (){

        volume_slider.value = PlayerPrefs.GetFloat("VolumeValue");

        resolution_dropdown.value = PlayerPrefs.GetInt ("ResolutionValue");
        
        fullscreen_toogle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullScreen"));;
    }

    public void GetResolutionsOptions(){

        Resolution[] avaialable_resolutions = Screen.resolutions;
        resolution_dropdown.ClearOptions();

        resolution_index = 0;

        List<string> options = new List<string>();

        for (int i = 0; i < avaialable_resolutions.Length; i++)
        {   
            float resolution_width = avaialable_resolutions[i].width;
            float resolution_height = avaialable_resolutions[i].height;
            // float resolution_hz = avaialable_resolutions[i].refreshRate;

            if ((float) System.Math.Round (resolution_width/resolution_height, 2) == 1.78f && !options.Contains(resolution_width + " x " + resolution_height + "  ")){ //16:9 aspect ratio

                string option = resolution_width + " x " + resolution_height + "  ";
                options.Add(option);

                resolutions.Add (avaialable_resolutions[i]);

                if (resolution_width == Screen.currentResolution.width && resolution_height == Screen.currentResolution.height){

                    resolution_index = resolutions.Count - 1;
                }
            }
            
        }

        resolution_dropdown.AddOptions(options);

    }

    public void SetVolume (float volume){

        if (volume < 1){
            volume = 0.001f;
        }
    
        audio_mixer.SetFloat("Volume", Mathf.Log10(volume/100)*20f);
        PlayerPrefs.SetFloat("VolumeValue", volume);
        PlayerPrefs.SetInt ("SavedPreferences", 1); //Bool  
          
    }

    public void SetFullScreen (bool full){

        Screen.fullScreen = full;
        PlayerPrefs.SetInt("FullScreen", System.Convert.ToInt32(full));
        PlayerPrefs.SetInt ("SavedPreferences", 1); //Bool    
    }

    public void SetResolution(int resolution_index_){
        
        Resolution resolution = resolutions[resolution_index_];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt ("ResolutionValue", resolution_index_);
        PlayerPrefs.SetInt ("SavedPreferences", 1); //Bool

    }
    


    public void PlaySoundScroller(){

        Audio_Manager.instance.Play("Scroll");
    }

    public void PlaySoundClick(){

        Audio_Manager.instance.Play("Click");
    }
    
    public void PlaySoundHover(){

        Audio_Manager.instance.Play("Hover");
    }
}
