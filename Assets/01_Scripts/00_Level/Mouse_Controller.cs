using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Character_Arrow;

public class Mouse_Controller : MonoBehaviour
{
    private Path_Finder path_finder;
    private Range_Finder range_finder;
    private Character_Arrow character_arrow;
    private List<Tile_Overlay> path = new List<Tile_Overlay>();
    private List<Tile_Overlay> movement_range = new List<Tile_Overlay>();
    private List<Tile_Overlay> attack_range = new List<Tile_Overlay>();
    private List<Transform> characters = new List<Transform>(); // Store all the controllable characters in game
    private Dictionary<Transform ,Vector3> characters_tile = new Dictionary<Transform ,Vector3>();
    private Character_Script character_clicked;
    private bool attack_in_process = false;

    private void Start(){

        path_finder = new Path_Finder();
        range_finder = new Range_Finder();
        character_arrow = new Character_Arrow();
        characters = GameObject.FindGameObjectsWithTag("Character").Select(go => go.transform).ToList();

    }

    private void LateUpdate(){

        SetCharacterstile(); 

        TilesSelector();

        if (character_clicked!= null){
            if (character_clicked.moving){

                MoveAlongPath();
            }
        }
    }

    private void SetCharacterstile(){   

        foreach (var character in characters ){
            
            if (character.GetComponent<Character_Script>().active_tile == null){
            
                RaycastHit2D start_hit = Physics2D.Raycast(new Vector2 (character.position.x,character.position.y), Vector2.zero);
                Tile_Overlay start_tile = start_hit.transform.GetComponentInParent<Tile_Overlay>();
                character.GetComponent<Character_Script>().PositionCharacterOnTile(start_tile);
            }
            
            Vector3 character_tile_pos = character.GetComponent<Character_Script>().active_tile.transform.position;
            
            if (!characters_tile.ContainsValue(character_tile_pos)){

                 if (characters_tile.ContainsKey(character)){

                    characters_tile.Remove(character);  //Remove the last position before adding the new one
                 }

                 characters_tile.Add(character,character_tile_pos);
             }
            
        }
    }

    private void TilesSelector(){

        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouse_pos_2D = new Vector2 ( mouse_pos.x, mouse_pos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll (mouse_pos_2D, Vector2.zero);  // Detects all colliders and return them in an array from first to last

        if (hits.Length > 0){
            
            RaycastHit2D focus_tile =  hits.OrderByDescending(i => i.collider.transform.position.z).First();
            
            Tile_Overlay overlay_tile = focus_tile.collider.gameObject.GetComponentInParent<Tile_Overlay>(); //Get the V_Overlay_Tilemap collided
            transform.position = overlay_tile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlay_tile.GetComponentInChildren<SpriteRenderer>().sortingOrder; // Assign the same sorting order

            if ( characters_tile.ContainsValue(transform.position) && Input.GetMouseButtonDown(0) && !attack_in_process){ // Set the character clicked
                
                for (int c = 0; c< characters.Count; c++ ){
                    
                    if (characters[c].transform.position == transform.position){

                        character_clicked = characters[c].GetComponent<Character_Script>();
                    }
                }
                
                movement_range = range_finder.GetAdjacentTiles(character_clicked.active_tile, character_clicked.movement); 


                foreach( var tile in movement_range ){
            
                    tile.ShowTile(Color.blue);
                } 
            }
            
            if (movement_range.Contains(overlay_tile) && !character_clicked.moving && character_clicked != null){
                
                DisplayArrowPath(overlay_tile, movement_range); //Display all the arrow to the tile hovered
            }

            if (character_clicked != null && Input.GetKeyDown(KeyCode.Alpha1)){
                
                CancelAction();
                attack_range = range_finder.GetAdjacentTiles(character_clicked.active_tile, character_clicked.slash_attack_range);
                character_clicked.attacking = true;
                attack_in_process = true;
                
                foreach( var tile in attack_range ){
            
                    tile.ShowTile(Color.red);
                }
            }

            if (attack_range.Contains(overlay_tile) && !character_clicked.moving){
                
                DisplayArrowPath(overlay_tile, attack_range);
            } 

            if ((Input.GetMouseButtonDown(1) && character_clicked != null) || (Input.GetMouseButtonDown(0) && !movement_range.Contains(overlay_tile) && character_clicked != null)){

                CancelAction();
                character_clicked = null;
            }
               
        }
        
    }

    private void DisplayArrowPath(Tile_Overlay end_tile, List<Tile_Overlay> tiles_affected ){
        
        path = path_finder.FindPath(character_clicked.active_tile, end_tile, tiles_affected);

        foreach ( var tile in tiles_affected){ //Remove unused arrows

            tile.SetArrowSprite (arrow_direction.None);
        }

        for (int i = 0; i< path.Count; i++ ){ // Show arrows

            var previous_tile = i > 0? path[i-1] : character_clicked.active_tile;
            var future_tile = i < path.Count -1 ? path[i+1]: null;

            var arrow_dir = character_arrow.TanslateDirection(previous_tile, path[i], future_tile);
            path[i].SetArrowSprite(arrow_dir);
        }
    
        if (Input.GetMouseButtonDown(0) && path.Count > 0 ){

            if(character_clicked.attacking){

                foreach (Transform character in characters){

                    if ( character.position == path[0].transform.position){

                        character.GetComponent<Character_Script>().health_system.Damage(character_clicked.slash_attack_damage);
                    }
                }
                
                character_clicked.SlashAttack();
                attack_in_process = false;
            }else{

                if (character_clicked != null){
            
                character_clicked.moving = true;
                }
            }
            
        }
    }

    private void MoveAlongPath(){

        var step = 4 * Time.deltaTime; //Movement speed

        character_clicked.transform.position = Vector2.MoveTowards(character_clicked.transform.position, path[0].transform.position, step );

        if (Vector2.Distance(character_clicked.transform.position, path[0].transform.position) < 0.0001f){
            
            character_clicked.PositionCharacterOnTile(path[0]);
            path[0].SetArrowSprite(arrow_direction.None);
            path.RemoveAt(0);
        }

        if (path.Count == 0){
            
            character_clicked.moving = false;
            CancelAction();
            character_clicked = null;
               
        }
    }

    private void CancelAction(){
        
        character_clicked.attacking = false;
        character_clicked.moving = false;

        foreach ( var tile in movement_range){

            tile.SetArrowSprite (arrow_direction.None);
            tile.HideTile();
        }

        foreach ( var tile in attack_range){

            tile.SetArrowSprite (arrow_direction.None);
            tile.HideTile();
        }

        movement_range.Clear();
        attack_range.Clear();
        path.Clear();
    }
}
