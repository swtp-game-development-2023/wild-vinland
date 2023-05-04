using System;
using System.IO;
using System.Linq;
using System.Text;

namespace WorldGeneration
{
    //TODO DOC comments
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
        private int[] _rawMap;

        /// <summary>
        /// the map with its different levels.
        /// </summary>
        //private int[][] _stackedMap;


        /// <summary>
        /// the minimum length a map can have at an edge. this number squared is the smallest possible map.
        /// </summary>
        public const int MinEdgeLength = 10;

        /// <summary>
        /// the maximum length a map can have at an edge. this number squared is the biggest possible map.
        /// </summary>
        public const int MaxEdgeLength = 64;

        public const int noTile = 0;


        public int MapSize => _mapSize;

        public int NumberOfLayers => _numberOfLayers;

        public int EdgeLength => _edgeLength;

        //TODO deep copys
        public int[] RawMap => _rawMap;

        //public int[][] StackedMap => _stackedMap;



        /// <summary>
        /// Creates a new empty map.
        /// </summary>
        /// <param name="edgeLength">the edge length of the map, this number squared is the total size of the map.</param>
        /// <param name="numberOfLayers">the number of different levels on which tiles can be located.</param>
        public Map(int edgeLength, int numberOfLayers)
        {
            if (edgeLength < MinEdgeLength || MaxEdgeLength < edgeLength)
                throw new ArgumentOutOfRangeException(nameof(_mapSize), edgeLength,
                    "Map Size have to be between " + MinEdgeLength + " and " + MaxEdgeLength);
            if (numberOfLayers <= 0)
                throw new ArgumentException("The value have to be at least 1", nameof(numberOfLayers));
            _edgeLength = edgeLength;
            _numberOfLayers = numberOfLayers;
            _mapSize = _edgeLength * _edgeLength;
            _rawMap = new int[_mapSize];
            //TODO _stackedMap = new int[_mapSize][];
        }

        /// <summary>
        /// Checks if a position for a tile is already occupied by something.
        /// </summary>
        /// <param name="pos">Position to be checked.</param>
        /// <param name="layer">Level at which is tested.</param>
        /// <returns>True if the position is not yet occupied.</returns>
        private bool IsTileEmpty(int pos, int layer)
        {
            return _stackedMap[pos][layer] == noTile;
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
            for (int i = 0; i < _rawMap.Length; i++)
            {
                stringBuilder.Append((IsLand(RawMap[i]) ? "[L]" : "[W]"));
                if ((i % _edgeLength) == (_edgeLength - 1)) stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
        //TODO check upper and lower neighbour

        //return -1 if there is no neighbour

        /// <summary>
        /// Searches the position of the specified neighbor.
        /// </summary>
        /// <param name="pos">The position from which to search for the neighbor.</param>
        /// <param name="direction">The direction in which to look for the neighbor</param>
        /// <returns>Returns the position of the neighbor, if there is no neighbor it returns -1.</returns>
        public int GetTileNeighbourPos(int pos, Directions.AllDirections direction)
        {
            int neighbourPos = direction switch
            {
                Directions.AllDirections.West => pos % (_edgeLength) != 0 ? pos - 1 : -1,
                Directions.AllDirections.North => pos - _edgeLength,
                Directions.AllDirections.South => pos + _edgeLength,
                Directions.AllDirections.East => (pos + 1) % _edgeLength != 0 ? pos + 1 : -1,
                Directions.AllDirections.NorthEast => GetTileNeighbourPos(
                    GetTileNeighbourPos(pos, Directions.AllDirections.North), Directions.AllDirections.East),
                Directions.AllDirections.NorthWest => GetTileNeighbourPos(
                    GetTileNeighbourPos(pos, Directions.AllDirections.North), Directions.AllDirections.West),
                Directions.AllDirections.SouthEast => GetTileNeighbourPos(
                    GetTileNeighbourPos(pos, Directions.AllDirections.South), Directions.AllDirections.East),
                Directions.AllDirections.SouthWest => GetTileNeighbourPos(
                    GetTileNeighbourPos(pos, Directions.AllDirections.South), Directions.AllDirections.West),
                _ => throw new ArgumentException("Invalid direction")
            };
            return IsOnMap(neighbourPos) ? neighbourPos : -1;
        }


        /// <param name="tile">The tile to be checked</param>
        /// <returns>True, if it is not a sea tile.</returns>
        public bool IsLand(int tile)
        {
            return tile != (int)TileTypes.Sea;
        }

        /// <summary>
        /// Counts the number of tiles of a certain type.
        /// </summary>
        /// <param name="map">Map on which to count.</param>
        /// <param name="types">Type to be checked.</param>
        /// <returns>The number of tiles to which the condition applies.</returns>
        public int countTiles(int[] map, TileTypes types)
        {
            int counter = 0;
            foreach (var tile in map)
            {
                if (tile == (int)types) counter++;
            }

            return counter;
        }

        /// <summary>
        /// Counts the distance of a position to the edge in a given direction.
        /// </summary>
        /// <param name="pos">Position from where to count.</param>
        /// <param name="direction">Direction in which to count.</param>
        /// <returns>the number of tiles up to the edge.</returns>
        public int GetBoarderDistance(int pos, Directions.AllDirections direction)
        {
            int counter = 0;
            while (IsOnMap(pos = GetTileNeighbourPos(pos, direction)))
            {
                counter++;
            }

            return counter;
        }

        /// <param name="pos">Position to be checked.</param>
        /// <returns>True if the position is still on the map.</returns>
        public bool IsOnMap(int pos)
        {
            return 0 <= pos && pos < _mapSize;
        }
    }
}