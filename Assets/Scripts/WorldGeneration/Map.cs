using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldGeneration.TileScripts;

namespace WorldGeneration
{
    public class Map
    {
        /// <summary>
        /// The number of all tiles on a map on one level.
        /// Is always a square number, because the map can only be square.
        /// </summary>
        private readonly int _mapSize;

        /// <summary>
        /// The number of different levels on which tiles can be located.
        /// </summary>
        private readonly int _numberOfLayers;

        /// <summary>
        /// The length of one side of the map, this number squared gives the map size.
        /// </summary>
        private readonly int _edgeLength;

        /// <summary>
        /// A raw state of the map where basic positions are stored first. 
        /// </summary>
        private int[] _biomTileTypeMap;


        /// <summary>
        /// the map with its different levels.
        /// </summary>
        private int[][] _stackedMap;


        /// <summary>
        /// the minimum length a map can have at an edge. this number squared is the smallest possible map.
        /// </summary>
        public const int MinEdgeLength = 32;

        /// <summary>
        /// the maximum length a map can have at an edge. this number squared is the biggest possible map.
        /// </summary>
        public const int MaxEdgeLength = 128;

        public const int NoTile = 0;


        public int MapSize => _mapSize;

        public int NumberOfLayers => _numberOfLayers;

        public int EdgeLength => _edgeLength;
        
        public int[] BiomTileTypeMap => _biomTileTypeMap;

        //public int[] ShapeTileTypeMap => _shapeTileTypeMap;
        public int[][] StackedMap => _stackedMap;


        /// <summary>
        /// Creates a new empty map.
        /// </summary>
        /// <param name="edgeLength">the edge length of the map, this number squared is the total size of the map.</param>
        public Map(int edgeLength)
        {
            if (edgeLength < MinEdgeLength || MaxEdgeLength < edgeLength)
                throw new ArgumentOutOfRangeException(nameof(_mapSize), edgeLength,
                    "Map Size have to be between " + MinEdgeLength + " and " + MaxEdgeLength);
            _edgeLength = edgeLength;
            _numberOfLayers = Enum.GetNames(typeof(EBiomTileTypes)).Length;
            _mapSize = _edgeLength * _edgeLength;
            _biomTileTypeMap = new int[_mapSize];
            InitStackedMap();
        }

        private Map InitStackedMap()
        {
            _stackedMap = new int[_numberOfLayers][];
            for (int i = 0; i < _numberOfLayers; i++)
            {
                _stackedMap[i] = new int[_mapSize];
            }

            return this;
        }

        /// <summary>
        /// Checks if a position for a tile is already occupied by something.
        /// </summary>
        /// <param name="pos">Position to be checked.</param>
        /// <param name="layer">Level at which is tested.</param>
        /// <returns>True if the position is not yet occupied.</returns>
        private bool IsTileEmpty(int pos, int layer)
        {
            return _stackedMap[pos][layer] == NoTile;
        }

        /// <summary>
        /// Determines the center of the map.
        /// </summary>
        /// <returns>the center of a square map.</returns>
        public int getMapMidPos()
        {
            int edgeDistance = (int)Math.Round(_edgeLength * 0.5f);
            return (_edgeLength - 1) * edgeDistance;
        }


        /// <returns>outputs the map as a string</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _biomTileTypeMap.Length; i++)
            {
                switch (BiomTileTypeMap[i])
                {
                    case (int)EBiomTileTypes.Sea:
                        stringBuilder.Append("[W]");
                        break;
                    case (int)EBiomTileTypes.Beach:
                        stringBuilder.Append("[ B ]");
                        break;
                    case (int)EBiomTileTypes.Grass:
                        stringBuilder.Append("[ G ]");
                        break;
                    case (int)EBiomTileTypes.Mountain:
                        stringBuilder.Append("[ M ]");
                        break;
                }

                if ((i % _edgeLength) == (_edgeLength - 1)) stringBuilder.Append("\n");
            }

            stringBuilder.Append("\n\n\n");
            for (int i = 0; i < _mapSize; i++)
            {
                switch (StackedMap[(int)EBiomTileTypes.Farmable][i])
                {
                    case (int)EGameObjectType.Tree:
                        stringBuilder.Append("[ T ]");
                        break;
                    case (int)EGameObjectType.Ore:
                        stringBuilder.Append("[ O ]");
                        break;
                    case (int)EGameObjectType.Stone01:
                        stringBuilder.Append("[ S ]");
                        break;
                    default:
                        stringBuilder.Append(StackedMap[(int)EBiomTileTypes.Decoration][i] != 0 ? "[ F ]" : "[   ]");
                        break;
                }

                if ((i % _edgeLength) == (_edgeLength - 1)) stringBuilder.Append("\n");
            }


            return stringBuilder.ToString();
        }
        
