using System;
using UnityEngine;

namespace WorldGeneration.TileScripts
{
    public abstract class Tile
    {
        protected static bool CheckRule(TileTypes tile, int pos, Map map)
        {
            if (!map.IsOnMap(pos)) throw new ArgumentException("Position is outside of the map");
            return true;
        }
    }
}