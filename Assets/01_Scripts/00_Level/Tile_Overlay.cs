using System;
using System.Collections;
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            
            HideTile();
        }
    }

    public void ShowTile(Color color){
        
        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color  = color;

    }

    public void HideTile(){
        
        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color  = Color.clear;
        SetArrowSprite(arrow_direction.None);

    }

    public void SetArrowSprite (arrow_direction direction ){
        
        var arrow = GetComponentsInChildren<SpriteRenderer>()[1];

        if (direction == arrow_direction.None){
            
            arrow.color = Color.clear;
        }else{

            arrow.color = Color.white;
            arrow.sprite = arrow_sprites [(int)direction];
            int_arrow_direction = (int)direction;
        }

    }

}
