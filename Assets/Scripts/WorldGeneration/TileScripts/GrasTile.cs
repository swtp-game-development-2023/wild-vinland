using System.Linq;
using WorldGeneration;
using WorldGeneration.TileScripts;

public abstract class GrasTile : LandTile
{
    public const TileTypes Type = TileTypes.Gras;
    public new static bool CheckRule(TileTypes tile, int pos, Map map)
    {
        LandTile.CheckRule(tile, pos, map);
        return !(map.GetNeighboursByCondition(pos, (t, p, m) => t == TileTypes.Sea).Any());
    }
}