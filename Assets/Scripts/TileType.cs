using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType
{
    public string TileName;
    public Vector2 TilePosition;
    public enum TileCategories { Empty, Solid, Ladder, Trap, DoorOpen, DoorClose}
    public TileCategories TileCategory;

    public TileType(Vector2 tilePosition, TileCategories tileCategory)
    {
        TilePosition = tilePosition;
        TileCategory = tileCategory;
        TileName = tileCategory.ToString() + " " + tilePosition;
    }
}
