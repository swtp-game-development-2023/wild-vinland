using System;
using UnityEngine;

///<summary>
/// Class to Save a Tile and its Positon in the Mapgrid
///</summary>
[Serializable]
public class PositionedTile {
    public Vector3Int Position;
    public MapTile Tile;
}