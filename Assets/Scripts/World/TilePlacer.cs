using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WorldGeneration;
using Random = System.Random;
using System;
public class TilePlacer
{
    private Tilemap SeaMap, BeachMap, GrassMap, MountainMap, DecoMap;
    static string tilePath = "World/MapTiles/";
    static TileBase seaTile = Resources.Load<TileBase>(tilePath + "MasterSimple_76");
    static TileBase grasTile = Resources.Load<TileBase>(tilePath + "MasterSimple_17");
    static TileBase mountainTile = Resources.Load<TileBase>(tilePath + "MasterSimple_149");
    static TileBase beachTile = Resources.Load<TileBase>(tilePath + "MasterSimple_154");
    static TileBase[] flowerTiles =
    {
        Resources.Load<TileBase>(tilePath + "MasterSimple_7"),
        Resources.Load<TileBase>(tilePath + "MasterSimple_8")
    };

    private Random _random;

    public TilePlacer(Tilemap SeaMap, Tilemap BeachMap, Tilemap GrassMap, Tilemap MountainMap, Tilemap DecoMap)
    {
        this.SeaMap = SeaMap;
        this.BeachMap = BeachMap;
        this.GrassMap = GrassMap;
        this.MountainMap = MountainMap;
        this.DecoMap = DecoMap;
        _random = new Random((int)DateTime.Now.Ticks);
    }

    public void Place(int tileType, Vector3Int pos)
    {
        switch (tileType)
        {
            case (int)EBiomTileTypes.Sea:
                SeaMap.SetTile(pos, seaTile);
                break;
            case (int)EBiomTileTypes.Beach:
                BeachMap.SetTile(pos, beachTile);
                break;
            case (int)EBiomTileTypes.Grass:
                GrassMap.SetTile(pos, grasTile);
                break;
            case (int)EBiomTileTypes.Mountain:
                MountainMap.SetTile(pos, mountainTile);
                break;
        }
        
    }

    public void PlaceDecoration(int tileType, Vector3Int pos)
    {
        switch (tileType)
        {
            case (int)ESpecialTiles.HighGrass:
                DecoMap.SetTile(pos, flowerTiles[0]);
                break;
            case (int)ESpecialTiles.Flowers:
                DecoMap.SetTile(pos, flowerTiles[1]);
                break;
            case (int)ESpecialTiles.WildMeadow:
                DecoMap.SetTile(pos, flowerTiles[_random.Next(0, flowerTiles.Length)]);
                break;
        }
        
    }
}
