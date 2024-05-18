using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class TileMapManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] GameObject startPointGameObject;
    [SerializeField] private List<TileEffectObject> tileEffectObjects = new List<TileEffectObject>();
    [SerializeField] private List<LevelContainer> levelContainers = new List<LevelContainer>();


    public static TileMapManager instance;

    public event Action OnTileStatusUpdate;

    private void Awake()
    {
        instance = this;
        BuildTileTypeCollectionLevels();
    }

    private void BuildTileTypeCollectionLevels()
    {
        tileEffectObjects.AddRange(FindObjectsOfType<TileEffectObject>());

        List<LevelAreaController> levelAreaControllers = new List<LevelAreaController>();
        levelAreaControllers.AddRange(FindObjectsOfType<LevelAreaController>());

        levelAreaControllers.Sort((a, b) => Vector3.Distance(a.transform.position, startPointGameObject.transform.position).CompareTo(Vector3.Distance(b.transform.position, startPointGameObject.transform.position)));

        for (int i = 0; i < levelAreaControllers.Count; i++)
        {
            LevelContainer level = new LevelContainer();

            level.LevelPosition = levelAreaControllers[i].transform.position;
            level.LevelSize = levelAreaControllers[i].GetLevelSize();
            level.LevelName = levelAreaControllers[i].GetLevelName();
            level.LevelIndex = i;
            level.SceneName = "Room_" + level.LevelIndex; 

            for (int o = 0; o < levelAreaControllers[i].GetTilePositionsInLevelArea().Count; o++)
            {
                int xInt = Mathf.RoundToInt(levelAreaControllers[i].GetTilePositionsInLevelArea()[o].x);
                int yInt = Mathf.RoundToInt(levelAreaControllers[i].GetTilePositionsInLevelArea()[o].y);

                if(GetTileEffectObjectFromPosition(new Vector2(xInt, yInt)))
                {
                    //Debug.Log(GetTileEffectObjectFromPosition(new Vector2(xInt, yInt)).GetTileCategory().ToString());
                    //Debug.Log(GetTileEffectObjectFromPosition(new Vector2(xInt, yInt)).transform.name);

                    level.TileTypes.Add(new TileType(new Vector2(xInt, yInt), GetTileEffectObjectFromPosition(new Vector2(xInt, yInt)).GetTileCategory()));
                }
                else
                {
                    if (tilemap.GetTile(new Vector3Int(xInt, yInt, 0)))
                        level.TileTypes.Add(new TileType(new Vector2(xInt, yInt), TileType.TileCategories.Solid));
                    else
                        level.TileTypes.Add(new TileType(new Vector2(xInt, yInt), TileType.TileCategories.Empty));
                }
            }

            levelContainers.Add(level);
        }
    }

    //Should only effect that particular tile
    public void TileStatusUpdated(TileEffectObject tileEffectObject)
    {
        Vector2 tileEffectObjectVector2 = new Vector2(tileEffectObject.transform.position.x, tileEffectObject.transform.position.y);

        //Debug.Log(tileEffectObjectVector2);

        for (int i = 0; i < levelContainers.Count; i++)
        {
            for (int o = 0; o < levelContainers[i].TileTypes.Count; o++)
            {
                if(levelContainers[i].TileTypes[o].TilePosition == tileEffectObjectVector2)
                {
                    //Debug.Log("Found");

                    levelContainers[i].TileTypes[o].TileCategory = tileEffectObject.GetTileCategory();
                    levelContainers[i].TileTypes[o].TileName = levelContainers[i].TileTypes[o].TileCategory.ToString() + " " + levelContainers[i].TileTypes[o].TilePosition;

                    if (OnTileStatusUpdate != null)
                        OnTileStatusUpdate();

                    return;
                }
            }
        }
    }

    public LevelContainer GetClosestLevelContainer(Vector2 pos)
    {
        LevelContainer level = null;

        float dist = Mathf.Infinity;

        for (int i = 0; i < levelContainers.Count; i++)
        {
            if(Vector2.Distance(levelContainers[i].LevelPosition, pos) < dist)
            {
                level = levelContainers[i];
                dist = Vector2.Distance(levelContainers[i].LevelPosition, pos);
            }
        }

        return level;
    }
 
    public Tilemap GetTileMap()
    {
        return tilemap;
    }

    public List<TileBase> GetAllTiles()
    {
        List<TileBase> tiles = new List<TileBase>();

        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
                for (int z = tilemap.cellBounds.min.z; z < tilemap.cellBounds.max.z; z++)
                    if (tilemap.GetTile(new Vector3Int(x, y, z)))
                        tiles.Add(tilemap.GetTile(new Vector3Int(x, y, z)));

        return tiles;
    }

    public TileEffectObject GetTileEffectObjectFromPosition(Vector2 vector)
    {
        TileEffectObject tileEffectObject = null;

        for (int i = 0; i < tileEffectObjects.Count; i++)
        {
            if(new Vector2(tileEffectObjects[i].transform.position.x, tileEffectObjects[i].transform.position.y) == vector)
            {
                tileEffectObject = tileEffectObjects[i];
                break;
            }
        }

        return tileEffectObject;
    }
}
