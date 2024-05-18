using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public TileMapManager tileMapManager;
    public List<PathNode> allNodes;
    public List<PathNode> openList;
    public List<PathNode> closedList;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public Pathfinding()
    {
        allNodes = new List<PathNode>();
    }

    public void ResetPathFinding()
    {
        openList.Clear();
        closedList.Clear();
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = GetNode(startX, startY);
        PathNode endNode = GetNode(endX, endY);

        openList = new List<PathNode>() { startNode };
        //Turn into hashset
        closedList = new List<PathNode>();

        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].gCost = int.MaxValue;
            allNodes[i].CalculateFCost();
            allNodes[i].cameFromNode = null;
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);

        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                //Final Node Reached
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);

                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        List<PathNode> path = FindPath((int)startWorldPosition.x, (int)startWorldPosition.y, (int)endWorldPosition.x, (int)endWorldPosition.y);

        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();

            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y));
            }

            return vectorPath;
        }
    }

    public List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        //Straight

        //Right
        if (allNodes.Contains(GetNode(currentNode.x + 1, currentNode.y)))
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));

        //Left
        if (allNodes.Contains(GetNode(currentNode.x - 1, currentNode.y)))
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));

        //Up
        if (allNodes.Contains(GetNode(currentNode.x, currentNode.y + 1)))
        {
            PathNode northPathNode = GetNode(currentNode.x, currentNode.y + 1);

            if(northPathNode.TileCategory == TileType.TileCategories.Ladder)
            {
                if (currentNode.TileCategory == TileType.TileCategories.Ladder)
                {
                    neighbourList.Add(northPathNode);
                }
            }
            else if (northPathNode.TileCategory == TileType.TileCategories.Empty)
            {
                if(currentNode.TileCategory == TileType.TileCategories.Ladder)
                {
                    neighbourList.Add(northPathNode);
                }
            }
        }

        //Down
        if (allNodes.Contains(GetNode(currentNode.x, currentNode.y - 1)))
        {
            PathNode southPathNode = GetNode(currentNode.x, currentNode.y - 1);

            if (southPathNode != null)
            {
                if (southPathNode.TileCategory == TileType.TileCategories.Ladder)
                {
                    neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
                }
            }

            if(currentNode.TileCategory == TileType.TileCategories.Empty)
            {
                if(southPathNode != null)
                {
                    neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
                }
            }
        }
        
        return neighbourList;
    }

    public PathNode GetNode(int x, int y)
    {
        PathNode path = null;

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i].x == x && allNodes[i].y == y)
            {
                path = allNodes[i];
                break;
            }
        }

        return path;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    //More nodes, slower this will be
    //Look into Binary tree
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCost = pathNodeList[0];

        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCost.fCost)
            {
                lowestFCost = pathNodeList[i];
            }
        }

        return lowestFCost;
    }

    //Diagonal
    /*
    if (allNodes.Contains(GetNode(currentNode.x + 1, currentNode.y + 1)))
        neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));

    if (allNodes.Contains(GetNode(currentNode.x - 1, currentNode.y - 1)))
        neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));

    if (allNodes.Contains(GetNode(currentNode.x - 1, currentNode.y + 1)))
        neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));

    if (allNodes.Contains(GetNode(currentNode.x + 1, currentNode.y - 1)))
        neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
    */
}
