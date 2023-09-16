using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class Start_Menu : MonoBehaviour
{

    private float mouse_pos_x;
    private float mouse_pos_y;
    public int move_quantity = 5;
    public GameObject background_back;
    public GameObject background_mid;
    public GameObject background_near;
    public GameObject main;
    public GameObject options;
    public Material blur;
    public AudioMixer audio_mixer;
    private List<Resolution> resolutions = new();
    public TMP_Dropdown resolution_dropdown;
    public Toggle fullscreen_toogle;
    public Slider volume_slider;
    public AudioListener listener;
    int resolution_index;

    private void Start() {
        
        Application.targetFrameRate = 60;
        GetResolutionsOptions();

        if (PlayerPrefs.GetInt ("SavedPreferences") == 1){
        
        LoadPreferences();
        }else{

            resolution_dropdown.value = resolution_index;
            PlayerPrefs.SetFloat("VolumeValue", 50);
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

            if ((float) System.Math.Round ((float)avaialable_resolutions[i].width/avaialable_resolutions[i].height, 2) == 1.78f ){ //16:9 aspect ratio

                string option = avaialable_resolutions[i].width + " x " + avaialable_resolutions[i].height + "  " + avaialable_resolutions[i].refreshRate + "hz";
                options.Add(option);

                resolutions.Add (avaialable_resolutions[i]);

                if (avaialable_resolutions[i].width == Screen.currentResolution.width && avaialable_resolutions[i].height == Screen.currentResolution.height){

                    resolution_index = resolutions.Count - 1;
                }
            }
            
        }

        resolution_dropdown.AddOptions(options);
    }

    private void Update() {
        
        mouse_pos_x = Input.mousePosition.x;
        mouse_pos_y = Input.mousePosition.y;

        background_mid.GetComponent<RectTransform>().position = Parallax(move_quantity);
        background_near.GetComponent<RectTransform>().position = Parallax(move_quantity*4f);
    }

    private Vector2 Parallax(float movement){

        return new Vector2( (mouse_pos_x/Screen.width) * movement + (Screen.width/2), (mouse_pos_y/Screen.height) * movement + (Screen.height /2));

    }

    public void LoadGame(){

        SceneManager.LoadScene("Game");

    }

    public void QuitGame(){

        Application.Quit();

    }

    public void OpenOptions(){

        main.SetActive(false);
        options.SetActive(true);
        background_back.GetComponent<Image>().material = blur;
        background_mid.GetComponent<Image>().material = blur;
        background_near.GetComponent<Image>().material = blur;
    }

    public void BackToMain(){

        main.SetActive(true);
        options.SetActive(false);
        background_back.GetComponent<Image>().material = null;
        background_mid.GetComponent<Image>().material = null;
        background_near.GetComponent<Image>().material = null;

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
