using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Menu : MonoBehaviour
{ 
    public Button_Anims end_turn_button;
    public Button_Anims undo_move_buton;
    public Button_Anims house_buton;
    public Button_Anims options_button;      
    public GameObject options;   
    private bool undo_button_state = false; 

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

        end_turn_button.SetButtonActive(false);
        StartCoroutine(Turns_Manager.Instance.EnemyTurn());
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

    public void OpenOptions(){

        options.SetActive(true);
        Audio_Manager.instance.Pause("Theme");
        Time.timeScale = 0;
        house_buton.SetButtonActive(false);
        options_button.SetButtonActive(false);

        if (undo_move_buton.active == true){
            undo_move_buton.SetButtonActive(false);
            undo_button_state = true;
        }
    }

    public void QuitOptions(){

        options.SetActive(false);
        Audio_Manager.instance.UnPause("Theme");
        Time.timeScale = 1;
        house_buton.SetButtonActive(true);
        options_button.SetButtonActive(true);

        if (undo_button_state == true){
            undo_move_buton.SetButtonActive(true);
        }
    }

    public void BackToStart(){

        SceneManager.LoadScene("Start_Menu");
    }
}
