using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathNode
{
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public TileType.TileCategories TileCategory;

    /*
    public PathNode pathNodeNorth;
    public PathNode pathNodeEast;
    public PathNode pathNodeSouth;
    public PathNode pathNodeWest;

    public PathNode pathNodeNorthEast;
    public PathNode pathNodeSouthEast;
    public PathNode pathNodeSouthWest;
    public PathNode pathNodeNorthWest;
    */

    public bool isWalkable;

    public PathNode cameFromNode;

    public PathNode(int x, int y)
    {
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
