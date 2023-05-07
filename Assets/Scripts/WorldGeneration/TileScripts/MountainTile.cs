using System.Linq;

namespace WorldGeneration.TileScripts
{
    public abstract class MountainTile : LandTile
    {
        public const TileTypes Type = TileTypes.Mountain;
        public new static bool CheckRule(TileTypes tileType, int pos, Map map)
        {
            LandTile.CheckRule(tileType, pos, map);
            return !(map.GetNeighboursByCondition(pos, (t, p, m) => t is TileTypes.Sea or TileTypes.Beach).Any());
        }
    }
}