using System.Collections.Generic;
using System.Text;
using UnityEngine;

///<summary>
/// Class to Save the World Information
///</summary>
public class SaveGame
{
    public int LevelIndex;
    public List<PositionedTile> SeaTiles, BeachTiles, GrassTiles, MountainTiles, FarmableTiles, DecoTiles, UnitTiles;
    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;
    public List<Collectable> Inventory;
}