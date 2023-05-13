using System;

namespace WorldGeneration.TileScripts
{
    public class BiomTileRule : TileRule
    {
        public BiomTileRule(Func<int, int, Map, bool> check)
            : base((tileType, pos, map) =>
            {
                if (!Enum.IsDefined(typeof(BiomTileTypes), tileType))
                    throw new ArgumentException("Value for TerroirTile is not valid");
                return check(tileType, pos, map);
            })
        {
        }
    }
}