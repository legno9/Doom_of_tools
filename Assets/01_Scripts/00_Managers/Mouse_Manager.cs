using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //Could cause lag because is in update
using UnityEngine;

public class Mouse_Manager : MonoBehaviour
{
    private static Mouse_Manager manager_instance;
    public static Mouse_Manager Instance {get { return manager_instance; } }

    private SpriteRenderer selector_sprite;
    private Character_Arrow character_arrow = new();
    private Path_Finder path_finder = new();
    private Range_Finder range_finder = new();

    public Dictionary<Transform ,Vector2Int> total_characters_tile = new(); //Store current characters position
    public Dictionary<Transform ,Vector2Int> ally_characters_tile = new();
    public Dictionary<Transform ,Vector2Int> enemy_characters_tile = new();

    [System.NonSerialized]public List<Tile_Overlay> attack_range = new();
    [System.NonSerialized]public List<Tile_Overlay> attack_range_selected = new();

    [System.NonSerialized] public Characters_Basic character_clicked;
    [System.NonSerialized] public Card_Mono card_on_use = null;
    [System.NonSerialized] public bool moving = false;
    private List<Tile_Overlay> path = new();
    private List<Tile_Overlay> movement_range = new();
    

    private void Awake() { 
        
        if (manager_instance == null){

            manager_instance = this;
        }else{

            Destroy(this.gameObject);}

        selector_sprite = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 60;
    }

    private void LateUpdate(){ //Select tiles

        
        
        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouse_pos_2D = new( mouse_pos.x, mouse_pos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll (mouse_pos_2D, Vector2.zero);  // Detects all colliders and return them in an array from first to last

        if (hits.Length > 0){
            
            RaycastHit2D focus_tile =  hits.OrderByDescending(i => i.collider.transform.position.z).First();
            
            Tile_Overlay overlay_tile = focus_tile.collider.gameObject.GetComponentInParent<Tile_Overlay>(); //Get the V_Overlay_Tilemap collided
            transform.position = overlay_tile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlay_tile.GetComponentInChildren<SpriteRenderer>().sortingOrder+1; // Assign the same +1 sorting order
            
            if (character_clicked != null){ 
                
                CheckAttackRange(overlay_tile);
                CheckMovementRange(overlay_tile);
                CheckStopAll(overlay_tile);
            }

            
            CheckCharacterClicked(overlay_tile);

        }else{

            CheckStopAll();
        }
    }
    private void CheckAttackRange( Tile_Overlay tile_to_check){

        attack_range_selected.Clear();

        if (attack_range.Contains(tile_to_check)){
            
            if (character_clicked.select_all_tiles is true){
                
                attack_range_selected.AddRange(attack_range);

                if (attack_range.Contains(tile_to_check)){
                    
                    foreach (Tile_Overlay tile in attack_range){
                        
                        tile.SetHitSprite(selected:true); //Set attack tiles sprite
                    }
                }                 
            
                    
            }else{

                foreach (Tile_Overlay tile in attack_range){
                    
                    tile.SetTileDirection(character_clicked.active_tile); 
                }

                if (attack_range.Contains(tile_to_check)){
                    
                    foreach (Tile_Overlay tile in attack_range){
                        
                        if (tile.direction == tile_to_check.direction){
                            
                            tile.SetHitSprite(selected:true);
                            attack_range_selected.Add(tile);
                        
                        }else{
                            tile.SetHitSprite(selected:false);
                            
                        }
                    }
                }  
            }

        }else{
            foreach ( var tile in attack_range){ 

                tile.SetHitSprite(selected:false);
            }
        }

        if (Input.GetMouseButtonDown(0) && attack_range_selected.Count > 0){ 
            
            character_clicked.Attack(attack_range_selected);
            Cards_Manager.Instance.DiscardCard(card_on_use);
            card_on_use = null;
            ClearAndHide();
        }
       
    }  
    private void CheckMovementRange( Tile_Overlay tile_to_check){

        if (movement_range.Contains(tile_to_check)){ //Display all the arrows to the tile hovered
            
            path = path_finder.FindPath(character_clicked.active_tile, tile_to_check);
            
            while (path.Count > character_clicked.movement){

                path.Remove(path.Last());
            }

            foreach ( var tile in movement_range){ //Remove unused arrows
                
                tile.SetArrowSprite (Character_Arrow.arrow_direction.None);
            }

            for (int i = 0; i< path.Count; i++ ){ // Show arrows

                var previous_tile = i > 0? path[i-1] : character_clicked.active_tile;
                var future_tile = i < path.Count -1 ? path[i+1]: null;

                var arrow_dir = character_arrow.TranslateDirection(previous_tile, path[i], future_tile);
                path[i].SetArrowSprite(arrow_dir);
                
            }
            
            if (Input.GetMouseButtonDown(0) && path.Count > 0){   //Move
                    
                HideTiles();
                StartCoroutine(character_clicked.MoveAlongPath(path));
                selector_sprite.enabled = true;
                
            } 
        }
    }
    private void CheckCharacterClicked (Tile_Overlay tile_to_check) {
        
        if (Input.GetMouseButtonDown(0) && ally_characters_tile.ContainsValue(tile_to_check.grid2D_location)){ 

            foreach( Transform character in ally_characters_tile.Keys ){
                
                if (character.transform.position == transform.position){
                    
                    if (!character.GetComponent<Characters_Basic>().action_used){
                        
                        HideTiles(); //If a new character is clicked
                        character_clicked = character.GetComponent<Characters_Basic>();
                        selector_sprite.enabled = false; 
                    }

                    else{

                        return;
                    }
                }
            }
            
            movement_range = range_finder.GetAdjacentTiles(character_clicked.active_tile ,character_clicked.movement); 

            foreach( Tile_Overlay tile in movement_range ){
        
                tile.ShowTile(Color.blue);
            }
        } 
    }
    private void CheckStopAll(Tile_Overlay tile_to_check){

        if ((Input.GetMouseButtonDown(0) && !movement_range.Contains(tile_to_check) && !attack_range.Contains(tile_to_check) ) || (Input.GetMouseButtonDown(1) ) ){
            
            if (card_on_use is not null){

                StartCoroutine(card_on_use.ReturnToHand());
                
            }else{

                ClearAndHide();
                character_clicked = null;

            }
        }
    }
    private void CheckStopAll(){

        if ( Input.GetMouseButtonDown(1) ){
            
            if (card_on_use is not null){

                StartCoroutine(card_on_use.ReturnToHand());
                
            }else{

                ClearAndHide();
                character_clicked = null;

            }
        }

    }
    public Characters_Basic GetCharacterScript ( Names.character name){

        foreach (Transform c in ally_characters_tile.Keys){
            
             Characters_Basic basics = c.GetComponent<Characters_Basic>();

            if (basics.name_ == name){

                return basics;
            }
        }

        return null;

    }

    
    public void ClearAndHide(){
        
        HideTiles();
        ClearActions();  
    }
    public void ClearActions(){
        
        foreach ( var tile in movement_range){

            tile.SetArrowSprite (Character_Arrow.arrow_direction.None);
        }

        foreach ( var tile in attack_range){

            tile.SetArrowSprite (Character_Arrow.arrow_direction.None);
        }

        movement_range.Clear();
        attack_range.Clear();
        path.Clear();
        selector_sprite.enabled = true;
    }
    public void HideTiles(){

        foreach ( var tile in movement_range){

            tile.HideTile();
        }
        
        foreach ( var tile in attack_range){

            tile.HideTile();
        }
    }
    
}
