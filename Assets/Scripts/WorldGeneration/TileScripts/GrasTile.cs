using System.Linq;
using WorldGeneration;
using WorldGeneration.TileScripts;

public abstract class GrasTile : LandTile
{
    public const TileTypes Type = TileTypes.Gras;
    public new static bool CheckRule(TileTypes tileType, int pos, Map map)
    {
        return LandTile.CheckRule(tileType, pos, map) && !(map.GetNeighboursByCondition(pos, (t, p, m) => t == TileTypes.Sea).Any());
    }
}