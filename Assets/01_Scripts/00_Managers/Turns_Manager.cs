using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Turns_Manager : MonoBehaviour
{
    private static Turns_Manager manager_instance;
    public static Turns_Manager Instance {get { return manager_instance; } }
    [System.NonSerialized] public bool next_enemy = false;
    List<Transform> enemies;
    public Game_Menu menu_functions;

    private void Awake() { 
        
        if (manager_instance == null){

            manager_instance = this;
        }else{

            Destroy(this);}
    }

     public IEnumerator EnemyTurn(){

        foreach (Transform ally in Mouse_Manager.Instance.ally_characters_tile.Keys){

            Mouse_Manager.Instance.GetCharacterScript((Names.character) Enum.Parse(typeof(Names.character), ally.name)).action_used = true;
        }

        menu_functions.undo_move_buton.SetButtonActive(false);
        enemies = new List<Transform>(Mouse_Manager.Instance.enemy_characters_tile.Keys);

        foreach (Transform enemy in enemies){

            Enemy_Basics controller;

            try{
                controller = (Enemy_Basics) enemy.GetComponent(typeof(Enemy_Basics)); //If the enemy dies, next enemy
                
            } 
            catch{
                continue;
            }
        
            yield return new WaitForSeconds(0.5f);

            next_enemy = false;
            StartCoroutine (controller.StartController());

            yield return new WaitUntil(() => next_enemy == true);
     
        }

        yield return new WaitForSeconds (1);
        NewTurn();
    }

    public void NewTurn(){

        StartCoroutine(Cards_Manager.Instance.DrawCards());
        
        foreach (Transform c in Mouse_Manager.Instance.ally_characters_tile.Keys){

            c.GetComponent<Characters_Basic>().action_used = false;

        }

        
    }

    
}
