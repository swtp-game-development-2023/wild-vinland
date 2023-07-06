using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using World;
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

    private Tilemap SeaMap, BeachMap, GrassMap, MountainMap, FarmableMap, DecoMap, UnitMap;

    private TilePlacer tilePlacer;

    private Rules _rules;

    private int _start;

    private const float MinPecent = 0f;
    private const float MaxPecent = 1f;

    [Range(Map.MinEdgeLength, Map.MaxEdgeLength)]
    public int edgeLength = 64;

    public bool useSeed = false;
    public bool generateWorldOnStart = true;
    public int seed;

    [Range(MinPecent, MaxPecent)] public float permittedDeviationFromMid = 0.05f;

    [Range(MinPecent, MaxPecent)] public float percentageOfMountain = 0.15f;

    [Range(MinPecent, MaxPecent)] public float percentOfLand = 0.45f;

    [Range(0.01f, MaxPecent)] public float smoothnessOfCoast = 0.3f;

    [Range(MinPecent, MaxPecent)] public float percentOfWood = 0.5f;

    [Range(MinPecent, MaxPecent)] public float percentOfStone = 0.5f;

    [Range(MinPecent, MaxPecent)] public float percentOfOre = 0.1f;

    [Range(MinPecent, MaxPecent)] public float percentOfFlowers = 0.5f;

    //ressources
    public GameObject[] treePrefab;
    public GameObject orePrefab;
    public GameObject[] stonePrefabs;


    /// <summary>
    /// Each piece of land is beach first.
    /// </summary>
    private const int Land = (int)EBiomTileTypes.Beach;

    void Start()
    {
        if (UIWorldGenDropBox.IsFilled)
        {
            generateWorldOnStart = UIWorldGenDropBox.GenOnStart;
            edgeLength = UIWorldGenDropBox.EdgeLength;
            percentOfWood = UIWorldGenDropBox.PercentOfWood;
            percentOfStone = UIWorldGenDropBox.PercentOfStone;
            percentOfOre = UIWorldGenDropBox.PercentOfOre;

            percentageOfMountain = UIWorldGenDropBox.PercentOfMountain;
            percentOfLand = UIWorldGenDropBox.PercentOfLand;
            percentOfFlowers = UIWorldGenDropBox.PercentOfFlowers;
            smoothnessOfCoast = UIWorldGenDropBox.SmoothnessOfCoast;

            useSeed = UIWorldGenDropBox.UseSeed;
            seed = UIWorldGenDropBox.Seed;
            UIWorldGenDropBox.IsFilled = false;
        }

        if (generateWorldOnStart)
        {
            Generate();
            // generation of first Map
        }
    }

    private bool isGenerated;

    private int framesAtGen;
    private void Update()
    {
        //TODO find a better solution, but it`s not stupid if it works!
        if (isGenerated && framesAtGen + 1 < Time.frameCount)
        {
            AstarPath.active.Scan();
            isGenerated = false;
        }
    }

    /// <summary>
    /// generates a new map when called.
    /// </summary>
    /// <param name="edgeLength">Edges length of the new map, this number squared is the total size of the map.</param>
    /// <param name="seed">A seed to generate the map.</param>
    public WorldGenerator GenerateMap(int edgeLength, int seed)
    {
        _map = new Map(edgeLength);
        _random = new Random(seed);
        _rules = new Rules(_map);
        return this;
    }

    /// <summary>
    /// generates a new map when called.
    /// </summary>
    /// <param name="edgeLength"> Edges length of the new map, this number squared is the total size of the map.</param>
    public WorldGenerator GenerateMap(int edgeLength)
    {
        return GenerateMap(edgeLength, (int)DateTime.Now.Ticks);
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
    private WorldGenerator GenerateLand(Map map, float percentOfLand = 0.45f, float smoothnessOfCoast = 0.3f,
        float permittedDeviation = 0.3f)
    {
        if (!IsPercentage(percentOfLand))
            throw new ArgumentException("Argument has to be in % between 0 and 1", nameof(percentOfLand));
        if (!IsPercentage(percentOfLand))
            throw new ArgumentException("Argument has to be in % between 0 and 1", nameof(smoothnessOfCoast));

        int requestedTileCount = (int)(percentOfLand * map.MapSize);

        int maxPossibleTiles = Map.CountTilesByRule(map.BiomTileTypeMap, _rules.BeachTileRule);
        _start = StartPoint(map, permittedDeviation);
        while (Map.CountTilesByType(map.BiomTileTypeMap, EBiomTileTypes.Beach) < maxPossibleTiles &&
               Map.CountTilesByType(map.BiomTileTypeMap, EBiomTileTypes.Beach) < requestedTileCount)
        {
            Enumerable.Range(0, map.MapSize)
                .Where(pos => Map.IsLand(map.BiomTileTypeMap[pos]))
                .SelectMany(pos => map.GetNeighboursByRule(pos, _rules.BeachTileRule.Check))
                .Where(n => WillEventHappen(smoothnessOfCoast))
                .ToList()
                .ForEach(pos => PlaceBiomTile(pos, map.BiomTileTypeMap, (EBiomTileTypes)Land));
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
    private WorldGenerator GenerateLandScape(Map map, float percentageOfMountain = 0.15f)
    {
        if (!IsPercentage(percentageOfMountain))
            throw new ArgumentException("Argument has to be in % between 0 and 1", nameof(percentageOfMountain));

        float smoothnessOfMountain = 0.1f;
        int requestedTileCount =
            (int)(percentageOfMountain * Map.CountTilesByType(map.BiomTileTypeMap, (EBiomTileTypes)Land));
        int maxPossibleMountainTiles;

        //Grass
        Enumerable.Range(0, map.MapSize)
            .Where(pos => Map.IsLand(map.BiomTileTypeMap[pos]))
            .SelectMany(pos => map.GetNeighboursByRule(pos, _rules.GrasTileRule.Check))
            .ToList()
            .ForEach(pos => PlaceBiomTile(pos, map.BiomTileTypeMap, EBiomTileTypes.Grass));

        //Mountain
        maxPossibleMountainTiles = Map.CountTilesByRule(map.BiomTileTypeMap, _rules.MountainTileRule);
        PlaceBiomTile(_start, map.BiomTileTypeMap, EBiomTileTypes.Mountain);
        while (Map.CountTilesByType(map.BiomTileTypeMap, EBiomTileTypes.Mountain) < maxPossibleMountainTiles &&
               Map.CountTilesByType(map.BiomTileTypeMap, EBiomTileTypes.Mountain) < requestedTileCount)
        {
            Enumerable.Range(0, map.MapSize)
                .Where(pos => map.BiomTileTypeMap[pos] == (int)EBiomTileTypes.Mountain)
                .SelectMany(pos => map.GetNeighboursByRule(pos, _rules.MountainTileRule.Check))
                .Where(n => WillEventHappen(smoothnessOfMountain))
                .ToList()
                .ForEach(pos => PlaceBiomTile(pos, map.BiomTileTypeMap, EBiomTileTypes.Mountain));
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
    private WorldGenerator PlaceBiomTile(int pos, int[] map, EBiomTileTypes type)
    {
        map[pos] = (int)type;
        return this;
    }

    private WorldGenerator SetSpecialTiles(Map map, float woodPercent = 0.5f, float stonePercent = 0.5f,
        float orePercent = 0.1f, float flowersPercent = 0.5f)
    {
        for (int i = 0; i < map.MapSize; i++)
        {
            //Wood
            SetTileByRuleAndProbability(i, (int)EGameObjectType.Tree, map.StackedMap[(int)EBiomTileTypes.Farmable],
                _rules.WoodTileRule.Check, woodPercent);
            //Stone
            SetTileByRuleAndProbability(i, (int)EGameObjectType.Stone01, map.StackedMap[(int)EBiomTileTypes.Farmable],
                _rules.StoneTileRule.Check, stonePercent);
            //Ore
            SetTileByRuleAndProbability(i, (int)EGameObjectType.Ore, map.StackedMap[(int)EBiomTileTypes.Farmable],
                _rules.OreTileRule.Check, orePercent);
            //Flower
            SetTileByRuleAndProbability(i, (int)ESpecialTiles.WildMeadow,
                map.StackedMap[(int)EBiomTileTypes.Decoration],
                _rules.FlowerTileRule.Check, flowersPercent);
        }

        return this;
    }

    private WorldGenerator SetTileByRuleAndProbability(int pos, int type, int[] map,
        Func<int, bool> ruleCheck,
        float probability)
    {
        if (ruleCheck(pos) && WillEventHappen(probability))
            map[pos] = type;
        return this;
    }

    private WorldGenerator SetupTileMaps()
    {
        SeaMap = transform.Find("Sea").gameObject.GetComponent<Tilemap>();
        BeachMap = transform.Find("Beach").gameObject.GetComponent<Tilemap>();
        GrassMap = transform.Find("Grass").gameObject.GetComponent<Tilemap>();
        MountainMap = transform.Find("Mountain").gameObject.GetComponent<Tilemap>();
        FarmableMap = transform.Find("Farmables").gameObject.GetComponent<Tilemap>();
        DecoMap = transform.Find("Deco").gameObject.GetComponent<Tilemap>();
        UnitMap = transform.Find("Buildings").gameObject.GetComponent<Tilemap>();
        return this;
    }

    private WorldGenerator SetTilesInUnity()
    {
        int x = 0;
        int y = 0;

        for (int i = 0; i < _map.MapSize; i++)
        {
            if (x == (_map.EdgeLength - 1))
            {
                y++;
            }

            x = (i % _map.EdgeLength);


            Vector3Int gridVector = new Vector3Int(x, y);
            Vector3 worldVector = gridToWorldVector(gridVector);
            tilePlacer.Place(_map.BiomTileTypeMap[i], gridVector);


            switch (_map.StackedMap[(int)EBiomTileTypes.Farmable][i])
            {
                case (int)EGameObjectType.Stone01:
                    Instantiate(stonePrefabs[_random.Next(0, stonePrefabs.Length)], worldVector, Quaternion.identity,
                        gameObject.transform.GetChild(4).transform);
                    break;
                case (int)EGameObjectType.Ore:
                    Instantiate(orePrefab, worldVector, Quaternion.identity,
                        gameObject.transform.GetChild(4).transform);
                    break;
                case (int)EGameObjectType.Tree:
                    Instantiate(treePrefab[_random.Next(0, treePrefab.Length)], worldVector, Quaternion.identity,
                        gameObject.transform.GetChild(4).transform);
                    break;
            }

            tilePlacer.PlaceDecoration(_map.StackedMap[(int)EBiomTileTypes.Decoration][i], gridVector);
        }

        return this;
    }

    private Vector3 gridToWorldVector(Vector3Int v)
    {
        return (gameObject.GetComponent<Grid>().GetCellCenterWorld(v)) -
               new Vector3(0f, 0.08f, 0); // 0.08 half the grid size (0.16)
    }

    /// <summary>
    /// Generates a new WorldMap
    /// </summary>
    public void Generate()
    {
        WorldHelper.ClearMap();
        WorldGenerator world = useSeed ? GenerateMap(edgeLength, seed) : GenerateMap(edgeLength);
        world.GenerateLand(_map, percentOfLand, smoothnessOfCoast, permittedDeviationFromMid)
            .GenerateLandScape(_map, percentageOfMountain)
            .SetSpecialTiles(_map, percentOfWood, percentOfStone, percentOfOre, percentOfFlowers)
            .SetupTileMaps();
        tilePlacer = new TilePlacer(SeaMap, BeachMap, GrassMap, MountainMap, DecoMap);
        SetTilesInUnity();
        WorldHelper.SetPlayerPosition(WorldHelper.GetRandomTileOfMap(BeachMap));
        
        //TODO ugly, but works:
        isGenerated = true;
        framesAtGen = Time.frameCount;
    }
}