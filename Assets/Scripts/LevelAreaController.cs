using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAreaController : MonoBehaviour
{
    [SerializeField] private bool showGizmo;
    [SerializeField] private bool showCells;
    [SerializeField] private string levelName;
    [SerializeField] private int levelIndex;

    [SerializeField] private Vector2 levelSize;
    [SerializeField] private Vector2 levelPosition;
    [SerializeField] private Vector2 cellSize;

    [SerializeField] private Vector2 focusPoint;
    [SerializeField] private float orthographicSize;
    [SerializeField] private bool followPlayer;
    [SerializeField] private bool isComplete;

    private void Awake()
    {
        //Showing during playtime is expensive
        showGizmo = false;
    }

    public Vector2 GetLevelSize()
    {
        return levelSize;
    }

    public Vector2 GetLevelPosition()
    {
        return levelPosition;
    }

    public string GetLevelName()
    {
        return levelName;
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    //Expensive
    public List<Vector2> GetTilePositionsInLevelArea()
    {
        List<Vector2> tilePositions = new List<Vector2>();

        if (levelSize != Vector2.zero)
            for (int x = 0; x < levelSize.x; x++)
                for (int y = 0; y < levelSize.y; y++)
                    tilePositions.Add(new Vector2(transform.position.x - (levelSize.x * 0.5f) + (cellSize.x * x) + (cellSize.x * 0.5f), transform.position.y - (levelSize.y * 0.5f) + (cellSize.y * y) + (cellSize.y * 0.5f)));

        return tilePositions;
    }

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, levelSize);

            if (showCells)
            {
                if (levelSize != Vector2.zero && cellSize != Vector2.zero)
                    if (GetTilePositionsInLevelArea().Count > 0)
                        for (int i = 0; i < GetTilePositionsInLevelArea().Count; i++)
                            Gizmos.DrawWireCube(GetTilePositionsInLevelArea()[i], cellSize);
            }
        }
    }
}
