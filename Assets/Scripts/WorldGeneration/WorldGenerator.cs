using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldGeneration;
using WorldGeneration.TileScripts;
using Random = System.Random;

public class WorldGenerator : MonoBehaviour
{
    
    /// <summary>
    /// The map on which the game world is generated.
    /// </summary>
    private Map _map;

    /// <summary>
    /// The random world generation generator.
    /// </summary>
    private Random _random;


    private Rules _rules;
    private int _start;
    //private const float _defaultLandProbability = 0.3f;

    /// <summary>
    /// Each piece of land is beach first.
    /// </summary>
    private const int Land = (int)BiomTileTypes.Beach;

    //TODO add parameterizability of probabilities for diffrent tiles
    /// <summary>
    /// generates a new map when called.
    /// </summary>
    /// <param name="edgeLength">Edges length of the new map, this number squared is the total size of the map.</param>
    /// <param name="seed">A seed to generate the map.</param>
    public void GenerateMap(int edgeLength, int seed)
    {
        _map = new Map(edgeLength, Enum.GetNames(typeof(BiomTileTypes)).Length);
        _random = new Random(seed);
        _rules = new Rules(_map);
    }

    /// <summary>
    /// generates a new map when called.
    /// </summary>
    /// <param name="edgeLength"> Edges length of the new map, this number squared is the total size of the map.</param>
    public void GenerateMap(int edgeLength)
    {
        GenerateMap(edgeLength, (int)DateTime.Now.Ticks);
    }

    /// <summary>
    /// Set the first land tile close to mid of the map.
    /// </summary>
    /// <param name="map">Map on which the tile should be placed.</param>
    /// <param name="permittedDeviation">The allowed random deviation from the center.
    /// WARNING keep it small if you do not want the island to be pressed too close to the edge.</param>
    /// <returns>The position of the first land tile</returns>
    private int StartPoint(Map map, float permittedDeviation = 0.05f)
    {
        if (!IsPercentage(permittedDeviation))
            throw new ArgumentException("Argument has to be in % between 0 and 1", nameof(permittedDeviation));
        int permittedRange = (int)Math.Ceiling(map.EdgeLength * permittedDeviation);
        int minDistance = (map.EdgeLength / 2) - permittedRange; //min border distance in each direction
        List<int> possibleStartPos = Enumerable.Range(0, map.MapSize)
            .Where(pos => !map.IsToCloseToBoarder(pos, minDistance))
            .ToList(); //searches all possible start points that have the required distance from the edge
        int starPoint = possibleStartPos[_random.Next(0, possibleStartPos.Count)];
        map.BiomTileTypeMap[starPoint] = Land;
        return starPoint;
    }


    /// <summary>
    /// Determines whether a given float value represents a percentage.
    /// </summary>
    /// <param name="f">The float value to check.</param>
    /// <returns>True if the value is between 0 and 1 inclusive, indicating a percentage; otherwise, false.</returns>
    private static bool IsPercentage(float f)
    {
        return f is >= 0 and <= 1;
    }

    /// <summary>
    /// Determines whether an event with a given probability will happen.
    /// </summary>
    /// <param name="probability">The probability of the event happening, represented as a percentage between 0 and 1.</param>
    /// <returns>True if the event happens, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown if the probability argument is not between 0 and 1, inclusive.</exception>
    private bool WillEventHappen(float probability)
    {
        if (!IsPercentage(probability))
            throw new ArgumentException("Argument has to be in % between 0 and 1", nameof(probability));
        return _random.NextDouble() < probability;
    }


