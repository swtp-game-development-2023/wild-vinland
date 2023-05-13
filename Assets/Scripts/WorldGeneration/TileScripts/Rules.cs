using System.Linq;

namespace WorldGeneration.TileScripts
{
    public class Rules
    {
        public static readonly BiomTileRule LandTileRule = new(
            (biomTileType, pos, map) =>
            {
                int landBoarderDistance = 2;
                if (!map.IsOnMap(pos)) return false;
                if ( /*map.IsLand(map.RawMap[pos]) || */map.IsToCloseToBoarder(pos, landBoarderDistance)) return false;
                return true;
            }
        );

        public static readonly BiomTileRule BeachTileRule = new (
            (biomTileType, pos, map) =>
            {
                return LandTileRule.Check(biomTileType, pos, map) &&
                       !(map.GetNeighboursByRule(pos, (t, p, m) => t == (int) BiomTileTypes.Mountain).Any());
            }
        );

        public static readonly BiomTileRule GrasTileRule = new(
            (biomTileType, pos, map) =>
            {
                return LandTileRule.Check(biomTileType, pos, map) &&
                       !(map.GetNeighboursByRule(pos, (t, p, m) => t == (int) BiomTileTypes.Sea).Any());
            }
        );

        public static readonly BiomTileRule MountainTileRule = new(
            (biomTileType, pos, map) =>
            {
                return LandTileRule.Check(biomTileType, pos, map) && !(map
                    .GetNeighboursByRule(pos, (t, p, m) => t is (int) BiomTileTypes.Sea or  (int) BiomTileTypes.Beach)
                    .Any());
            }
        );
    }
}