using System;
using System.Collections.Generic;

namespace WorldGeneration
{
    /// <summary>
    /// directions in which a neighbor tile can lie
    /// </summary>
    public static class AllDirections
    {
        /// <summary>
        /// all directions in which a neighbor tile can lie
        /// </summary>
        public enum Directions
        {
            NorthWest, North, NorthEast, East, SouthEast, South, SouthWest, West
        }

        /// <summary>
        /// List of the four basic directions north, east, south, west.
        /// </summary>
        /// <value>A List List of the four basic directions</value>
        public static readonly Directions[] BaseDirections = { Directions.North, Directions.East, Directions.South, Directions.West };
    }

}