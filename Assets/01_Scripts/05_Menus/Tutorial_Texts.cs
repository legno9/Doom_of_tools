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
    "Click the character and move it to other tile.\n They can only move once ",
    "It's possible to undo the movement by pressing the rewind button to the right of the settings.",
    "You can also press left shift or backspace.",
    "Important. Once you attack, the character cannot move again.",
    "If you hover your mouse over the cards you can see their attacks. \n Click one. ",
    "If you click on the red tiles, the character will attack. ",
    "Note that you have limited mana. At the top left of each card you can see how much it costs.",
    "It's possible to cancel the card with right click. You can also click on another card.",
    "If you run out of mana, you can end the turn by clicking the button in the lower right corner.",
    "At the end of the turn, the enemies' turn begins. They as well as you, can move and attack.",
    "Now defeat the enemy",
    }; 


    private void Start() {
        
        text_component.text = text_to_show[0];
        index = 0;
    }


    public void NextText() {
        
        index += 1;

        if (index == text_to_show.Count){

            index = 0;
        }
        text_component.text = text_to_show[index];
        Audio_Manager.instance.Play ("Notification");
        
    }

    public void End() {
        
        SceneManager.LoadScene("Game");
        Audio_Manager.instance.Play ("Fight");
        Audio_Manager.instance.Stop ("Chill");
        PlayerPrefs.SetInt ("TutorialPlayed", 1);
    }
}