    /// <summary>
    /// Generates land tiles on the given map until a desired percentage of land is reached.
    /// </summary>
    /// <param name="map">The map on which to generate land tiles.</param>
    /// <param name="percentOfLand">The desired percentage of land tiles, represented as a float value between 0 and 1.</param>
    /// <param name="smoothnessOfCoast">The desired percentage of how rugged the island should be, represented as a float value between 0 and 1.
    /// Larger values make for smoother coasts. Smaller ones for more natural ones.</param>
    /// <exception cref="ArgumentException">Thrown when the percentOfLand argument is not between 0 and 1.</exception>
    /// <returns>The current instance of the <see cref="WorldGenerator"/> class.</returns>
    private WorldGenerator GenerateLand(Map map, float percentOfLand = 0.45f, float smoothnessOfCoast = 0.3f)
    {
        if (!IsPercentage(percentOfLand))
            throw new ArgumentException("Argument has to be in % between 0 and 1", nameof(percentOfLand));
        if (!IsPercentage(percentOfLand))
            throw new ArgumentException("Argument has to be in % between 0 and 1", nameof(smoothnessOfCoast));
        
        int requestedTileCount = (int)(percentOfLand * map.MapSize);

        int maxPossibleTiles = map.CountTilesByRule(map.BiomTileTypeMap, _rules.BeachTileRule);
        _start = StartPoint(map);
        while (Map.CountTilesByType(map.BiomTileTypeMap, BiomTileTypes.Beach) < maxPossibleTiles &&
               Map.CountTilesByType(map.BiomTileTypeMap, BiomTileTypes.Beach) < requestedTileCount)
        {
            Enumerable.Range(0, map.MapSize)
                .Where(pos => Map.IsLand(map.BiomTileTypeMap[pos]))
                .SelectMany(pos => map.GetNeighboursByRule(pos, _rules.BeachTileRule.Check))
                .Where(n => WillEventHappen(smoothnessOfCoast))
                .ToList()
                .ForEach(pos => PlaceTile(pos, map.BiomTileTypeMap, (BiomTileTypes)Land));
        }

        return this;
    }

    /// <summary>
    /// Generates landscape features on the given map, such as mountains and grass, based on the provided percentage of mountain parameter.
    /// </summary>
    /// <param name="map">The map on which to generate landscape features.</param>
    /// <param name="percentageOfMountain">The desired percentage of mountain tiles, represented as a float value between 0 and 1.</param>
    /// <exception cref="ArgumentException">Thrown when the percentageOfMountain argument is not between 0 and 1.</exception>
    /// <returns>The current instance of the <see cref="WorldGenerator"/> class.</returns>
    private WorldGenerator GenerateLandScape(Map map, float percentageOfMountain = 0.15f )
    {
        if (!IsPercentage(percentageOfMountain))
            throw new ArgumentException("Argument has to be in % between 0 and 1", nameof(percentageOfMountain));
        
        float smoothnessOfMountain = 0.1f;
        int requestedTileCount = (int)(percentageOfMountain * Map.CountTilesByType(map.BiomTileTypeMap, (BiomTileTypes)Land));
        int maxPossibleTiles = Map.CountTilesByType(map.BiomTileTypeMap, (BiomTileTypes)Land);
        
        //Grass
        Enumerable.Range(0, map.MapSize)
            .Where(pos => Map.IsLand(map.BiomTileTypeMap[pos]))
            .SelectMany(pos => map.GetNeighboursByRule(pos, _rules.GrasTileRule.Check))
            .ToList()
            .ForEach(pos => PlaceTile(pos, map.BiomTileTypeMap, BiomTileTypes.Gras));

        //Mountain
        PlaceTile(_start, map.BiomTileTypeMap, BiomTileTypes.Mountain);
        while (Map.CountTilesByType(map.BiomTileTypeMap, BiomTileTypes.Mountain) < maxPossibleTiles &&
               Map.CountTilesByType(map.BiomTileTypeMap, BiomTileTypes.Mountain) < requestedTileCount)
        {
            Enumerable.Range(0, map.MapSize)
                .Where(pos => map.BiomTileTypeMap[pos] == (int)BiomTileTypes.Mountain)
                .SelectMany(pos => map.GetNeighboursByRule(pos, _rules.MountainTileRule.Check))
                .Where(n => WillEventHappen(smoothnessOfMountain))
                .ToList()
                .ForEach(pos => PlaceTile(pos, map.BiomTileTypeMap, BiomTileTypes.Mountain));
        }
        return this;
    }

    /// <summary>
    /// Places a tile of the given type at the specified position in the given map.
    /// </summary>
    /// <param name="pos">The position at which to place the tile.</param>
    /// <param name="map">The map on which to place the tile.</param>
    /// <param name="type">The type of tile to place.</param>
    /// <returns>The current instance of the <see cref="WorldGenerator"/> class.</returns>
    private WorldGenerator PlaceTile(int pos, int[] map, BiomTileTypes type)
    {
        map[pos] = (int)type;
        return this;
    }

    //just for testing
    public void OnTestGenerate()
    {
        GenerateMap(Map.MaxEdgeLength);
        /*var startPos = StartPoint(_map);
        print("start " + startPos);
        foreach (var placeableNeighbour in GetPlaceableNeighbours(startPos, _map, _landTileRule))
        {
            _map.RawMap[placeableNeighbour] = land;
        }*/
        print(_map.ToString());
        GenerateLand(_map);
        print(_map.ToString());
        GenerateLandScape(_map);
        print(_map.ToString());
    }
}