using System.Linq;

namespace WorldGeneration.TileScripts
{
    public abstract class MountainTile : LandTile
    {
        public const TileTypes Type = TileTypes.Mountain;
        public new static bool CheckRule(TileTypes tile, int pos, Map map)
        {
            LandTile.CheckRule(tile, pos, map);
            LandTile.CheckRule(tile, pos, map);
            return !(map.GetNeighboursByCondition(pos, (t, p, m) => t is TileTypes.Sea or TileTypes.Beach).Any());
        }
    }
}