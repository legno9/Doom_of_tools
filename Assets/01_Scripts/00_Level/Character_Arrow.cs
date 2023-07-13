using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Arrow{

    public enum arrow_direction{

        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        TopRight = 5,
        BottomRight = 6,
        TopLeft = 7,
        BottomLeft = 8,
        UpFinished = 9,
        DownFinished = 10,
        LeftFinished = 11,
        RightFinished = 12,
    }

    public arrow_direction TanslateDirection( Tile_Overlay previous, Tile_Overlay current, Tile_Overlay future){

        bool final = future == null;

        Vector2Int past_direction = previous != null? current.grid2D_location - previous.grid2D_location : new Vector2Int (0,0);
        Vector2Int future_direction = future != null? future.grid2D_location - current.grid2D_location : new Vector2Int (0,0);
        Vector2Int direction = past_direction != future_direction? past_direction + future_direction : future_direction;

        if (direction == new Vector2Int (0,1) && !final){
            return arrow_direction.Up;
        }

        if (direction == new Vector2Int (0,-1) && !final){
            return arrow_direction.Down;
        }

        if (direction == new Vector2Int (1,0) && !final){
            return arrow_direction.Right;
        }

        if (direction == new Vector2Int (-1,0) && !final){
            return arrow_direction.Left;
        }

        if (direction == new Vector2Int (1,1)){
            
            if (past_direction.y < future_direction.y ){
                return arrow_direction.BottomLeft;
            }else{
                return arrow_direction.TopRight;
            }
        }

        if (direction == new Vector2Int (-1,1)){
            
            if (past_direction.y < future_direction.y ){
                return arrow_direction.BottomRight;
            }else{
                return arrow_direction.TopLeft;
            }
        }

        if (direction == new Vector2Int (1,-1)){
            
            if (past_direction.y > future_direction.y ){
                return arrow_direction.TopLeft;
            }else{
                return arrow_direction.BottomRight;
            }
        }

        if (direction == new Vector2Int (-1,-1)){
            
            if (past_direction.y > future_direction.y ){
                return arrow_direction.TopRight;
            }else{
                return arrow_direction.BottomLeft;
            }
        }
        if (direction == new Vector2Int (0,1) && final){
            return arrow_direction.UpFinished;
        }

        if (direction == new Vector2Int (0,-1) && final){
            return arrow_direction.DownFinished;
        }

        if (direction == new Vector2Int (1,0) && final){
            return arrow_direction.RightFinished;
        }

        if (direction == new Vector2Int (-1,0) && final){
            return arrow_direction.LeftFinished;
        }

        return arrow_direction.None;
    }
}

