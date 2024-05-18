using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalExpanseController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int count = 0;
    [SerializeField] private int iterations = 4;

    [Header("Positions")]
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 startingDirection;
    [SerializeField] private Vector2 curretPos;
    [SerializeField] private Vector2 currentDirection;
    [SerializeField] private Vector2 previousPos;

    [Header("Following")]
    [SerializeField] private bool isFollowing;
    [SerializeField] private int followIndex;
    [SerializeField] private float refreshRate = 0.1f;
    [SerializeField] private float followSpeed = 5f;

    [SerializeField] private float forcePullMultiplier = 1;
    [SerializeField] private float forcePushMultiplier = 1;
    [SerializeField] private float objectVelocityLimit = 1;


    [SerializeField] private List<Vector3> path = new List<Vector3>();

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnEnable()
    {
        startPos = transform.position;
    }

    private void OnDisable()
    {
        Reset();
    }

    public void Reset()
    {
        StopAllCoroutines();

        isFollowing = false;
        followIndex = 0;
        path.Clear();

        startPos = Vector2.zero;
        startingDirection = Vector2.zero;
        curretPos = Vector2.zero;
        currentDirection = Vector2.zero;
        previousPos = Vector2.zero;
    }

    public void CreatePath()
    {
        while (count < iterations)
        {
            //Double Check to see if this is even needed?
            //Make StartPos = Current
            //Start Direction = Current Direction
            //Previous Pos = CurrentPos - Curret Direction
            if (count == 0)
            {
                startPos = new Vector2(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y));
                Vector3Int dir = new Vector3Int(Mathf.RoundToInt(startPos.x + startingDirection.x), Mathf.RoundToInt(startPos.y + startingDirection.y), 0);

                path.Add(startPos);
                count++;

                if (TileMapManager.instance.GetTileMap().GetTile(dir))
                {
                    Debug.Log("Space Blocked");

                    if (GetAvailableDirections(startPos, previousPos).Count > 0)
                    {
                        Debug.Log("Direction Blocked + " + GetAvailableDirections(startPos, previousPos).Count + " Directions Available");

                        Vector2 randomDir = GetAvailableDirections(startPos, previousPos)[Random.Range(0, GetAvailableDirections(startPos, previousPos).Count)];
                        currentDirection = randomDir - startPos;
                        previousPos = startPos;
                        curretPos = new Vector2(randomDir.x, randomDir.y);
                        path.Add(curretPos);
                        count++;
                    }
                    else
                    {
                        Debug.Log("Start Blocked");
                        count = iterations;
                    }
                }
                else
                {
                    Debug.Log("Space Free");
                    curretPos = new Vector2(dir.x, dir.y);
                    currentDirection = startingDirection;
                    path.Add(curretPos);
                    previousPos = startPos;
                    count++;
                }
            }
            else
            {
                Vector3Int dir = new Vector3Int(Mathf.RoundToInt(curretPos.x + currentDirection.x), Mathf.RoundToInt(curretPos.y + currentDirection.y), 0);

                if (TileMapManager.instance.GetTileMap().GetTile(dir))
                {
                    if (GetAvailableDirections(curretPos, previousPos).Count > 0)
                    {
                        Debug.Log("Direction Blocked + " + GetAvailableDirections(curretPos, previousPos).Count + " Directions Available");

                        Vector2 randomDir = GetAvailableDirections(curretPos, previousPos)[Random.Range(0, GetAvailableDirections(curretPos, previousPos).Count)];
                        currentDirection = randomDir - curretPos;
                        previousPos = curretPos;
                        curretPos = new Vector2(randomDir.x, randomDir.y);
                        path.Add(curretPos);
                        count++;
                    }
                    else
                    {
                        Debug.Log("All Directions Blocked");
                        count = iterations;
                    }
                }
                else
                {
                    Debug.Log("Space Free");
                    previousPos = curretPos;
                    curretPos = new Vector2(dir.x, dir.y);
                    path.Add(curretPos);
                    count++;
                }
            }
        }
    }

    public void FollowPath()
    {
        if (!isFollowing)
            StartCoroutine(FollowPathRoutine());
    }

    IEnumerator FollowPathRoutine()
    {
        isFollowing = true;

        while(followIndex < path.Count)
        {
            if (transform.position != path[followIndex])
                transform.position = Vector2.MoveTowards(transform.position, path[followIndex], followSpeed * Time.deltaTime);
            else
                followIndex++;
            
            yield return null;
        }

        isFollowing = false;
    }

    public List<Vector2> GetAvailableDirections(Vector2 startPos, Vector2 excludedPos)
    {
        List<Vector2> directions = new List<Vector2>();

        excludedPos = new Vector2(Mathf.RoundToInt(excludedPos.x), Mathf.RoundToInt(excludedPos.y));
        Vector3Int excludedPosVector3Int = new Vector3Int(Mathf.RoundToInt(excludedPos.x), Mathf.RoundToInt(excludedPos.y));

        Vector3Int tileNorth = new Vector3Int(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y + 1));
        Vector3Int tileEast = new Vector3Int(Mathf.RoundToInt(startPos.x + 1), Mathf.RoundToInt(startPos.y));
        Vector3Int tileSouth = new Vector3Int(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y - 1));
        Vector3Int tileWest = new Vector3Int(Mathf.RoundToInt(startPos.x - 1), Mathf.RoundToInt(startPos.y));

        if (!TileMapManager.instance.GetTileMap().GetTile(tileNorth) && tileNorth != excludedPosVector3Int)
                directions.Add(new Vector2(tileNorth.x, tileNorth.y));

        if (!TileMapManager.instance.GetTileMap().GetTile(tileEast) && tileEast != excludedPosVector3Int)
            directions.Add(new Vector2(tileEast.x, tileEast.y));

        if (!TileMapManager.instance.GetTileMap().GetTile(tileSouth) && tileSouth != excludedPosVector3Int)
            directions.Add(new Vector2(tileSouth.x, tileSouth.y));

        if (!TileMapManager.instance.GetTileMap().GetTile(tileWest) && tileWest != excludedPosVector3Int)
            directions.Add(new Vector2(tileWest.x, tileWest.y));

        return directions;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ForceInteractableObjectController forceInteractable = collision.GetComponent<ForceInteractableObjectController>();

        if (isFollowing)
        {
            if (forceInteractable)
            {
                if (forceInteractable.gameObject.layer == 3)
                {
                    if (followIndex < path.Count)
                        forceInteractable.GetRigidbody2D().AddForce((path[followIndex] - transform.position).normalized * forcePushMultiplier);

                    //forceInteractable.GetRigidbody2D().AddForce((transform.position - forceInteractable.transform.position).normalized * forcePullMultiplier);
                    if (forceInteractable.GetRigidbody2D().velocity.magnitude > objectVelocityLimit)
                    {
                        float x = forceInteractable.GetRigidbody2D().velocity.magnitude / objectVelocityLimit;
                        forceInteractable.GetRigidbody2D().velocity /= x;
                    }
                }
            }
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(startPos, Vector2.one);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(previousPos, Vector2.one);

        Gizmos.color = Color.red;
        if (path.Count > 0)
            for (int i = 0; i < path.Count; i++)
                Gizmos.DrawSphere(path[i], 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(curretPos, Vector2.one);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector2.one);
    }
}
