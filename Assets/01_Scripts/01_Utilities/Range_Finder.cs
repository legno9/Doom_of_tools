using System.Collections.Generic;
using System.Linq; /////////////////Distinct could cause lag
using UnityEngine;

public class Range_Finder{
    
    
    public Tile_Overlay GetAdjacentTile ( Names.direction_name direction, Tile_Overlay start_tile ){

        Vector2Int new_location = start_tile.grid2D_location + Tiles_Manager.Instance.direction[direction];

        if (Tiles_Manager.Instance.map.ContainsKey(new_location)){
            
            return (Tiles_Manager.Instance.map[new_location]);
        }

        return null;
    }
    
    public List<Tile_Overlay> GetAdjacentTiles ( Tile_Overlay start_tile, int range = 1 ){

        List<Tile_Overlay> adjacent_tiles = new();
        int step_count = 0;

        adjacent_tiles.Add (start_tile); 

        while (step_count < range){

            var previous_step_tiles = new List<Tile_Overlay>();

            foreach (var tile in adjacent_tiles){

                Tile_Overlay[] tiles_to_check = {
                GetAdjacentTile( Names.direction_name.Top, start_tile:tile),
                GetAdjacentTile( Names.direction_name.Bottom, start_tile:tile),
                GetAdjacentTile( Names.direction_name.Right, start_tile:tile),
                GetAdjacentTile( Names.direction_name.Left, start_tile:tile),
                };

                foreach (Tile_Overlay adjacent_tile in tiles_to_check){
            
                    if (adjacent_tile != null){
                    
                        if (Tiles_Manager.Instance.map.ContainsKey(adjacent_tile.grid2D_location)){

                            previous_step_tiles.Add (adjacent_tile);
                        }   
                    }
                }
            }
            
            adjacent_tiles.AddRange(previous_step_tiles.Distinct().ToList());
            adjacent_tiles = adjacent_tiles.Distinct().ToList();
            adjacent_tiles.Remove(start_tile);
            step_count ++;
        }

        return adjacent_tiles;
    }
    
    public List<Tile_Overlay> GetVerticalTiles (  int ignore_adjacent_tiles , int range, Tile_Overlay start_tile){
            
            if (!start_tile){

                start_tile = Mouse_Manager.Instance.character_clicked.active_tile;
            }
            
            List<Tile_Overlay> vertical_tiles = new();
            int total_tiles = range + ignore_adjacent_tiles;

        foreach (Names.direction_name direction in System.Enum.GetValues(typeof(Names.direction_name))){ 

            Tile_Overlay step_tile = start_tile;
            int step_count = 0;
            
            while ( step_count < total_tiles){

                if (step_tile == null){ //The tile is out of the map range
                    
                    break;
                }

                Tile_Overlay new_step = GetAdjacentTile( direction, step_tile);
                
                if (step_count < ignore_adjacent_tiles ){

                    //pass
                }else{
                    
                    if (new_step != null){
                    
                        if (Tiles_Manager.Instance.map.ContainsKey(new_step.grid2D_location)){

                            vertical_tiles.Add (new_step);
                        }
                    }else{
                         
                        break;
                    }
                }
                step_tile = new_step;
                step_count ++;
            }

        }
        
        return vertical_tiles;
    }

    public List<Tile_Overlay> GetHorizontalTiles ( int ignore_adjacent_tiles , int range, Tile_Overlay start_tile){
        
        List<Tile_Overlay> start_horizontal_tiles = new();
        start_horizontal_tiles.AddRange( GetVerticalTiles(ignore_adjacent_tiles, 1, start_tile));
        
        List<Tile_Overlay> horizontal_tiles = new();

        for ( int t = 0; t < start_horizontal_tiles.Count(); t++){

            int step_count = 1;//There is already 1 tile, the middle one
            List<Tile_Overlay> one_direction_tiles = new()
            {
                start_horizontal_tiles[t]
            };

            while ( step_count < range){

                List<Tile_Overlay> previous_step_tiles = new();

                foreach (Tile_Overlay tile in one_direction_tiles) {
                    
                    tile.SetTileDirection(start_tile);

                    if (tile.direction == Names.direction_name.Top  || tile.direction == Names.direction_name.Bottom){

                        Tile_Overlay[] tiles_to_check = {
                        GetAdjacentTile( Names.direction_name.Right, tile),
                        GetAdjacentTile( Names.direction_name.Left, tile),
                        };
                    
                        foreach (Tile_Overlay adjacent_tile in tiles_to_check){
                            
                            if (adjacent_tile != null){
                            
                                if (Tiles_Manager.Instance.map.ContainsKey(adjacent_tile.grid2D_location)){

                                    previous_step_tiles.Add (adjacent_tile);
                                }   
                            }
                        }
                
                    }else{

                        Tile_Overlay[] tiles_to_check = {
                        GetAdjacentTile( Names.direction_name.Top, tile),
                        GetAdjacentTile( Names.direction_name.Bottom, tile),
                        };
                        
                        foreach (Tile_Overlay adjacent_tile in tiles_to_check){
                    
                            if (adjacent_tile != null){
                            
                                if (Tiles_Manager.Instance.map.ContainsKey(adjacent_tile.grid2D_location)){

                                    previous_step_tiles.Add (adjacent_tile);
                                }   
                            }
                        }
                    }
                    

                }
                
                one_direction_tiles.AddRange((previous_step_tiles.Distinct().ToList()));
                one_direction_tiles = one_direction_tiles.Distinct().ToList();
                
                step_count += 2;
            }

            horizontal_tiles.AddRange((one_direction_tiles.Distinct().ToList()));
            horizontal_tiles = horizontal_tiles.Distinct().ToList();
        }
        
        return horizontal_tiles;
    }

    public List<Tile_Overlay> GetSurroundingTiles ( int ignore_adjacent_tiles, int range, Tile_Overlay start_tile ){

        List<Tile_Overlay> surrounding_tiles = new();
        int tiles_needed = 3 + 2* ignore_adjacent_tiles; // Each row adds 2 tiles

        
        for ( int i = 0; i < range; i++){
            
            surrounding_tiles.AddRange(GetHorizontalTiles(i + ignore_adjacent_tiles, tiles_needed, start_tile));

            tiles_needed += 2;
        }
        
        return surrounding_tiles;
    }

    public List<Tile_Overlay> GetMultipleHorizontal ( int ignore_adjacent_tiles, int range, int rows, Tile_Overlay start_tile ){

        List<Tile_Overlay> multiple_tiles = new();
       
        for (int i = 0; i < rows; i++)
        {
            multiple_tiles.AddRange(GetHorizontalTiles(ignore_adjacent_tiles + i,range,start_tile));
        }
        
        return multiple_tiles;
    }

    public List<Tile_Overlay> GetCrossTiles ( ){
        return new List<Tile_Overlay>();
    }

    public List<Tile_Overlay> GetEnemiesTiles ( ){
        return new List<Tile_Overlay>();
    }

    public List<Tile_Overlay> GetOneEnemyTile ( ){
        return new List<Tile_Overlay>();
    }

    public List<Tile_Overlay> GetAlliesTiles ( ){
        return new List<Tile_Overlay>();
    }

    public List<Tile_Overlay> GetOneAllyTile ( ){
        return new List<Tile_Overlay>();
    }

    public List<Tile_Overlay> GetAleatoryTiles ( ){
        return new List<Tile_Overlay>();
    }
}
