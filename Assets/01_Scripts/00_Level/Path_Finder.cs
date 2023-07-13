using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path_Finder
{
    public List <Tile_Overlay> FindPath (Tile_Overlay start,Tile_Overlay end, List<Tile_Overlay> available_tiles){

        List<Tile_Overlay> openList = new List<Tile_Overlay>();
        List<Tile_Overlay> closedList = new List<Tile_Overlay>();
        
        openList.Add(start);

        while(openList.Count > 0){
            
            Tile_Overlay currentOverlayTile = openList.OrderBy(x => x.total_cost).First();
            
            openList.Remove (currentOverlayTile);
            closedList.Add (currentOverlayTile);

            if (currentOverlayTile == end){
                
                return GetFinishedList (start, end);
                
            }

            var neighbour_tiles = Tiles_Manager.Instance.GetNeighbourTile(currentOverlayTile, available_tiles);

            foreach (var neighbour in neighbour_tiles){

                if (neighbour.blocked || closedList.Contains(neighbour)){
                    continue;
                }

                neighbour.start_cost = GetDistance(start, neighbour);
                neighbour.end_cost = GetDistance(end, neighbour);

                neighbour.previous = currentOverlayTile;
                
                
                if (!openList.Contains(neighbour)){

                    openList.Add(neighbour);
                }
            }

        }

        return new List<Tile_Overlay>();
    }

    private List<Tile_Overlay> GetFinishedList(Tile_Overlay start, Tile_Overlay end)
    {
        List<Tile_Overlay> finished_list = new List<Tile_Overlay>();
        Tile_Overlay current_tile = end;

        while (current_tile != start){

            finished_list.Add(current_tile);
            current_tile = current_tile.previous;

        }

        finished_list.Reverse();
        return finished_list;
    }

    private int GetDistance(Tile_Overlay start, Tile_Overlay neighbour)
    {
        return Mathf.Abs( start.grid_location.x - neighbour.grid_location.x) + Mathf.Abs( start.grid_location.y - neighbour.grid_location.y);
    }

    
}
