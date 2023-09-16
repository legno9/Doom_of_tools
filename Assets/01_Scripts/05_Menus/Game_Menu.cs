using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Menu : MonoBehaviour
{ 
    public Button_Anims end_turn_button;
    public Button_Anims undo_move_buton;  
    public GameObject options;    

    private void Start() {

        Application.targetFrameRate = 60;
    } 
    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter)){
            if (end_turn_button.active == true){
                EndTurn();
            }         
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Return)){
            if (undo_move_buton.active == true){
                UndoMove();
            }         
        }
    }

    public void EndTurn(){

        end_turn_button.ButtonActive(false);
        StartCoroutine(Turns_Manager.Instance.EnemyTurn());
        Cards_Manager.Instance.DiscardHand();

        if (Cards_Manager.Instance.card_being_clicked != null){
            Cards_Manager.Instance.card_being_clicked.MoveToDiscard();
            Mouse_Manager.Instance.ClearAndHide();
        }
    }

    public void UndoMove(){

        Mouse_Manager.Instance.character_moved.UndoMovevement();
        undo_move_buton.ButtonActive(false);

    }

    public void OpenOptions(){

        options.SetActive(true);
    }

    public void QuitOptions(){

        options.SetActive(false);
    }

    public void BackToStart(){

        SceneManager.LoadScene("Start_Menu");
    }
}