        /// <summary>
        /// Searches the position of the specified neighbor.
        /// </summary>
        /// <param name="pos">The position from which to search for the neighbor.</param>
        /// <param name="direction">The direction in which to look for the neighbor</param>
        /// <returns>Returns the position of the neighbor, if there is no neighbor it returns -1.</returns>
        public int GetTileNeighbourPos(int pos, AllDirections.Directions direction)
        {
            int neighbourPos = direction switch
            {
                AllDirections.Directions.West => pos % (_edgeLength) != 0 ? pos - 1 : -1,
                AllDirections.Directions.North => pos - _edgeLength,
                AllDirections.Directions.South => pos + _edgeLength,
                AllDirections.Directions.East => (pos + 1) % _edgeLength != 0 ? pos + 1 : -1,
                AllDirections.Directions.NorthEast => GetTileNeighbourPos(
                    GetTileNeighbourPos(pos, AllDirections.Directions.North), AllDirections.Directions.East),
                AllDirections.Directions.NorthWest => GetTileNeighbourPos(
                    GetTileNeighbourPos(pos, AllDirections.Directions.North), AllDirections.Directions.West),
                AllDirections.Directions.SouthEast => GetTileNeighbourPos(
                    GetTileNeighbourPos(pos, AllDirections.Directions.South), AllDirections.Directions.East),
                AllDirections.Directions.SouthWest => GetTileNeighbourPos(
                    GetTileNeighbourPos(pos, AllDirections.Directions.South), AllDirections.Directions.West),
                _ => throw new ArgumentException("Invalid direction")
            };
            return IsOnMap(neighbourPos) ? neighbourPos : -1;
        }

        /// <summary>
        /// Returns a hashset of positions of all neighbours by a condition of a given position on the map.
        /// </summary>
        /// <param name="pos">Position whose neighbors are to be checked.</param>
        /// <param name="ruleCheck">A function that determines whether a given neighbour is allowed or not.</param>
        /// <returns>Set of position of all neighbors on which a tile may be positioned according to valid rules.</returns>
        public HashSet<int> GetNeighboursByRule(int pos, Func<int, bool> ruleCheck)
        {
            return Enumerable.ToHashSet(AllDirections.BaseDirections
                .ToList()
                .Select(direction => GetTileNeighbourPos(pos, direction))
                .Where(neighbourPos => ruleCheck(neighbourPos)));
        }

        /// <param name="tileType">The tile to be checked</param>
        /// <returns>True, if it is not a sea tile.</returns>
        public static bool IsLand(int tileType)
        {
            return tileType != (int)EBiomTileTypes.Sea;
        }

        /// <summary>
        /// Counts the number of tiles of a certain type.
        /// </summary>
        /// <param name="intMap">Map on which to count.</param>
        /// <param name="eBiomTileType">Type to be checked.</param>
        /// <returns>The number of tiles to which the condition applies.</returns>
        public static int CountTilesByType(int[] intMap, EBiomTileTypes eBiomTileType)
        {
            return intMap.Count(tiles => tiles == (int)eBiomTileType);
        }

        /// <summary>
        /// Counts the number of tiles in the given int map that match the specified tile rule.
        /// </summary>
        /// <param name="intMap">The int map to search for matching tiles.</param>
        /// <param name="rule">The tile rule to use for matching tiles.</param>
        /// <returns>The number of tiles in the int map that match the specified tile rule.</returns>
        public static int CountTilesByRule(int[] intMap, TileRule rule)
        {
            int counter = 0;

            Enumerable.Range(0, intMap.Length)
                .ToList()
                .ForEach((pos) =>
                {
                    if (rule.Check(pos))
                    {
                        counter++;
                    }
                });

            return counter;
        }

        /// <summary>
        /// Counts the distance of a position to the edge in a given direction.
        /// </summary>
        /// <param name="pos">Position from where to count.</param>
        /// <param name="direction">Direction in which to count.</param>
        /// <returns>the number of tiles up to the edge.</returns>
        private int GetBoarderDistance(int pos, AllDirections.Directions direction)
        {
            int counter = 0;
            while (IsOnMap(pos = GetTileNeighbourPos(pos, direction)))
            {
                counter++;
            }

            return counter;
        }

        /// <summary>
        /// Checks if a position is close to the border.
        /// </summary>
        /// <param name="pos">Position to be checked.</param>
        /// <param name="toleratedDistance">The allowed distance to the border.</param>
        /// <returns>True if the position is too close to the border.</returns>
        public bool IsToCloseToBoarder(int pos, int toleratedDistance)
        {
            return AllDirections.BaseDirections.ToList()
                .Select(direction => GetBoarderDistance(pos, direction))
                .Any(distance => distance < toleratedDistance);
        }

        /// <param name="pos">Position to be checked.</param>
        /// <returns>True if the position is still on the map.</returns>
        public bool IsOnMap(int pos)
        {
            return 0 <= pos && pos < _mapSize;
        }
    }
}