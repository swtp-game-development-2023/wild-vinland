﻿using System.Linq;

namespace WorldGeneration.TileScripts
{
    public class BeachTile : LandTile
    {
        public const TileTypes Type = TileTypes.Beach;

        public new static bool CheckRule(TileTypes tileType, int pos, Map map)
        {
            LandTile.CheckRule(tileType, pos, map);
            return LandTile.CheckRule(tileType, pos, map) && !(map.GetNeighboursByCondition(pos, (t, p, m) => t == TileTypes.Mountain).Any());
        }
    }
}