using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Level Tile", menuName = "2D/Tiles/Level Tile")]
public class MapTile : Tile
{
    public TileType Type;

}

[Serializable]
public enum TileType
{
    // Ground
   
   Sea = 0,
   Beach = 1,
   Grass = 2,
   Mountain = 3,
   Farmable  = 4,
   Deco = 5,

   // Unit
   Player = 1000,
   Animal  = 1001
}
