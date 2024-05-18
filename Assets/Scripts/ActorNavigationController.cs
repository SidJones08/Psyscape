using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorNavigationController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private bool isRunning;
    [SerializeField] private bool playerFocusOnStart; 
    [SerializeField] private float refreshRate = 1f;
  
    [SerializeField] private float maxTargetDistanceCurrent;
    [SerializeField] private float maxTargetDistanceDefault;
    [SerializeField] private float maxTargetDistanceLadder;

    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private List<Vector3> path = new List<Vector3>();

    public event Action OnPathUpdate;

    private void Start()
    {
        if (playerFocusOnStart)
            playerController = FindObjectOfType<PlayerController>();

        TileMapManager.instance.OnTileStatusUpdate += UpdatePathNodes;

        SetPathNodes(TileMapManager.instance.GetClosestLevelContainer(transform.position).TileTypes);
        StartPathfindingRoutine();
    }
    
    public void StartPathfindingRoutine()
    {
        if (!isRunning)
            StartCoroutine("PathfindingCoroutine");
    }

    public void StopPathfindingRoutine()
    {
        StopCoroutine("PathfindingCoroutine");
        path.Clear();
        isRunning = false;
    }

    private IEnumerator PathfindingCoroutine()
    {
        isRunning = true;
        while (true)
        {
            SetPath();
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public void SetPathNodes(List<TileType> InputTileTypes)
    {
        pathfinding.allNodes.Clear();

        for (int i = 0; i < InputTileTypes.Count; i++)
        {
            TileType tileType = InputTileTypes[i];
            PathNode node = new PathNode(Mathf.RoundToInt(tileType.TilePosition.x), Mathf.RoundToInt(tileType.TilePosition.y));

            node.TileCategory = tileType.TileCategory;

            if (tileType.TileCategory == TileType.TileCategories.Solid || tileType.TileCategory == TileType.TileCategories.DoorClose)
                node.isWalkable = false;

            pathfinding.allNodes.Add(node);
        }

        int walkable = 0;
        int unwalkable = 0;

        for (int i = 0; i < pathfinding.allNodes.Count; i++)
        {
            if (pathfinding.allNodes[i].isWalkable)
                walkable++;
            else
                unwalkable++;
        }
    }

    public void UpdatePathNodes()
    {
        SetPathNodes(TileMapManager.instance.GetClosestLevelContainer(transform.position).TileTypes);
    }

    public void SetPath()
    {
        Vector3 target = Vector3.zero;

        if (playerController.GetOnGround())
        {
            target = playerController.transform.position;
        }
        else
        {
            if (playerController.GetIsClimbing())
            {
                target = playerController.transform.position;
            }
            else
            {
                target = playerController.GetLastGroundedPosition();
            }
        }

        if (playerController)
        {
            pathfinding.ResetPathFinding();
            path.Clear();

            Vector3 startRound = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
            Vector3 endRound = new Vector3(Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.y), 0);

            PathNode startNode = pathfinding.GetNode((int)startRound.x, (int)startRound.y);
            PathNode endNode = pathfinding.GetNode((int)endRound.x, (int)endRound.y);

            if (endNode != null && startNode != null)
            {
                Vector3 startPos = new Vector3(startNode.x, startNode.y, 0);
                Vector3 endPos = new Vector3(endNode.x, endNode.y, 0);

                if (pathfinding.FindPath(startPos, endPos) != null)
                    path.AddRange(pathfinding.FindPath(startPos, endPos));
                //else
                    //Debug.Log("Invalid Path");
            }
            else
            {
                //Debug.Log("Invalid Nodes");
            }
        }
        else
        {
            //Debug.Log("No Target");
        }

        OnPathUpdate?.Invoke();
    }

    public List<Vector3> GetPath()
    {
        return path;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public float GetMaxTargetDistanceCurrent()
    {
        return maxTargetDistanceCurrent;
    }

    public float GetMaxTargetDistanceDefault()
    {
        return maxTargetDistanceDefault;
    }

    public float GetMaxTargetDistanceLadder()
    {
        return maxTargetDistanceLadder;
    }

    public void SetMaxTargetDistanceCurrent(float value)
    {
        maxTargetDistanceCurrent = value;
    }
}
