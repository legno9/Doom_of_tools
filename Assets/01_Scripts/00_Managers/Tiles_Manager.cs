using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Tiles_Manager : MonoBehaviour
{
    private static Tiles_Manager manager_instance;
    public static Tiles_Manager Instance {get { return manager_instance; } }

    [SerializeField] private Tile_Overlay overlay_tile_prefab;
    [SerializeField]private GameObject overlay_container;

    public Dictionary<Vector2Int, Tile_Overlay> map; //To store all the tiles
    public Dictionary<Names.direction_name, Vector2Int> direction = new(4){ //To store the grid direction
        {Names.direction_name.Top, new Vector2Int (0,1)},
        {Names.direction_name.Bottom, new Vector2Int (0,-1)},
        {Names.direction_name.Right, new Vector2Int (1,0)},
        {Names.direction_name.Left, new Vector2Int (-1,0)},
    };

    private void CreateSingleton(){
        
        if (manager_instance == null){

            manager_instance = this;
        }else{

            Destroy(this.gameObject);}
    }
    
    void Awake(){
        
        CreateSingleton();
        var tilemap = gameObject.GetComponentInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, Tile_Overlay>();
        BoundsInt bounds = tilemap.cellBounds;
        
        //Loop through all tiles
            
        for (int y = bounds.min.y; y < bounds.max.y; y++){
            
            for (int x = bounds.min.x; x < bounds.max.x; x++){
                
                var tile_location = new Vector3Int (x,y,0);
                var tile_key = new Vector2Int (x,y);
                
                if (tilemap.HasTile (tile_location) && !map.ContainsKey(tile_key)){

                    var overlay_tile = Instantiate (overlay_tile_prefab, overlay_container.transform);
                    var cell_world_position = tilemap.GetCellCenterWorld (tile_location);

                    overlay_tile.transform.position = new Vector3 (cell_world_position.x, cell_world_position.y, cell_world_position.z +1 ); //+1 To separate it from the floor
                    overlay_tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = tilemap.GetComponent<TilemapRenderer>().sortingOrder;
                    overlay_tile.grid_location = tile_location;
                    overlay_tile.name = "Tile "+ tile_location;
                    map.Add(tile_key, overlay_tile); // Add the overlay_tile in the scene

                }
            }
        }
    }

}
