using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace World
{
    public class WorldHelper : MonoBehaviour
    {
        
        ///<summary>
        /// Finds all Tilemaps and Gameobjects and deletes all Tiles, Objects, clears player inventory and resets the player position
        ///</summary>
        public static void ClearMap()
        {
            var maps = FindObjectsOfType<Tilemap>();

            foreach (var tilemap in maps)
            {
                tilemap.ClearAllTiles();

                if(tilemap.transform.childCount > 0)
                {
                    foreach (Transform gamobjectInTilemap in tilemap.transform)
                    {
                        Destroy(gamobjectInTilemap.gameObject);
                    }
                }
            }
            SetPlayerPosition(new Vector3(0,0,0));
            GetPlayer().GetComponent<Inventory>().ClearSlots();
        }

        ///<summary>
        /// Get Player GameObject
        ///</summary>
        public static GameObject GetPlayer(){ return GameObject.FindWithTag("Player"); }

        ///<summary>
        /// Returns the Player Position as a Vector2 since we use a 2D Game World
        ///</summary>
        public static Vector2 GetPlayerPositon(){
            try
            {
            return GetPlayer().transform.position;
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
              return GetPlayer().transform.rotation;  
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
            GetPlayer().transform.position = position;
        }

        ///<summary>
        /// Sets the Player Rotation
        ///</summary>
        public static void SetPlayerRotation(Quaternion rotation){
            GetPlayer().transform.rotation = rotation;
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