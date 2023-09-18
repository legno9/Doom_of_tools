using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Game_Menu : MonoBehaviour
{ 
    public Button_Anims end_turn_button;
    public Button_Anims undo_move_buton;
    public Button_Anims house_buton;
    public Button_Anims options_button;      
    public GameObject options;   
    public GameObject confirmation;   
    public GameObject end_game_object;   
    public TextMeshProUGUI eng_game_text;   
    private bool undo_button_state = false; 

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter)){
            if (end_turn_button.active == true){
                EndTurn();
            }         
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Backspace)){
            if (undo_move_buton.active == true){
                UndoMove();
            }         
        }
    }

    public void EndTurn(){

        end_turn_button.SetButtonActive(false);
        StartCoroutine(Turns_Manager.Instance.EnemyTurn( ));
        Cards_Manager.Instance.DiscardHand();

        if (Cards_Manager.Instance.card_being_clicked != null){
            Cards_Manager.Instance.card_being_clicked.MoveToDiscard();
            Mouse_Manager.Instance.ClearAndHide();
        }
    }

    public void UndoMove(){

        Mouse_Manager.Instance.character_moved.UndoMovevement();
        undo_move_buton.SetButtonActive(false);

    }

    public void StopGame( bool stop){

        if (stop){
            
            // Audio_Manager.instance.Pause("Theme");
            Time.timeScale = 0;

            if (undo_move_buton.active == true){
                undo_move_buton.SetButtonActive(stop);
                undo_button_state = true;
            }

        }else{

            // Audio_Manager.instance.UnPause("Theme");
            Time.timeScale = 1;

            if (undo_button_state == true){
                undo_move_buton.SetButtonActive(true);
            }
        }
        
        house_buton.SetButtonActive(!stop);
        options_button.SetButtonActive(!stop);
    }

    public void OpenOptions(){

        options.SetActive(true);
        StopGame( true);
    }

    public void CloseOptions(){

        options.SetActive(false);
        StopGame(false);
    }

    public void OpenConfirmation(){

        confirmation.SetActive(true);
        StopGame(true);
    }

    public void CloseConfirmation(){

        confirmation.SetActive(false);
        StopGame(false);
    }

    public void BackToStart(){

        if (PlayerPrefs.GetInt ("TutorialPlayed") == 0){

            PlayerPrefs.SetInt ("TutorialPlayed", 1);
            StopGame(false);
            SceneManager.LoadScene("Game");
            Audio_Manager.instance.Stop("Theme");
            Audio_Manager.instance.Play("Fight");
            

        }else{

            StopGame(false);
            SceneManager.LoadScene("Start_Menu");
            Audio_Manager.instance.Stop("Fight");
            Audio_Manager.instance.Play("Theme");
        }
        
    }

    public IEnumerator EndGame(bool victory){
        
        yield return new WaitForSeconds(2f);
        StopGame(true);
        Audio_Manager.instance.Stop("Fight");
        Audio_Manager.instance.Stop("Chill");

        if (victory){
            eng_game_text.text = "Victory";
            Audio_Manager.instance.Play("Victory");
        }else{
            eng_game_text.text = "Loose";
            Audio_Manager.instance.Play("Loose");
        }
        
        end_game_object.SetActive(true);
        
    }

    public void RestartGame(){

        if (PlayerPrefs.GetInt ("TutorialPlayed") == 0){

            PlayerPrefs.SetInt ("TutorialPlayed", 0);
            StopGame(false);
            SceneManager.LoadScene("Tutorial");
            Audio_Manager.instance.Play ("Chill");
            
        }else{

            PlayerPrefs.SetInt ("TutorialPlayed", 1);
            StopGame(false);
            SceneManager.LoadScene("Game");
            Audio_Manager.instance.Play ("Fight");
        }
        
    }
}
