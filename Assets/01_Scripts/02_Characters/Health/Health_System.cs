
using UnityEngine;
using System;

public class Health_System
{
    private Characters_Basic character;
    public float health;
    private float health_max;
    public Health_System (float health_max, Characters_Basic character){ //Constructor
        
        this.health_max = health_max;
        health = health_max;
        this.character = character; 
    }

    public float GetHealth(){
        return health;
    }

    public void Damage (float damage_amount){
        
        health -= damage_amount;
        character.health_bar.UpdateBar();
        
        if (health < 0){

            health = 0;
        }
    }

    public void Heal (float heal_amount){
        
        health += heal_amount;

        if (health > health_max){

            health = health_max;
        }else{

            character.health_bar.UpdateBar();
        } 
    }
}
