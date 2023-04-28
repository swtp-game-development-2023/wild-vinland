using System;
using System.IO;
using System.Linq;
using System.Text;

namespace WorldGeneration
{
    //TODO DOC comments
    public class Map
    {
        private readonly int _mapSize;
        private readonly int _numberOfLayers;
        private readonly int _edgeLength;
        private bool[] _isLandMap;
        private int[][] _stackedMap;
        public const int MinEdgeLength = 10;
        public const int MaxEdgeLength = 64;

        public int MapSize => _mapSize;

        public int NumberOfLayers => _numberOfLayers;

        public int EdgeLength => _edgeLength;

        //TODO deep copys
        public bool[] IsLandMap => _isLandMap;

        public int[][] StackedMap => _stackedMap;

        private const int noTile = 0;

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
            _isLandMap = new bool[_mapSize];
            //TODO _stackedMap = new int[_mapSize][];
        }

        private bool IsTileEmpty(int pos, int layer)
        {
            return _stackedMap[pos][layer] == noTile;
        }

        public int getMapMidPos()
        {
            int edgeDistance = (int)Math.Round(_edgeLength * 0.5f);
            return (_edgeLength - 1) * edgeDistance;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _isLandMap.Length; i++)
            {
                stringBuilder.Append((_isLandMap[i] ? "[L]" : "[W]"));
                if ((i % _edgeLength) == (_edgeLength - 1)) stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
        //TODO check upper and lower neighbour

        //return -1 if there is no neighbour
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
        
        public int GetBoarderDistance (int pos, Directions.AllDirections direction)
        {
            int counter = 0;
            while (IsOnMap( pos = GetTileNeighbourPos(pos, direction)))
            {
                counter++;
            }

            return counter;
        }
        
        public bool IsOnMap(int pos)
        {
            return 0 <= pos && pos < _mapSize;
        }
    }
}