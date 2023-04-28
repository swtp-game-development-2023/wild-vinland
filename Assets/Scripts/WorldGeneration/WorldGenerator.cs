using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldGeneration;
using Random = System.Random;

public class WorldGenerator : MonoBehaviour
{
    
    //TODO DOC comments
    private Map _map;
    private Random _random;
    private const float _defaultLandProbabilitie = 0.3f;

    private Func<int, Map, bool> _landTileRule = (pos, map) =>
    {
        int landBoarderDistance = 3;
        if (pos == -1) return false;

        if (!map.IsLandMap[pos] && IsToCloseToBoarder(pos,map, landBoarderDistance))
            return false;
        return true;
    };

    private static bool IsToCloseToBoarder(int pos, Map map, int toleratedDistance)
    {
        return Directions.BaseDirections.ToList()
            .Select(direction => map.GetBoarderDistance(pos, direction))
            .Any(distance => distance < toleratedDistance);
    }

    
    //TODO maybe tile rule visiter-pattern
    //TODO add parameterizability of probabilities for diffrent tiles
    public void GenerateMap(int edgeLength, int seed)
    {
        _map = new Map(edgeLength, Enum.GetNames(typeof(ELayers)).Length);
        _random = new Random(seed);
    }
    
    public void GenerateMap(int mapSize)
    {
        GenerateMap(mapSize, (int)DateTime.Now.Ticks);
    }
    
    //set start point of the island at mid of the map
    private int StartPoint(Map map)
    {
        //TODO currently only x-axis random
        float permittedDeviation = 5f; // permitted deviation from the middle of the map in %
        int permittedRange = (int) Math.Round((map.MapSize / 100) * permittedDeviation);
        
        int minPos = map.getMapMidPos() - (permittedRange / 2);
        int maxPos = map.getMapMidPos() + (permittedRange / 2);
        int starPoint = _random.Next(minPos, maxPos);
        map.IsLandMap[starPoint] = true;
        
        return starPoint;
    }
    
    private HashSet<int> GetPlaceableNeighbours(int pos, Map map, Func<int, Map, bool> isAllowedNeighbor)
    {
        return Directions.BaseDirections
            .ToList()
            .Select(direction => map.GetTileNeighbourPos(pos, direction))
            .Where(neighbourPos => isAllowedNeighbor(neighbourPos, map))
            .ToHashSet();
    }
    
    public void OnTestGenerate()
    {
        GenerateMap(Map.MaxEdgeLength);
        var startPos = StartPoint(_map);
        print(_map.ToString());
        foreach (var placeableNeighbour in GetPlaceableNeighbours(startPos, _map, _landTileRule))
        {
            _map.IsLandMap[placeableNeighbour] = true;
        }
        print(_map.ToString());
    }
    
}