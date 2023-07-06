using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;
using WorldGeneration;

///<summary>
/// Class to Save a Tile and its Positon in the Mapgrid
///</summary>
[Serializable]
public class PositionedTile
{
    public Vector3Int Position;
    public EBiomTileTypes Type;
    public ESpecialTiles SpecialType;
    private Random _random;

    public PositionedTile(Vector3Int Position, EBiomTileTypes Type)
    {
        this.Type = Type;
        this.Position = Position;
    }
    public PositionedTile(Vector3Int Position, ESpecialTiles Type)
    {
        this.SpecialType = Type;
        this.Position = Position;
    }
}