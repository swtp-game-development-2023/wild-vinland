using System;
using System.Buffers.Text;
using Unity.VisualScripting;

namespace WorldGeneration.TileScripts
{
    public abstract class LandTile: Tile
    {
        public new static bool CheckRule(int tile, int pos, Map map)
        {
            Tile.CheckRule(tile, pos, map);
            int landBoarderDistance = 0;
            if (!map.IsOnMap(pos)) return false;
            if (map.IsLand(map.RawMap[pos]) || map.IsToCloseToBoarder(pos,  landBoarderDistance)) return false;
            return true;
        }
        
    }
}