using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using WorldGeneration;

[CreateAssetMenu(fileName = "New Level Tile", menuName = "2D/Tiles/Level Tile")]
public class MapTile : Tile
{
    public EBiomTileTypes Type;
    public ESpecialTiles SpecialType;
}