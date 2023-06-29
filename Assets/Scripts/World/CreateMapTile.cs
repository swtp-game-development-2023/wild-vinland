/*
 TODO ask tim why?
 using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

///<summary>
/// Gives Unity the ability to create Tiles of Class Type MapTile
///</summary>
public class CreateMapTile
{
    [CreateTileFromPalette]
    public static TileBase MapTile(Sprite sprite)
    {
        var mapTile = ScriptableObject.CreateInstance<MapTile>();
        mapTile.sprite = sprite;
        mapTile.name = sprite.name;
        mapTile.Type = 0;
        return mapTile;
    }
}*/