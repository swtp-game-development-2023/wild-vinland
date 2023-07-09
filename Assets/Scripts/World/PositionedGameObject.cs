using System;
using UnityEngine;
using WorldGeneration;

///<summary>
/// Class to Save a GameObject and its Positon in the Mapgrid
///</summary>
[Serializable]
public class PositionedGameObject
{
    public Vector3 Position;
    public EGameObjectType Type;
    public Vector3 attachedGameObjectPosition;
    public int status;

    public PositionedGameObject(Vector3 Position, EGameObjectType Type)
    {
        this.Type = Type;
        this.Position = Position;
    }

    public Vector3 GetAttachedGameObjectPosition(){ return attachedGameObjectPosition; }
    public void SetAttachedGameObjectPosition(Vector3 attachedGameObject){ this.attachedGameObjectPosition = attachedGameObject; }
    public int GetStatus(){ return status; }
    public void SetStatus(int status){ this.status = status; }
}