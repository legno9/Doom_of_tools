using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Character_Script : MonoBehaviour
{

    [HideInInspector] public Tile_Overlay active_tile; ////////////////////////
    [System.NonSerialized] public bool moving;
    [System.NonSerialized] public bool attacking;
    [System.NonSerialized] public int slash_attack_range = 1;
    [System.NonSerialized] public int slash_attack_damage = 10;
    public int health;
    private Animator animator;
    public int movement;
    public Health_System health_system;
    public Health_Bar health_bar;
    
    private void Start() {
        
        animator = GetComponentInChildren<Animator>();
        health_system = new Health_System(health, this);
        
    }

    private void Update() {
        
        if (moving){
            
            animator.SetBool("Walking",true);

        }else{

            animator.SetBool("Walking",false);
        }

    }

    public void PositionCharacterOnTile (Tile_Overlay tile){
        
        transform.position = tile.transform.position;
        active_tile = tile;
    }


    public void SlashAttack (){

        animator.SetTrigger("Slash");

    }
}

    
