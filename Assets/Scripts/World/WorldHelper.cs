﻿using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace World
{
    public class WorldHelper : MonoBehaviour
    {
        
        ///<summary>
        /// Finds all Tilemap Gameobjects and deletes all Tiles
        ///</summary>
        public static void ClearMap()
        {
            var maps = FindObjectsOfType<Tilemap>();

            foreach (var tilemap in maps)
            {
                tilemap.ClearAllTiles();
            }
        }

        ///<summary>
        /// Function to set a PositionedTile on a Tilemap
        ///</summary>
        public static void SetTile(Tilemap map, PositionedTile tile)
        {
            map.SetTile(tile.Position, tile.Tile);
        }

        ///<summary>
        /// Returns the Player Position as a Vector2 since we use a 2D Game World
        ///</summary>
        public static Vector2 GetPlayerPositon(){
            try
            {
            GameObject player = GameObject.FindWithTag("Player");
            return player.transform.position;
            }
            catch (System.Exception)
            {
                Debug.Log("Player Object not Found!");
                throw;
            }
        }

        ///<summary>
        /// Returns the Player Rotation as a Quaternion
        ///</summary>
        public static Quaternion GetPlayerRotation(){
            try
            {
              GameObject player = GameObject.FindWithTag("Player");
              return player.transform.rotation;  
            }
            catch (System.Exception)
            {
                Debug.Log("Player Object not Found!");
                throw;
            }
        }

        ///<summary>
        /// Sets the Player Position
        ///</summary>
        public static void SetPlayerPosition(Vector3 position){
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = position;
        }

        ///<summary>
        /// Sets the Player Rotation
        ///</summary>
        public static void SetPlayerRotation(Quaternion rotation){
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.rotation = rotation;
        }

        public static Vector3 GetRandomTileOfMap(Tilemap map){
            TileBase[] allTiles = map.GetTilesBlock(map.cellBounds);
            List<Vector3> validPositions = new List<Vector3>();
            
            validPositions.Add(new Vector3(0,0,0));
            // Fallback Vector

            Grid grid = GameObject.FindWithTag("World").GetComponent<Grid>() ;
            float cs = grid.cellSize.x;
            // add Grid CellSize
            float offset = grid.GetCellCenterWorld(new Vector3Int(0,0,0)).x; 
            Vector3 position = new Vector3(0,0,0);

            for (int x = 0; x < map.cellBounds.size.x; x++)
            {
                for (int y = 0; y < map.cellBounds.size.y; y++)
                {
                    if( null != allTiles[x + y * map.cellBounds.size.x] )
                    {
                        position.x = x*cs;
                        position.y = y*cs;
                        offset = grid.GetCellCenterWorld(new Vector3Int(0,0,0)).x;
                        position.x = position.x+offset;
                        validPositions.Add(position);
                    }
                }
            }
            // collect possible positions

            int randomPositonIndex = Random.Range(0, validPositions.Count);
            return validPositions[randomPositonIndex];
        }
    }
}