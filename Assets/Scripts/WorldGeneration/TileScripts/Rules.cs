using System.Linq;

namespace WorldGeneration.TileScripts
{
    public class Rules
    {
        private readonly TileRule _baseRule;
        private readonly TileRule _landTileRule;

        //Biom
        public readonly TileRule BeachTileRule;
        public readonly TileRule GrasTileRule;
        public readonly TileRule MountainTileRule;

        //Farmables
        public readonly TileRule WoodTileRule;
        public readonly TileRule StoneTileRule;
        public readonly TileRule OreTileRule;

        //Decoration
        public readonly TileRule FlowerTileRule;

        //Shape
        public readonly TileRule MidTileRule;
        public readonly TileRule NW_TileRule;
        public readonly TileRule N_TileRule;
        public readonly TileRule NE_TileRule;

        public Rules(Map map)
        {
            _baseRule = new TileRule(
                (pos) => map.IsOnMap(pos));

            _landTileRule = new(
                (pos) =>
                {
                    int landBoarderDistance = 4;
                    return _baseRule.Check(pos) && !map.IsToCloseToBoarder(pos, landBoarderDistance);
                });

            BeachTileRule = new(
                (pos) =>
                {
                    return _landTileRule.Check(pos) &&
                           !(map.GetNeighboursByRule(pos, (p) => map.BiomTileTypeMap[p] == (int)EBiomTileTypes.Mountain)
                               .Any());
                }
            );

            GrasTileRule = new(
                (pos) =>
                {
                    return _landTileRule.Check(pos) &&
                           !(map.GetNeighboursByRule(pos, (p) => map.BiomTileTypeMap[p] == (int)EBiomTileTypes.Sea)
                               .Any());
                }
            );

            MountainTileRule = new(
                (pos) =>
                {
                    return _landTileRule.Check(pos) && !(map
                        .GetNeighboursByRule(pos,
                            (p) => map.BiomTileTypeMap[p] is (int)EBiomTileTypes.Sea or (int)EBiomTileTypes.Beach)
                        .Any());
                }
            );
            
            //Farmable
            WoodTileRule = new TileRule(
                (pos) =>
                {
                    return _baseRule.Check(pos) && map.BiomTileTypeMap[pos] == (int)EBiomTileTypes.Gras &&
                           map.StackedMap[(int)TileType.Deco][pos] == 0 &&
                           map.StackedMap[(int)TileType.Farmable][pos] == 0;
                });

            StoneTileRule = new TileRule(
                (pos) =>
                {
                    return _baseRule.Check(pos) && map.BiomTileTypeMap[pos] == (int)EBiomTileTypes.Mountain &&
                           map.StackedMap[(int)TileType.Mountain][pos] == 0;
                });

            OreTileRule = new TileRule(
                (pos) =>
                {
                    return _baseRule.Check(pos) && map.BiomTileTypeMap[pos] == (int)EBiomTileTypes.Mountain &&
                           map.StackedMap[(int)TileType.Mountain][pos] == 0;
                });

            //Decoration
            FlowerTileRule = new TileRule(
                (pos) =>
                {
                    return _baseRule.Check(pos) && map.BiomTileTypeMap[pos] == (int)EBiomTileTypes.Gras &&
                           map.StackedMap[(int)TileType.Deco][pos] == 0 &&
                           map.StackedMap[(int)TileType.Farmable][pos] == 0;
                });

            //Shape Rules
            MidTileRule = new TileRule(
                (pos) =>
                {
                    return _baseRule.Check(pos) && map
                        .GetNeighboursByRule(pos, (p) => map.BiomTileTypeMap[pos] == map.BiomTileTypeMap[p])
                        .Count == AllDirections.BaseDirections.Length;
                });
        }

        //public static readonly ShapeTileRule NwRule = new ()
    }
}