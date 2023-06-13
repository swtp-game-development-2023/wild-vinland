using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

///<summary>
/// Class to Save the World Information
///</summary>
[Serializable]
public class SaveGame
{
    public int LevelIndex;
    public List<PositionedTile> SeaTiles, BeachTiles, GrassTiles, MountainTiles, FarmableTiles, DecoTiles, UnitTiles;
    public SerializedInventory Inventory;
    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;
}