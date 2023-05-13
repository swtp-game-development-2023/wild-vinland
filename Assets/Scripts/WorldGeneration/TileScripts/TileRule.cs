using System;
using UnityEngine;

namespace WorldGeneration.TileScripts
{
    public abstract class TileRule
    {
 
        public readonly Func<int, int, Map, bool> Check;
        
        protected TileRule(Func<int, int, Map, bool> check)
        {
            Check = (tileType, pos, map) =>
            {
                if (!map.IsOnMap(pos)) throw new ArgumentException("Position is outside of the map");
                return check(tileType, pos, map);
            };
        }
    }
}