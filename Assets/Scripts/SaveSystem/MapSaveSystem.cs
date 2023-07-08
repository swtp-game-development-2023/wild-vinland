using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using SceneDropBoxes;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using World;
using WorldGeneration;

///<summary>
/// Class with basic load, clear and save functionality
///</summary>
public class MapSaveSystem : MonoBehaviour
{
    // Variables to Link with Tilemaps in Unity
    [SerializeField] private int _levelIndex;
    // index to easly name different Save Files

    private Tilemap _seaMap, _beachMap, _grassMap, _mountainMap, _farmableMap, _decoMap, _buildingMap;
    public GameObject orePrefab;
    public GameObject[] treePrefab, stonePrefabs, buildingPrefabs;
    private Inventory _inventory;
    private TilePlacer tilePlacer;
    
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
        _buildingMap = transform.Find("Buildings").gameObject.GetComponent<Tilemap>();
        tilePlacer = new TilePlacer(_seaMap, _beachMap, _grassMap, _mountainMap, _decoMap);

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

    
    //TODO find a better solution, but it`s not stupid if it works!
    private bool isLoaded;
    private int framesAtLoad;
    void Update()
    {
        if (_save.IsPressed()) SaveMap(); // Saves the Tilemaps, if the save button is pressed
        if (_load.IsPressed()) LoadMap(); // Loads the Tilemaps, if the load button is pressed

        if (isLoaded && framesAtLoad + 1 < Time.frameCount)
        {
            AstarPath.active.Scan();
            isLoaded = false;
        }
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
        
        newSave.SeaTiles = GetTilesFromMap(_seaMap).ToList();
        newSave.BeachTiles = GetTilesFromMap(_beachMap).ToList();
        newSave.GrassTiles = GetTilesFromMap(_grassMap).ToList();
        newSave.MountainTiles = GetTilesFromMap(_mountainMap).ToList();
        newSave.DecoTiles = GetTilesFromMap(_decoMap).ToList();
        
        newSave.FarmableObjects = GetGameObjectsFromMap(_farmableMap).ToList();
        newSave.BuildingObjects = GetGameObjectsFromMap(_buildingMap).ToList();

        newSave.PlayerPosition = WorldHelper.GetPlayerPositon();
        newSave.PlayerRotation = WorldHelper.GetPlayerRotation();
        newSave.Inventory = _inventory.Serialize();
        
        String json = JsonUtility.ToJson(newSave, true);
        // Saves the SaveGame object as Json textfile, second parameter formats the Json in a more readable format if true, at cost of bigger file size
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        File.WriteAllText(Application.persistentDataPath + "/Saves/sav_" + saveName + ".json", json);
        // Writes the Json File to disk inside the Assets/save folder (folder structure needs to exit)

        IEnumerable<PositionedTile> GetTilesFromMap(Tilemap map)
        {
            // gathers the MapTiles and their Positions in the Tilemap to save them as PositionedTile Objects
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (map.HasTile(pos))
                {
                    MapTile mapTile = map.GetTile<MapTile>(pos);
                    switch (mapTile.sprite.name)
                    {
                        case "MasterSimple_76":
                            mapTile.Type = EBiomTileTypes.Sea;
                            yield return new PositionedTile(pos, EBiomTileTypes.Sea);
                            break;
                        case "MasterSimple_17":
                            mapTile.Type = EBiomTileTypes.Grass;
                            yield return new PositionedTile(pos, EBiomTileTypes.Grass);
                            break;
                        case "MasterSimple_149":
                            mapTile.Type = EBiomTileTypes.Mountain;
                            yield return new PositionedTile(pos, EBiomTileTypes.Mountain);
                            break;
                        case "MasterSimple_154":
                            mapTile.Type = EBiomTileTypes.Beach;
                            yield return new PositionedTile(pos, EBiomTileTypes.Beach);
                            break;
                        case "MasterSimple_7":
                            mapTile.SpecialType = ESpecialTiles.HighGrass;
                            yield return new PositionedTile(pos, ESpecialTiles.HighGrass);
                            break;
                        case "MasterSimple_8":
                            mapTile.SpecialType = ESpecialTiles.Flowers;
                            yield return new PositionedTile(pos, ESpecialTiles.Flowers);
                            break;
                    }
                }
            }
        }
        IEnumerable<PositionedGameObject> GetGameObjectsFromMap(Tilemap map)
        {
            if(map.transform.childCount > 0)
            {
                foreach (Transform gamobjectInTilemap in map.transform)
                {
                    EGameObjectType type = EGameObjectType.Tree;
                    String spritename;
                    if(gamobjectInTilemap.gameObject.GetComponent<SpriteRenderer>().Equals(null))
                    {
                        spritename = gamobjectInTilemap.gameObject.GetComponentInChildren<SpriteRenderer>().sprite.name;
                    }else{
                        spritename = gamobjectInTilemap.gameObject.GetComponent<SpriteRenderer>().sprite.name;
                    }
                    //Debug.Log(spritename);
                    switch (spritename)
                    {
                        case "Tree":
                            type = EGameObjectType.Tree;
                            break;
                        case "Bush":
                            type = EGameObjectType.Bush;
                            break;
                        case "Ore":
                            type = EGameObjectType.Ore;
                            break;
                        case "Stone01":
                            type = EGameObjectType.Stone01;
                            break;
                        case "Stone02":
                            type = EGameObjectType.Stone02;
                            break;
                        case "Stone03":
                            type = EGameObjectType.Stone03;
                            break;
                        case "MasterSimple_41":
                            type = EGameObjectType.Dock;
                            break;
                        case "MasterSimple_40":
                            type = EGameObjectType.Windmill;
                            break;
                        case "MasterSimple_25":
                            type = EGameObjectType.Lumberjack;
                            break;
                        case "MasterSimple_10":
                            type = EGameObjectType.Stonecutter;
                            break;
                        case "ship_strip16 1_4":
                            type = EGameObjectType.Ship;
                            break;
                    }
                    yield return new PositionedGameObject(
                        gamobjectInTilemap.transform.position, 
                        type);
                }
            }
        }

