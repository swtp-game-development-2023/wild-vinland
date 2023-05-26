using UnityEngine;
using UnityEngine.Tilemaps;

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
        public static void SetTile(Tilemap map, PositionedTile tile)
        {
            map.SetTile(tile.Position, tile.Tile);
        }
    }
}