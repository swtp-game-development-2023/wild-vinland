using System;
using UnityEngine;

namespace WorldGeneration.TileScripts
{
    public class TileRule
    {
 
        public readonly Func<int, bool> Check;
        
        public TileRule(Func<int, bool> check)
        {
            Check = check;
        }
    }
}