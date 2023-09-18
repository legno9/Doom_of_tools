using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial_Texts : MonoBehaviour
{
    public TextMeshProUGUI text_component;
    private int index;
    List<string> text_to_show = new List<string>{
    "a","b","c"
    
    }; 


    private void Start() {
        
        text_component.text = text_to_show[0];
        index = 0;
    }


    public void NextText() {
        
        index += 1;

        if (index == text_to_show.Count){

            Destroy(this.gameObject);
            return;
        }
        text_component.text = text_to_show[index];
        Audio_Manager.instance.Play ("Notification");
        
    }

    public void End() {
        
        SceneManager.LoadScene("Game");
        Audio_Manager.instance.Play ("Fight");
    }
}
