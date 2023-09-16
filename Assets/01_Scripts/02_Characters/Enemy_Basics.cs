using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Basics: MonoBehaviour{
    
    protected Range_Finder range_finder = new();
    protected Path_Finder path_finder = new();
    private Character_Arrow character_arrow = new();
    protected Dictionary<Transform ,Vector2Int> ally_characters_tile;
    protected Dictionary<Transform ,Vector2Int> total_characters_tile;
    protected Characters_Basic basics;
    protected List<Tile_Overlay> movement_range;
    protected List <Tile_Overlay> path;
    protected Tile_Overlay desired_tile_move;
    protected Names.attack_selector[] available_attacks;
    protected Dictionary< Tile_Overlay, Names.attack_selector > attacks_preference = new();
    public List<Tile_Overlay> attack_range;
    protected List<Tile_Overlay> attack_range_selected = new();
    protected Vector2Int attack_position;
    protected bool can_attack = false;
    protected bool moving = false;
    protected Tile_Overlay tile_attacking;

    public IEnumerator StartController() {
        
        Reset();
        CheckAttacks();
        
        
        if (!can_attack){

            GetNearestAlly();
            MoveToAttackPosition();
            
            yield return new WaitUntil(() => moving == false );
            Reset();
            CheckAttacks();

            if (!can_attack){

                Turns_Manager.Instance.next_enemy= true;
            }else{

                StartCoroutine(Attack ());
            }
            

        }else{

            StartCoroutine(Attack ());
        }
    }

    protected virtual void CheckAttacks(){
        
        foreach(Names.attack_selector attack in available_attacks){
            
            attack_range = (Attacks_Manager.Instance.GetEnemyCharacterAttacks(basics, attack));
            
            foreach (Tile_Overlay t in attack_range){
                attacks_preference.Add(t, attack);
                foreach ( Transform c in ally_characters_tile.Keys){
                    
                    if (c.position == t.transform.position){  // If any tile position its the same as any character position
                        
                        can_attack = true;
                        tile_attacking = t;
                        return;
                    }
                }

                if (can_attack){
                    
                    return;
                }               
            }
               
            if (can_attack){
                
                return;
            }    
        }
    }

    protected virtual void GetNearestAlly(){

        int shortest_distance = 10;

        Names.attack_selector attack_type = Names.attack_selector.none;

        foreach (Tile_Overlay tile in attacks_preference.Keys){
            
            if (attack_type == Names.attack_selector.none ){ //From the most important attack to the least
                foreach ( Vector2Int ally_position in ally_characters_tile.Values){

                    int tiles_number = basics.TilesOfDistance(ally_position, tile.grid2D_location);

                    if ( tiles_number < shortest_distance){
                                                
                        attack_type = attacks_preference[tile];
                        attack_position = ally_position - tile.grid2D_location + basics.active_tile.grid2D_location;
                        shortest_distance = tiles_number;
                        
                    }
                }
            }else{

                return;
            }  
        }  
    }

    protected virtual void MoveToAttackPosition(){
        
        int lowest_distance = 10; //Provisional number

        foreach (Tile_Overlay possible_tile in movement_range){ //Get nearest tile to new_position

            possible_tile.ShowTile(possible_tile.blue_color);
            int new_distance = basics.TilesOfDistance(attack_position, possible_tile.grid2D_location );
            
            if ( new_distance < lowest_distance ){

                lowest_distance = new_distance;
                desired_tile_move = possible_tile;

            }  
        }

        if (desired_tile_move.grid2D_location != attack_position && lowest_distance <= basics.movement){ //Out of map ///Recheck///////

            //Shoulr revise next possible attack position
            //If there isnt any posible attack move to create one
        }

        path = path_finder.FindPath(basics.active_tile, desired_tile_move, basics.movement);

        if (path.Count == 0){ //Cant move
            
            path = path_finder.FindPath(basics.active_tile, movement_range[UnityEngine.Random.Range(0, movement_range.Count)],basics.movement); //Random tile

        }

        StartCoroutine(MoveEnemyAlongPath(path));
        moving = true;
        
    }

    protected IEnumerator MoveEnemyAlongPath(List<Tile_Overlay> movement_path){

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i< path.Count; i++ ){ // Show arrows

            var previous_tile = i > 0? path[i-1] : basics.active_tile;
            var future_tile = i < path.Count -1 ? path[i+1]: null;

            var arrow_dir = character_arrow.TranslateDirection(previous_tile, path[i], future_tile);
            path[i].SetArrowSprite(arrow_dir);
        }

        yield return new WaitForSeconds(0.5f);
        
        while (movement_path.Count > 0){

            basics.animator.SetBool("Walking",true);

            var step = basics.speed * Time.deltaTime; //Movement speed

            transform.position = Vector2.MoveTowards(transform.position, movement_path[0].transform.position, step );

            if (Vector2.Distance(transform.position, movement_path[0].transform.position) < 0.0001f){
                
                basics.PositionCharacterOnTile(movement_path[0]);
                movement_path[0].SetArrowSprite(Character_Arrow.arrow_direction.None);
                movement_path.RemoveAt(0);
            }

            yield return new WaitForEndOfFrame();
        }

        HideTiles();
        basics.animator.SetBool("Walking",false);
        moving = false;
    }

    protected IEnumerator Attack (){

        List<Transform> to_attack = new List<Transform>(total_characters_tile.Keys);
        Attacks_Manager.Instance.SetAttackBasics();
            
        if (basics.select_all_tiles is true){

            foreach (Tile_Overlay tile in attack_range){
                
                tile.SetHitSprite(selected:true); //Set attack tiles sprite
                tile.ShowTile(tile.red_color);
            }
                                
        }else{

            foreach (Tile_Overlay tile in attack_range){
                
                tile.SetTileDirection(basics.active_tile);
                tile.ShowTile(tile.red_color); 
                    
                if (tile.direction == tile_attacking.direction){
                    
                    tile.SetHitSprite(selected:true);
                    attack_range_selected.Add(tile);
                
                }else{
                    tile.SetHitSprite(selected:false);
                    
                }
            }     
        }
        

        yield return new WaitForSeconds(1.5f);

        foreach ( Transform c in to_attack){
            foreach ( Tile_Overlay t in attack_range){
                if (c.position == t.transform.position){  // If any tile position its the same as any character position, do damage ///////////////////////Error killed
                    
                    c.GetComponent<Characters_Basic>().health_system.Damage(basics.damage_amount);
                    
                }
            } 
        }

        HideTiles();
        Turns_Manager.Instance.next_enemy = true;
         
    }

    protected virtual void Reset(){

        ally_characters_tile = Mouse_Manager.Instance.ally_characters_tile;
        total_characters_tile = Mouse_Manager.Instance.total_characters_tile;
        basics = gameObject.GetComponent<Characters_Basic>();
        can_attack = false;
        attacks_preference.Clear();
        attack_range_selected.Clear();
        movement_range = range_finder.GetAdjacentTiles(basics.active_tile, basics.movement);
    }

    public void HideTiles(){

        foreach ( var tile in movement_range){

            tile.HideTile();
        }
        
        foreach ( var tile in attack_range){

            tile.HideTile();
            tile.SetHitSprite(false);
        }
    }
}
