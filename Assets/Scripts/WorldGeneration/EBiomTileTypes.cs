using System;
namespace WorldGeneration
{
    /// <summary>
    /// The basic types that a tile has.
    /// </summary>
    [Serializable]
    public enum EBiomTileTypes
    {
        Sea = 0,
        Beach = 1,
        Grass = 2,
        Mountain = 3,
        //TODO separate layers and biom
        Farmable = 4,
        Decoration = 5,
    }
}