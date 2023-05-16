using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

///<summary>
/// Class with basic load, clear and save functionality
///</summary>
public class MapSaveSystem : MonoBehaviour
{
    private Tilemap _seaMap, _beachMap, _grassMap, _MountainMap, _farmableMap, _decoMap, _unitMap;

    // Variables to Link with Tilemaps in Unity
    [SerializeField] private int _levelIndex;
    // index to easly name different Save Files

    private InputManager _input = null;
    // Links up with our InputManager.inpuctactions object in Unity
    private InputAction _save, _load;
    
    void Start()
    {
        _seaMap = transform.Find("Sea").gameObject.GetComponent<Tilemap>();
        _beachMap = transform.Find("Beach").gameObject.GetComponent<Tilemap>();
        _grassMap = transform.Find("Grass").gameObject.GetComponent<Tilemap>();
        _MountainMap = transform.Find("Mountain").gameObject.GetComponent<Tilemap>();
        _farmableMap = transform.Find("Farmables").gameObject.GetComponent<Tilemap>();
        _decoMap = transform.Find("Deco").gameObject.GetComponent<Tilemap>();
        _unitMap = transform.Find("Buildings").gameObject.GetComponent<Tilemap>();
    }

    private void Awake() 
    {
        _input = new InputManager();
    }

    private void OnEnable() 
    {
        _save = _input.UI.save;
        _save.Enable();
        _load = _input.UI.load;
        _load.Enable();
    }

    void OnDisable()
    {
        _save.Disable();
        _load.Disable();
    }

    void Update()
    {
        if(_save.IsPressed()) SaveMap();
        // Saves the Tilemaps, if the save button is pressed
        if(_load.IsPressed()) LoadMap();
        // Loads the Tilemaps, if the load button is pressed
    }

    ///<summary>
    /// saves the Tilemaps in a Json textfile in the Assets/Saves/ Folder
    ///</summary>
    public void SaveMap()
    {
        SaveGame newSave = new SaveGame();

        newSave.LevelIndex = _levelIndex;
        newSave.SeaTiles = GetTilesFromMap(_seaMap, TileType.Sea).ToList();
        newSave.BeachTiles = GetTilesFromMap(_beachMap, TileType.Beach).ToList();
        newSave.GrassTiles = GetTilesFromMap(_grassMap, TileType.Grass).ToList();
        newSave.MountainTiles = GetTilesFromMap(_MountainMap, TileType.Mountain).ToList();
        newSave.FarmableTiles = GetTilesFromMap(_farmableMap, TileType.Farmable).ToList();
        newSave.DecoTiles = GetTilesFromMap(_decoMap, TileType.Deco).ToList();
        newSave.UnitTiles = GetTilesFromMap(_unitMap, TileType.Player).ToList();

        String json = JsonUtility.ToJson(newSave, true);
        // Saves the SaveGame object as Json textfile, second parameter formats the Json in a more readable format if true, at cost of bigger file size
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Saves");
        File.WriteAllText(Application.dataPath + "/Saves/worldmap_sav_" + newSave.LevelIndex + ".json", json);
        // Writes the Json File to disk inside the Assets/save folder (folder structure needs to exit)

        IEnumerable<PositionedTile> GetTilesFromMap(Tilemap map, TileType tiletype)
        {
            // gathers the MapTiles and their Positions in the Tilemap to save them as PositionedTile Objects
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (map.HasTile(pos))
                {
                    var mapTile = map.GetTile<MapTile>(pos);
                    mapTile.Type = tiletype;
                    yield return new PositionedTile()
                    {
                        Position = pos,
                        Tile = mapTile
                    };
                }
            }
        }

        Debug.Log("Gameworld saved!");
    }

    ///<summary>
    /// Finds all Tilemap Gameobjects and deletes all Tiles
    ///</summary>
    public void ClearMap()
    {
        var maps = FindObjectsOfType<Tilemap>();

        foreach (var tilemap in maps)
        {
            tilemap.ClearAllTiles();
        }

        Debug.Log("Gameworld deleted!");
    }

    ///<summary>
    /// Trys to load a Savegame from path (index set in the _levelIndex field): Assets/saves/worldmap_sav_<index> 
    ///</summary>
    public void LoadMap()
    {
        string json = File.ReadAllText(Application.dataPath + "/Saves/worldmap_sav_" + _levelIndex + ".json");
        SaveGame newLoad = JsonUtility.FromJson<SaveGame>(json);

        ClearMap();

        List<PositionedTile>[] tilemaps =
        {
            newLoad.SeaTiles, newLoad.BeachTiles, newLoad.GrassTiles, newLoad.MountainTiles, newLoad.FarmableTiles,
            newLoad.DecoTiles, newLoad.UnitTiles
        };

        foreach (var tilemap in tilemaps)
        {
            foreach (var savedTile in tilemap)
            {
                switch (savedTile.Tile.Type)
                {
                    case TileType.Sea:
                        SetTile(_seaMap, savedTile);
                        break;
                    case TileType.Beach:
                        SetTile(_beachMap, savedTile);
                        break;
                    case TileType.Grass:
                        SetTile(_grassMap, savedTile);
                        break;
                    case TileType.Mountain:
                        SetTile(_MountainMap, savedTile);
                        break;
                    case TileType.Farmable:
                        SetTile(_farmableMap, savedTile);
                        break;
                    case TileType.Deco:
                        SetTile(_decoMap, savedTile);
                        break;
                    case TileType.Player:
                        SetTile(_unitMap, savedTile);
                        break;
                    case TileType.Animal:
                        SetTile(_unitMap, savedTile);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        void SetTile(Tilemap map, PositionedTile tile)
        {
            map.SetTile(tile.Position, tile.Tile);
        }

        Debug.Log("Gameworld loaded!");
    }
}