using System;
namespace WorldGeneration
{
    
    [Serializable]
    public enum EGameObjectType
    {
        // Farmable
    Tree = 200,
    Bush = 201,
    Stone01 = 202,
    Stone02 = 203,
    Stone03 = 204,
    Ore = 210,
    
    // Buildings
    Dock = 500,
    Windmill = 501,
    Lumberjack = 502,
    Stonecutter = 503,
    Ship = 510,

    // Unit
    Animal  = 1000
    }
    
}