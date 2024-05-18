using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelContainer
{
    public string LevelName;
    public int LevelIndex;
    public Vector2 LevelPosition;
    public Vector2 LevelSize;
    public List<TileType> TileTypes = new List<TileType>();
    public string SceneName;
}