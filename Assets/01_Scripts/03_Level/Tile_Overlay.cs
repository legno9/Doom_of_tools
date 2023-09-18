using System.Collections.Generic;
using UnityEngine;
using static Character_Arrow;

public class Tile_Overlay : MonoBehaviour
{
    
    [System.NonSerialized]public int start_cost;
    [System.NonSerialized]public int end_cost;
    public int total_cost{get{return start_cost+end_cost;}}
    [System.NonSerialized]public bool blocked;
    [System.NonSerialized]public Tile_Overlay previous;
    [System.NonSerialized]public Vector3Int grid_location;
    public Vector2Int grid2D_location {get { return new Vector2Int(grid_location.x, grid_location.y);}}
    public List<Sprite> arrow_sprites;
    [System.NonSerialized]public int int_arrow_direction;
    [System.NonSerialized]public Names.direction_name direction;
    private Animator above_animator;
    private SpriteRenderer arrow_sprite;
    private SpriteRenderer hit_sprite;
    public Color red_color; 
    public Color blue_color;

    private void Awake() {
        
        above_animator = GetComponentInChildren<Animator>();
        arrow_sprite = GetComponentsInChildren<SpriteRenderer>()[1];
        hit_sprite = GetComponentsInChildren<SpriteRenderer>()[2];
        above_animator.SetBool("Selected",true);
        blue_color = new(0.067f, 0.251f, 0.851f);
        red_color = new(0.95f, 0.15f, 0.23f);
    }
    public void ShowTile(Color color, float extract_alpha= 0){
        
        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color  = color - new Color(0,0,0, extract_alpha);

    }

    public void HideTile(){

        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color  = Color.clear;
        hit_sprite.color = Color.clear;

    }

    public void SetArrowSprite (arrow_direction direction ){
        
        if (direction == arrow_direction.None){
            
            arrow_sprite.color = Color.clear;
        }else{

            arrow_sprite.color = Color.white;
            arrow_sprite.sprite = arrow_sprites [(int)direction];
            int_arrow_direction = (int)direction;
        }

    }

    public void SetHitSprite (bool selected ){
        
        SetArrowSprite (arrow_direction.None);

        if (selected){
            
            hit_sprite.color = Color.white;
            
        }else{

            hit_sprite.color = Color.clear;
        }
    }

    public void SetTileDirection (Tile_Overlay character_tile){

        float tile_x = grid2D_location.x;
        float tile_y = grid2D_location.y;
        float charac_x = character_tile.grid2D_location.x;
        float charac_y = character_tile.grid2D_location.y;  

        if (tile_y > charac_y && ((Mathf.Abs((tile_y - charac_y)/(tile_x-charac_x))) > 1)){
            direction =  Names.direction_name.Top;
        }

        else if (tile_y < charac_y && ((Mathf.Abs((tile_y - charac_y)/(tile_x-charac_x))) > 1)){
            direction = Names.direction_name.Bottom;
        }
        
        else if (tile_x > charac_x && ((Mathf.Abs((tile_y - charac_y)/(tile_x-charac_x))) < 1)){
            direction = Names.direction_name.Right;
        }

        else if (tile_x < charac_x && ((Mathf.Abs((tile_y - charac_y)/(tile_x-charac_x))) < 1)){
            direction = Names.direction_name.Left;
        }

        else {
            Debug.Log ("Error, tile direction not set");
        }

    }

}
