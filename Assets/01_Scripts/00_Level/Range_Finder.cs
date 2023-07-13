using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Range_Finder{

    public List<Tile_Overlay> GetAdjacentTiles( Tile_Overlay start, int range ){

        var in_range = new List<Tile_Overlay>(); 
        int step_count = 0;

        var previous_step_tiles = new List<Tile_Overlay>(); 
        previous_step_tiles.Add (start);

        while ( step_count < range){

            var surrounding = new List<Tile_Overlay>();

            foreach (var tile in previous_step_tiles){

                surrounding.AddRange(Tiles_Manager.Instance.GetNeighbourTile(tile, new List<Tile_Overlay>() ));
            }

            in_range.AddRange (surrounding);
            previous_step_tiles = surrounding.Distinct().ToList();
            step_count ++;
        }

        return in_range.Distinct().ToList();
    }
   
}