        Debug.Log("Gameworld saved!");
    }

    public void LoadMap()
    {
        LoadMap("sav_" + _levelIndex + ".json");
    }
    ///<summary>
    /// Trys to load a Savegame from path (index set in the _levelIndex field): Assets/saves/worldmap_sav_<index> 
    ///</summary>
    public void LoadMap(string saveGameName)
    {
        try
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/Saves/" + saveGameName );
            SaveGame newLoad = JsonUtility.FromJson<SaveGame>(json);

            WorldHelper.ClearMap();

            List<PositionedTile>[] tilemaps =
            {
                newLoad.SeaTiles, newLoad.BeachTiles, newLoad.GrassTiles, newLoad.MountainTiles
            };
            List<PositionedGameObject>[] gameobjectslist =
            {
                newLoad.FarmableObjects, newLoad.BuildingObjects
            };
            
            // Placing regular Tiles
            foreach (List<PositionedTile> tilemap in tilemaps)
            {
                foreach (PositionedTile savedTile in tilemap)
                {
                    tilePlacer.Place((int)savedTile.Type, savedTile.Position);
                }
            }

            // Placing special Tiles
            foreach (var savedTile in newLoad.DecoTiles)
            {
                tilePlacer.PlaceDecoration((int)savedTile.SpecialType, savedTile.Position);
            }

            // Placing GameObjects
            GameObject GameObjectToPlace;
            foreach (List<PositionedGameObject> gameObjects in gameobjectslist)
            {
                foreach (PositionedGameObject gameObject in gameObjects)
                {
                    switch (gameObject.Type)
                    {
                        case EGameObjectType.Tree:
                            Instantiate(treePrefab[0], gameObject.Position, Quaternion.identity, _farmableMap.transform);
                            break;
                        case EGameObjectType.Bush:
                            Instantiate(treePrefab[1], gameObject.Position, Quaternion.identity, _farmableMap.transform);
                            break;
                        case EGameObjectType.Ore:
                            Instantiate(orePrefab, gameObject.Position, Quaternion.identity, _farmableMap.transform);
                            break;
                        case EGameObjectType.Stone01:
                            Instantiate(stonePrefabs[0], gameObject.Position, Quaternion.identity, _farmableMap.transform);
                            break;
                        case EGameObjectType.Stone02:
                            Instantiate(stonePrefabs[1], gameObject.Position, Quaternion.identity, _farmableMap.transform);
                            break;
                        case EGameObjectType.Stone03:
                            Instantiate(stonePrefabs[2], gameObject.Position, Quaternion.identity, _farmableMap.transform);
                            break;
                        case EGameObjectType.Dock:
                            GameObjectToPlace = Instantiate(buildingPrefabs[0], gameObject.Position, Quaternion.identity, _buildingMap.transform);
                            GameObjectToPlace.GetComponent<PolygonCollider2D>().enabled = true;
                            break;
                        case EGameObjectType.Windmill:
                            GameObjectToPlace = Instantiate(buildingPrefabs[1], gameObject.Position, Quaternion.identity, _buildingMap.transform);
                            GameObjectToPlace.GetComponent<PolygonCollider2D>().enabled = true;
                            break;
                        case EGameObjectType.Lumberjack:
                            GameObjectToPlace = Instantiate(buildingPrefabs[2], gameObject.Position, Quaternion.identity, _buildingMap.transform);
                            GameObjectToPlace.GetComponent<PolygonCollider2D>().enabled = true;
                            break;
                        case EGameObjectType.Stonecutter:
                            GameObjectToPlace = Instantiate(buildingPrefabs[3], gameObject.Position, Quaternion.identity, _buildingMap.transform);
                            GameObjectToPlace.GetComponent<PolygonCollider2D>().enabled = true;
                            break;
                        case EGameObjectType.Ship:
                            Instantiate(buildingPrefabs[4], gameObject.Position, Quaternion.identity, _buildingMap.transform);
                            break;
                    }
                }
            }

            WorldHelper.SetPlayerPosition(newLoad.PlayerPosition);
            WorldHelper.SetPlayerRotation(newLoad.PlayerRotation);
            _inventory.DeSerialize(newLoad.Inventory);
            Debug.Log("Gameworld "+ saveGameName+" loaded!");

            //TODO ugly, but works:
            isLoaded = true;
            framesAtLoad = Time.frameCount;
        }
        catch (Exception)
        {
            //throw;
            Debug.Log("Gameworld Save File not Found under: "+Application.persistentDataPath + "/Saves/sav_" + saveGameName);
        }
    }
    

}