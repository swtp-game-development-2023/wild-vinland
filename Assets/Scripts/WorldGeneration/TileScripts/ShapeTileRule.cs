using System;

namespace WorldGeneration.TileScripts
{
    public class ShapeTileRule : TileRule
    {
        ShapeTileRule(Func<int, int, Map, bool> check)
            : base((tileType, pos, map) =>
            {
                if (!Enum.IsDefined(typeof(ShapeTileTypes), tileType))
                    throw new ArgumentException("Value for ShapeTileTypes is not valid");
                return check(tileType, pos, map);
            })
        {
        }
    }
}