using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Menu : MonoBehaviour
{
    public GameObject end_turn_object;   
    private Button end_turn_button;   

    private void Start() {
        
        end_turn_button = end_turn_object.GetComponent<Button>();
        
    } 
    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)){
            if (end_turn_button.interactable == true){
                EndTurn();
            }         
        }
    }

    public void EndTurn(){

        end_turn_button.interactable = false;
        StartCoroutine(Turns_Manager.Instance.EnemyTurn());
        Cards_Manager.Instance.DiscardHand();
        

    }
}
