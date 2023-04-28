using System;
using System.Collections.Generic;

namespace WorldGeneration
{
    public static class Directions
    {
        public enum AllDirections
        { 
            NorthWest, North, NorthEast, East, SouthEast, South, SouthWest, West
        }

        public static readonly AllDirections[] BaseDirections = new AllDirections[]{ AllDirections.North, AllDirections.East, AllDirections.South, AllDirections.West };
    }
    
}