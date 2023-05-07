using System.Linq;

namespace WorldGeneration.TileScripts
{
    public class BeachTile : LandTile
    {
        public const TileTypes Type = TileTypes.Beach;

        public new static bool CheckRule(TileTypes tile, int pos, Map map)
        {
            LandTile.CheckRule(tile, pos, map);
            return !(map.GetNeighboursByCondition(pos, (t, p, m) => t == TileTypes.Mountain).Any());
        }
    }
}