using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using SceneDropBoxes;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using World;

///<summary>
/// Class with basic load, clear and save functionality
///</summary>
public class MapSaveSystem : MonoBehaviour
{
    // Variables to Link with Tilemaps in Unity
    [SerializeField] private int _levelIndex;
    // index to easly name different Save Files

    private Tilemap _seaMap, _beachMap, _grassMap, _mountainMap, _farmableMap, _decoMap, _unitMap;
    private Inventory _inventory;
    
    private InputManager _input = null;

    // Links up with our InputManager.inpuctactions object in Unity
    private InputAction _save, _load;

    void Start()
    {
        _seaMap = transform.Find("Sea").gameObject.GetComponent<Tilemap>();
        _beachMap = transform.Find("Beach").gameObject.GetComponent<Tilemap>();
        _grassMap = transform.Find("Grass").gameObject.GetComponent<Tilemap>();
        _mountainMap = transform.Find("Mountain").gameObject.GetComponent<Tilemap>();
        _farmableMap = transform.Find("Farmables").gameObject.GetComponent<Tilemap>();
        _decoMap = transform.Find("Deco").gameObject.GetComponent<Tilemap>();
        _unitMap = transform.Find("Buildings").gameObject.GetComponent<Tilemap>();
        _inventory = GameObject.Find("Player").GetComponent<Inventory>();

        if (UILoadGameDropBox.IsFilled)
        {
            LoadMap(UILoadGameDropBox.SaveGameName);
            UILoadGameDropBox.IsFilled = false;
        }
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
        if (_save.IsPressed()) SaveMap();
        // Saves the Tilemaps, if the save button is pressed
        if (_load.IsPressed()) LoadMap();
        // Loads the Tilemaps, if the load button is pressed
    }

    public void SaveMap()
    {
        SaveMap(_levelIndex.ToString());
    }

    ///<summary>
    /// saves the Tilemaps in a Json textfile in the Assets/Saves/ Folder
    ///</summary>
    public void SaveMap(String saveName)
    {
        SaveGame newSave = new SaveGame();
        
        newSave.SeaTiles = GetTilesFromMap(_seaMap, TileType.Sea).ToList();
        newSave.BeachTiles = GetTilesFromMap(_beachMap, TileType.Beach).ToList();
        newSave.GrassTiles = GetTilesFromMap(_grassMap, TileType.Grass).ToList();
        newSave.MountainTiles = GetTilesFromMap(_mountainMap, TileType.Mountain).ToList();
        newSave.FarmableTiles = GetTilesFromMap(_farmableMap, TileType.Farmable).ToList();
        newSave.DecoTiles = GetTilesFromMap(_decoMap, TileType.Deco).ToList();
        newSave.UnitTiles = GetTilesFromMap(_unitMap, TileType.Player).ToList();
        newSave.PlayerPosition = WorldHelper.GetPlayerPositon();
        newSave.PlayerRotation = WorldHelper.GetPlayerRotation();
        newSave.Inventory = _inventory.Serialize();
        
        String json = JsonUtility.ToJson(newSave, true);
        // Saves the SaveGame object as Json textfile, second parameter formats the Json in a more readable format if true, at cost of bigger file size
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Saves");
        File.WriteAllText(Application.dataPath + "/Saves/worldmap_sav_" + saveName + ".json", json);
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
                    yield return new PositionedTile(pos, mapTile);
                }
            }
        }

        Debug.Log("Gameworld saved!");
    }

    public void LoadMap()
    {
        LoadMap("worldmap_sav_" + _levelIndex + ".json");
    }
    ///<summary>
    /// Trys to load a Savegame from path (index set in the _levelIndex field): Assets/saves/worldmap_sav_<index> 
    ///</summary>
    public void LoadMap(string saveGameName)
    {
        try
        {
            string json = File.ReadAllText(Application.dataPath + "/Saves/" + saveGameName );
            SaveGame newLoad = JsonUtility.FromJson<SaveGame>(json);

            WorldHelper.ClearMap();

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
                            WorldHelper.SetTile(_seaMap, savedTile);
                            break;
                        case TileType.Beach:
                            WorldHelper.SetTile(_beachMap, savedTile);
                            break;
                        case TileType.Grass:
                            WorldHelper.SetTile(_grassMap, savedTile);
                            break;
                        case TileType.Mountain:
                            WorldHelper.SetTile(_mountainMap, savedTile);
                            break;
                        case TileType.Farmable:
                            WorldHelper.SetTile(_farmableMap, savedTile);
                            break;
                        case TileType.Deco:
                            WorldHelper.SetTile(_decoMap, savedTile);
                            break;
                        case TileType.Player:
                            WorldHelper.SetTile(_unitMap, savedTile);
                            break;
                        case TileType.Animal:
                            WorldHelper.SetTile(_unitMap, savedTile);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            WorldHelper.SetPlayerPosition(newLoad.PlayerPosition);
            WorldHelper.SetPlayerRotation(newLoad.PlayerRotation);
            _inventory.DeSerialize(newLoad.Inventory);
            Debug.Log("Gameworld loaded!");
        }
        catch (System.Exception)
        {
            throw;
            Debug.Log("Gameworld Save File not Found under: " + saveGameName);
            return;
        }
    }
}