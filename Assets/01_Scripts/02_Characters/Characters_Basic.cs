using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class Characters_Basic : MonoBehaviour
{
    [System.NonSerialized] public Animator animator;
    public Health_System health_system;
    [System.NonSerialized] public Health_Bar health_bar;
    [System.NonSerialized] public Tile_Overlay active_tile; 

    private Dictionary<Transform ,Vector2Int> total_characters_tile;
    private Dictionary<Transform ,Vector2Int> ally_characters_tile;
    private Dictionary<Transform ,Vector2Int> enemy_characters_tile;

    public Names.character name_;
    public int health;
    public int movement;
    public int speed = 8;
    public bool ally;
    [System.NonSerialized]public bool action_used = false;
    [System.NonSerialized]public int damage_amount;
    [System.NonSerialized]public int heal_amount;
    [System.NonSerialized]public bool select_all_tiles;
    

    private void Awake() {
        
        this.name = name_.ToString();
        animator = GetComponentInChildren<Animator>();
        health_system = new Health_System(health, this);
        health_bar = GetComponentInChildren<Health_Bar>();
        StartCoroutine(SetCharactersStartTile());  
    } 

    IEnumerator SetCharactersStartTile() {
         yield return new WaitForFixedUpdate();
        while (active_tile == null) {

            RaycastHit2D start_hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.zero);

            if (start_hit.collider != null) {
                
                Tile_Overlay start_tile = start_hit.transform.GetComponentInParent<Tile_Overlay>();
                PositionCharacterOnTile(start_tile);
                
            }
            
            yield return new WaitForEndOfFrame();
        }    
    }

    public IEnumerator MoveAlongPath(List<Tile_Overlay> movement_path){
        
        Mouse_Manager.Instance.character_clicked = null;
        Mouse_Manager.Instance.moving = true;
        action_used = true;
        animator.SetBool("Walking",true);
        
        while (movement_path.Count > 0){

            var step = speed * Time.deltaTime; //Movement speed

            transform.position = Vector2.MoveTowards(transform.position, movement_path[0].transform.position, step );

            if (Vector2.Distance(transform.position, movement_path[0].transform.position) < 0.0001f){
                
                PositionCharacterOnTile(movement_path[0]);
                movement_path[0].SetArrowSprite(Character_Arrow.arrow_direction.None);
                movement_path.RemoveAt(0);
            }
            
            yield return new WaitForEndOfFrame();
        }
        
        animator.SetBool("Walking",false);
        Mouse_Manager.Instance.moving = false;
        
    }

    public void PositionCharacterOnTile (Tile_Overlay tile){
        
        if (active_tile is null) {

            tile.blocked = true; //Block the tile used
        }else{

            active_tile.blocked = false;
        }

        ally_characters_tile = Mouse_Manager.Instance.ally_characters_tile;
        enemy_characters_tile = Mouse_Manager.Instance.enemy_characters_tile;
        total_characters_tile = Mouse_Manager.Instance.total_characters_tile;
           
        transform.position = tile.transform.position; //Set the same position 
        active_tile = tile;
        active_tile.blocked = true;


        if (ally){

            if (ally_characters_tile.ContainsKey(transform)){
    
                ally_characters_tile.Remove(transform);  //Remove the last position before adding the new one
                total_characters_tile.Remove(transform);
            }

            ally_characters_tile.Add(transform,active_tile.grid2D_location);
            total_characters_tile.Add(transform,active_tile.grid2D_location);
            
        }else{

            if (enemy_characters_tile.ContainsKey(transform)){
    
                enemy_characters_tile.Remove(transform); 
                total_characters_tile.Remove(transform);
            }

            enemy_characters_tile.Add(transform,active_tile.grid2D_location);
            total_characters_tile.Add(transform,active_tile.grid2D_location);
        }  
    }

    public void Attack ( List<Tile_Overlay> attack_range_selected){

        action_used = true;
        List<Transform> characters_to_attack = new List<Transform> (total_characters_tile.Keys);

        foreach ( Transform c in characters_to_attack){
            foreach ( Tile_Overlay t in attack_range_selected){
                if (c.position == t.transform.position){  // If any tile position its the same as any character position, do damage
                    
                    c.GetComponent<Characters_Basic>().health_system.Damage(damage_amount);
                    
                }
            } 
        }
    }

    public int TilesOfDistance( Vector2Int end_tile, Vector2Int start_tile = default (Vector2Int) ){

        if (start_tile == Vector2Int.zero){

            start_tile = active_tile.grid2D_location;
        }
        
        Vector2Int distance =  end_tile - start_tile;
        int distance_tiles = (int)(Mathf.Abs(distance.x) + Mathf.Abs(distance.y));

        return distance_tiles;
    }

    public void Dead (){
        
        Mouse_Manager.Instance.total_characters_tile.Remove(transform);
        
        if (ally){

            Mouse_Manager.Instance.ally_characters_tile.Remove(transform);
            Cards_Manager.Instance.RemoveCardsCharacterDead(this);
            
        }else{

             Mouse_Manager.Instance.enemy_characters_tile.Remove(transform);
        }

        active_tile.blocked = false;
        
        
        Destroy(this.gameObject);
    }
}

    
