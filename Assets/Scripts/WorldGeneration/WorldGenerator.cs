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

    /// <summary>
    /// The map on which the game world is generated.
    /// </summary>
    private Map _map;

    /// <summary>
    /// The random world generation generator.
    /// </summary>
    private Random _random;
    //private const float _defaultLandProbability = 0.3f;

    /// <summary>
    /// Each piece of land is beach first.
    /// </summary>
    private const int land = (int)TileTypes.Beach;

    //TODO Move in a seperate class
    /// <summary>
    /// The rules for placing a land tile.
    /// </summary>
    /// <returns>True if a land tile may be placed.</returns>
    private readonly Func<int, Map, bool> _landTileRule = (pos, map) =>
    {
        int landBoarderDistance = 0;
        if (!map.IsOnMap(pos)) return false;
        if (map.IsLand(map.RawMap[pos]) || IsToCloseToBoarder(pos, map, landBoarderDistance)) return false;
        return true;
    };

    /// <summary>
    /// Checks if a position is close to the border.
    /// </summary>
    /// <param name="pos">Position to be checked.</param>
    /// <param name="map">Map on which to check.</param>
    /// <param name="toleratedDistance">The allowed distance to the border.</param>
    /// <returns>True if the position is too close to the border.</returns>
    private static bool IsToCloseToBoarder(int pos, Map map, int toleratedDistance)
    {
        return Directions.BaseDirections.ToList()
            .Select(direction => map.GetBoarderDistance(pos, direction))
            .Any(distance => distance < toleratedDistance);
    }


    //TODO add parameterizability of probabilities for diffrent tiles
    /// <summary>
    /// generates a new map when called.
    /// </summary>
    /// <param name="edgeLength">Edges length of the new map, this number squared is the total size of the map.</param>
    /// <param name="seed">A seed to generate the map.</param>
    public void GenerateMap(int edgeLength, int seed)
    {
        _map = new Map(edgeLength, Enum.GetNames(typeof(TileTypes)).Length);
        _random = new Random(seed);
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

        List<int> possibleStartPos = Enumerable.Range(0, map.MapSize)
            .Where(i => !IsToCloseToBoarder(i, map, (map.EdgeLength / 2) - permittedRange))
            .ToList(); //searches all possible start points that have the required distance from the edge
        int starPoint = possibleStartPos[_random.Next(0, possibleStartPos.Count)];
        map.RawMap[starPoint] = land;
        return starPoint;
    }


    /// <summary>
    /// Returns a hashset of positions of all placeable neighbours of a given position on the map.
    /// </summary>
    /// <param name="pos">Position whose neighbors are to be checked.</param>
    /// <param name="map">Map on which to check.</param>
    /// <param name="isAllowedNeighbor">A function that determines whether a given neighbour is allowed or not.</param>
    /// <returns>Set of position of all neighbors on which a tile may be positioned according to valid rules.</returns>
    private static HashSet<int> GetPlaceableNeighbours(int pos, Map map, Func<int, Map, bool> isAllowedNeighbor)
    {
        return Directions.BaseDirections
            .ToList()
            .Select(direction => map.GetTileNeighbourPos(pos, direction))
            .Where(neighbourPos => isAllowedNeighbor(neighbourPos, map))
            .ToHashSet();
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
    /// <exception cref="ArgumentException">Thrown when the percentOfLand argument is not between 0 and 1.</exception>
    /// <returns>The current instance of the <see cref="WorldGenerator"/> class.</returns>
    private WorldGenerator GenerateLand(Map map, float percentOfLand = 0.45f)
    {
        if (!IsPercentage(percentOfLand))
            throw new ArgumentException("Argument has to be in % between 0 and 1", nameof(probability));
        int maxLandTiles = (int)(percentOfLand * map.MapSize);
        int start = StartPoint(map);
        //TODO dont ignor minland tiles check the minimu
        while (map.countTiles(map.RawMap, TileTypes.Beach) < maxLandTiles)
        {
            Enumerable.Range(0, map.MapSize)
                .Where(pos => map.IsLand(map.RawMap[pos]))
                .SelectMany(pos => GetPlaceableNeighbours(pos, map, _landTileRule))
                .Where(n => WillEventHappen(0.3f)) //TODO remove magic number
                .ToList()
                .ForEach(pos => map.RawMap[pos] = land);
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
    private WorldGenerator PlaceTile(int pos, int[] map, TileTypes type)
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
    }
}