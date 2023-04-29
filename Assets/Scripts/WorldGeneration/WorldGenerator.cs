using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldGeneration;
using Random = System.Random;

public class WorldGenerator : MonoBehaviour
{
    //TODO DOC comments
    //TODO maybe tile rule visiter-pattern

    private Map _map;

    private Random _random;
    //private const float _defaultLandProbabilitie = 0.3f;

    private readonly Func<int, Map, bool> _landTileRule = (pos, map) =>
    {
        int landBoarderDistance = 3;
        if (pos == -1) return false;

        if (!map.IsLand(map.RawMap[pos]) && IsToCloseToBoarder(pos, map, landBoarderDistance))
            return false;
        return true;
    };

    private static bool IsToCloseToBoarder(int pos, Map map, int toleratedDistance)
    {
        return Directions.BaseDirections.ToList()
            .Select(direction => map.GetBoarderDistance(pos, direction))
            .Any(distance => distance < toleratedDistance);
    }


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

    //set start point of the island close to mid of the map
    private int StartPoint(Map map, float permittedDeviation = 0.05f)
    {
        if (!IsPercentage(permittedDeviation))
            throw new ArgumentException("Argument has to be in % between 0.00 and 1", nameof(permittedDeviation));
        int permittedRange = (int)Math.Ceiling(map.EdgeLength * permittedDeviation);

        List<int> possibleStartPos = Enumerable.Range(0, map.MapSize)
            .Where(i => !IsToCloseToBoarder(i, map, (map.EdgeLength / 2) - permittedRange))
            .ToList(); //searches all possible start points that have the required distance from the edge
        int starPoint = possibleStartPos[_random.Next(0, possibleStartPos.Count)];
        map.RawMap[starPoint] = Map.land;
        return starPoint;
    }


    private static HashSet<int> GetPlaceableNeighbours(int pos, Map map, Func<int, Map, bool> isAllowedNeighbor)
    {
        return Directions.BaseDirections
            .ToList()
            .Select(direction => map.GetTileNeighbourPos(pos, direction))
            .Where(neighbourPos => isAllowedNeighbor(neighbourPos, map))
            .ToHashSet();
    }
    
    private static bool IsPercentage(float f)
    {
        return f is >= 0 and <= 100;
    }
    public void OnTestGenerate()
    {
        GenerateMap(Map.MinEdgeLength);
        var startPos = StartPoint(_map);
        print(_map.ToString());
        foreach (var placeableNeighbour in GetPlaceableNeighbours(startPos, _map, _landTileRule))
        {
            _map.RawMap[placeableNeighbour] = Map.land;
        }

        print(_map.ToString());
    }


}