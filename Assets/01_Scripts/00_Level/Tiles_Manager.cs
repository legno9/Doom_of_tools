using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles_Manager : MonoBehaviour
{
    private static Tiles_Manager manager_instance;
    public static Tiles_Manager Instance {get { return manager_instance; } }
    [SerializeField] private Tile_Overlay overlay_tile_prefab;
    [SerializeField] private GameObject overlay_container;
    public Dictionary<Vector2Int, Tile_Overlay> map; //To store all the tiles


    private void Awake() {
        
        if (manager_instance != null && manager_instance!= this){ // Only one Tiles_Manager in the scene
            
            Destroy(this.gameObject);
            
        }
        else{
            
            manager_instance = this;
        }
    }
    void Start()
    {
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

    public List<Tile_Overlay> GetNeighbourTile (Tile_Overlay currentOverlayTile, List<Tile_Overlay> available_tiles){

        List<Tile_Overlay> neighbour = new List<Tile_Overlay>(); 
        Dictionary<Vector2Int ,Tile_Overlay> tile_to_search = new Dictionary<Vector2Int ,Tile_Overlay>();

        if (available_tiles.Count > 0){
            
            foreach (var tile in available_tiles ){

                tile_to_search.Add(tile.grid2D_location, tile);

            }
        } else{

            tile_to_search = map;
        }
        
        Vector2Int location_to_check = new Vector2Int (currentOverlayTile.grid_location.x, currentOverlayTile.grid_location.y + 1); // Top 

        if (tile_to_search.ContainsKey(location_to_check)){

            neighbour.Add (tile_to_search[location_to_check]);
        }

        location_to_check = new Vector2Int (currentOverlayTile.grid_location.x, currentOverlayTile.grid_location.y - 1); // Bottom 

        if (tile_to_search.ContainsKey(location_to_check)){
            
            neighbour.Add (tile_to_search[location_to_check]);
        }

        location_to_check = new Vector2Int (currentOverlayTile.grid_location.x + 1, currentOverlayTile.grid_location.y); // Right 

        if (tile_to_search.ContainsKey(location_to_check)){
            
            neighbour.Add (tile_to_search[location_to_check]);
        }

        location_to_check = new Vector2Int (currentOverlayTile.grid_location.x - 1, currentOverlayTile.grid_location.y); // Left 

        if (tile_to_search.ContainsKey(location_to_check)){
            
            neighbour.Add (tile_to_search[location_to_check]);
        }

        return neighbour;
    }

}
