using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;
using WorldGeneration;

///<summary>
/// Class to Save a Tile and its Positon in the Mapgrid
///</summary>
[Serializable]
public class PositionedGameObject
{
    public Vector3 Position;
    public EGameObjectType Type;

    public PositionedGameObject(Vector3 Position, EGameObjectType Type)
    {
        this.Type = Type;
        this.Position = Position;
    }
}