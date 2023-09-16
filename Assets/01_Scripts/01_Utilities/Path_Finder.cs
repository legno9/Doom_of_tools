using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path_Finder{

    private Range_Finder range_finder = new();
    public List <Tile_Overlay> FindPath (Tile_Overlay start,Tile_Overlay end, int character_movement){

        List<Tile_Overlay> openList = new();
        List<Tile_Overlay> closedList = new();
        
        openList.Add(start);
        int tiles_added = 0;

        while(openList.Count > 0){
            
            Tile_Overlay current_overlay_tile = openList.OrderBy(x => x.total_cost).First(); //If lag, recode this
            
            openList.Remove (current_overlay_tile);
            closedList.Add (current_overlay_tile);
            tiles_added += 1;

            if (current_overlay_tile == end ){ //Because is taking into account the start tile
                
                List<Tile_Overlay> finished_list = new();
                Tile_Overlay current_tile = end;

                while (current_tile != start){

                    finished_list.Add(current_tile);
                    current_tile = current_tile.previous;
                
                }

                finished_list.Reverse();

                while (finished_list.Count > character_movement){

                    finished_list.Remove(finished_list.Last());
                }
                return finished_list;
                
            }

            var neighbour_tiles = range_finder.GetAdjacentTiles(current_overlay_tile);

            foreach (var neighbour in neighbour_tiles){

                if (neighbour.blocked || closedList.Contains(neighbour)){
                    continue;
                }

                neighbour.start_cost = GetDistance(start, neighbour);
                neighbour.end_cost = GetDistance(end, neighbour);

                neighbour.previous = current_overlay_tile;
                
                
                if (!openList.Contains(neighbour)){

                    openList.Add(neighbour);
                }
            }

        }

        return new List<Tile_Overlay>();
    }

    private int GetDistance(Tile_Overlay start, Tile_Overlay neighbour)
    {
        return Mathf.Abs( start.grid_location.x - neighbour.grid_location.x) + Mathf.Abs( start.grid_location.y - neighbour.grid_location.y);
    }

    
}
