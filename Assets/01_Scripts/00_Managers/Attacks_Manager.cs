using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks_Manager : MonoBehaviour{

    private static Attacks_Manager manager_instance;
    public static Attacks_Manager Instance {get { return manager_instance; } }
    private void Awake() { 
        
        if (manager_instance == null){

            manager_instance = this;
        }else{

            Destroy(this.gameObject);}
    }

    private Characters_Basic character_attacking;
    List<Tile_Overlay> attack_tiles = new();
    Range_Finder range_finder = new();
    bool select_all_tiles;
    int damage_amount;
    int heal_amount;

    public void Attacks_Creator ( Names.tiles_selector tiles_selector, int damage = 0, int heal = 0, int range = 0, int rows = 0, Names.direction_name direction = 0, int ignore_tiles = 0, 
    Names.status_effect status_effect = 0, int status_quantity = 0){
        
        attack_tiles.Clear();
        damage_amount = damage;
        heal_amount = heal;
        select_all_tiles = false;
        
        switch (tiles_selector){

            case Names.tiles_selector.adjacent_tile:
                attack_tiles.Add (range_finder.GetAdjacentTile(direction, character_attacking.active_tile));
                break;

            case Names.tiles_selector.adjacent_tiles:
                attack_tiles = range_finder.GetAdjacentTiles(character_attacking.active_tile, range );
                break;

            case Names.tiles_selector.vertical_tiles:
                attack_tiles = range_finder.GetVerticalTiles(ignore_tiles, range, character_attacking.active_tile);
                break;
            
            case Names.tiles_selector.horizontal_tiles:
                attack_tiles = range_finder.GetHorizontalTiles(ignore_tiles, range, character_attacking.active_tile);
                break;
            
            case Names.tiles_selector.surrounding_tiles:
                attack_tiles = range_finder.GetSurroundingTiles(ignore_tiles,range, character_attacking.active_tile);
                select_all_tiles = true;
                break;

            case Names.tiles_selector.multiple_hortizontal_tiles:
                attack_tiles = range_finder.GetMultipleHorizontal(ignore_tiles, range, rows, character_attacking.active_tile);
                break;    
        }    
    }

    public void SetAttackBasics (){

        character_attacking.select_all_tiles = select_all_tiles;  
        character_attacking.damage_amount = damage_amount * 10;
        character_attacking.heal_amount = heal_amount * 10;
    }

    public void GetAllyCharacterAttacks ( Names.character character, Names.attack_selector attack ){
        
        character_attacking = Mouse_Manager.Instance.GetCharacterTransform(character);        

        if (character == Names.character.Peeler){

            if (attack == Names.attack_selector.attack_01){ // 1º Attack

                Attacks_Creator ( Names.tiles_selector.vertical_tiles,  damage:1, range:9 );
            }
            else if (attack == Names.attack_selector.attack_02){ // 2º Attack

                Attacks_Creator ( Names.tiles_selector.vertical_tiles,  ignore_tiles: 2, damage:2, range:1 );
            }

        }

        else if (character == Names.character.Blender){

            if (attack == Names.attack_selector.attack_01){ // 1º Attack

                Attacks_Creator (  Names.tiles_selector.horizontal_tiles, damage:1 , range:3, ignore_tiles:2  );
            }
            else if (attack == Names.attack_selector.attack_02){ // 2º Attack

                Attacks_Creator (  Names.tiles_selector.surrounding_tiles, damage:1 , range:2);    
            }

        }

        foreach( var tile in attack_tiles ){
        
            tile.ShowTile(Color.red, 0.5f);
        } 
    }

    public void ClearPreview (){

        foreach( var tile in attack_tiles ){
        
            tile.ShowTile(Color.clear);
        }
    }

    public void SetAllyAttack (){

        SetAttackBasics();
        Mouse_Manager.Instance.character_clicked = character_attacking;   
        Mouse_Manager.Instance.attack_range =  attack_tiles;

        foreach( var tile in attack_tiles ){
        
            tile.ShowTile(Color.red);
        } 
    }


    public List<Tile_Overlay> GetEnemyCharacterAttacks ( Characters_Basic character, Names.attack_selector attack ){

        character_attacking = character;

        if (character.name_ == Names.character.Spatula){

            if (attack == Names.attack_selector.attack_01){

                Attacks_Creator (  Names.tiles_selector.multiple_hortizontal_tiles, damage:1 , range:3, rows:2, ignore_tiles:3);
                return attack_tiles;
            }
        }

        else if (character.name_ == Names.character.Mill){

            if (attack == Names.attack_selector.attack_01){

                Attacks_Creator (  Names.tiles_selector.surrounding_tiles, damage:1 , range:2);
                return attack_tiles;
            }
        }

        return attack_tiles;
    }
}





