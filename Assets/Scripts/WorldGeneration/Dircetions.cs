using System;
using System.Collections.Generic;

namespace WorldGeneration
{
    /// <summary>
    /// directions in which a neighbor tile can lie
    /// </summary>
    public static class Directions
    {
        /// <summary>
        /// all directions in which a neighbor tile can lie
        /// </summary>
        public enum AllDirections
        {
            NorthWest, North, NorthEast, East, SouthEast, South, SouthWest, West
        }

        /// <summary>
        /// List of the four basic directions north, east, south, west.
        /// </summary>
        /// <value>A List List of the four basic directions</value>
        public static readonly AllDirections[] BaseDirections = new AllDirections[] { AllDirections.North, AllDirections.East, AllDirections.South, AllDirections.West };
    }

}