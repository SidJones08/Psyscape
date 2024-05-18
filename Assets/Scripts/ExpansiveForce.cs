using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class ExpansiveForce : MonoBehaviour, LoopingSound
{
    [SerializeField] private bool canExpand = true;
    [SerializeField] private bool isExpanding;
    [SerializeField] private int iterations = 100;
    [SerializeField] private List<Vector2> directions = new List<Vector2>();

    [SerializeField] private List<Vector2> expansionTiles = new List<Vector2>();
    [SerializeField] private List<Vector2> expansionLockedTiles = new List<Vector2>();

    [SerializeField] private ExpansiveForceBoarderVisualTileController expansiveForceVisualPrefab;
    [SerializeField] private List<ExpansiveForceBoarderVisualTileController> expansiveForceVisuals = new List<ExpansiveForceBoarderVisualTileController>();
    [SerializeField] private List<ForceInteractableObjectController> forceInteractables = new List<ForceInteractableObjectController>();

    [SerializeField] private float elapsedCharge;
    [SerializeField] private float totalCharge = 1;
    [SerializeField] private float progress;

    [SerializeField] private int count = 0;

    [SerializeField] Vector2 previousForceDirection;
    [SerializeField] Vector2 forceDirection;

    [SerializeField] float forceDirectionDegrees;

    [SerializeField] private float velocityLimit = 1;
    [SerializeField] private float forceDirectionHeldPower = 5f;

    [SerializeField] private float dragOnForce;
    [SerializeField] private float dragDefault;

    private BurstForceController burstForceController;
    private PlayerController playerController;

    public event Action<LoopingSound, ExpansiveForce, Vector3> OnExpandEnter;
    public event Action<LoopingSound, ExpansiveForce, Vector3> OnExpandStay;
    public event Action<LoopingSound, ExpansiveForce, Vector3> OnExpandExit;

    public static ExpansiveForce instance;

    private void Awake()
    {
        instance = this;
        playerController = GetComponent<PlayerController>();
        burstForceController = GetComponent<BurstForceController>();
    }

    private void Update()
    {
        forceDirection.x = Input.GetAxisRaw("Horizontal");
        forceDirection.y = Input.GetAxisRaw("Vertical");

        if (playerController.GetCanAct())
        {
            if (canExpand)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    isExpanding = true;

                    if (OnExpandEnter != null)
                        OnExpandEnter(this, this, transform.position);

                    count = 0;
                    expansionLockedTiles.Clear();
                    expansionTiles.Clear();
                    expansionTiles.AddRange(GetExpandedTiles(transform.position, iterations));
                }

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    isExpanding = true;

                    if (OnExpandStay != null)
                        OnExpandStay(this, this, transform.position);

                    if (count < expansionTiles.Count)
                    {
                        if (elapsedCharge < totalCharge)
                        {
                            elapsedCharge += Time.deltaTime;
                        }
                        else
                        {
                            elapsedCharge = 0;
                            expansionLockedTiles.Add(expansionTiles[count]);
                            SpawnExpansiveTileDisplay(expansionTiles[count]);
                            count++;
                        }
                    }
                }

                if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                {
                    ClearExpansiveForce();
                }
            }
        }
    }

    public void ClearExpansiveForce()
    {
        isExpanding = false;

        if (OnExpandExit != null)
            OnExpandExit(this, this, transform.position);

        count = 0;
        ClearExpansiveTileDisplays();
        expansionLockedTiles.Clear();
        expansionTiles.Clear();
    }

    private void FixedUpdate()
    {
        if (forceInteractables.Count > 0)
        {
            for (int i = 0; i < forceInteractables.Count; i++)
            {
                if (!burstForceController.GetIsBurstForce())
                {
                    if (!forceInteractables[i].GetComponent<MissileController>())
                    {
                        if (!forceInteractables[i].GetIsInfluenceImmunity())
                        {
                            forceInteractables[i].SetIsInfluenced(true);
                            forceInteractables[i].UpdateInfluenceState();

                            forceInteractables[i].GetRigidbody2D().AddForce(forceDirection.normalized * forceDirectionHeldPower);
                            forceInteractables[i].ForceEffectAction(forceDirection);

                            if (forceInteractables[i].GetRigidbody2D().velocity.magnitude > velocityLimit)
                            {
                                float x = forceInteractables[i].GetRigidbody2D().velocity.magnitude / velocityLimit;
                                forceInteractables[i].GetRigidbody2D().velocity /= x;
                            }
                        }
                    }
                    else
                    {
                        forceInteractables[i].ForceEffectAction(forceDirection);
                    }
                }
            }
        }
    }

    public void SpawnExpansiveTileDisplay(Vector2 pos)
    {
        ExpansiveForceBoarderVisualTileController visualTileController = ObjectPool.instance.GetObjectFromPool(expansiveForceVisualPrefab.name).GetComponent<ExpansiveForceBoarderVisualTileController>();
        expansiveForceVisuals.Add(visualTileController);
        visualTileController.SetExpansiveForce(this);
        visualTileController.transform.position = pos;
        visualTileController.gameObject.SetActive(true);

        if (expansiveForceVisuals.Count > 0)
            for (int i = 0; i < expansiveForceVisuals.Count; i++)
                expansiveForceVisuals[i].DetectTiles();
    }

    public void ClearExpansiveTileDisplays()
    {
        if (expansiveForceVisuals.Count > 0)
        {
            for (int i = 0; i < expansiveForceVisuals.Count; i++)
            {
                expansiveForceVisuals[i].gameObject.SetActive(false);
                expansiveForceVisuals[i].SetExpansiveForce(null);
            }
        }

        expansiveForceVisuals.Clear();
    }

    //Revist and Clean
    public List<Vector2> GetExpandedTiles(Vector2 startPos, int amount)
    {
        List<Vector2> positions = new List<Vector2>();

        int count = 0;
        
        startPos = new Vector2(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y));

        List<Vector2> candidates = new List<Vector2>();
        Vector2 selectedPos;

        candidates.Add(startPos);
        positions.Add(startPos);

        while (candidates.Count > 0)
        {
            selectedPos = candidates[0];
            candidates.Remove(selectedPos);

            for (int i = 0; i < directions.Count; i++)
            {
                if (!positions.Contains(selectedPos + directions[i]))
                {
                    int xInt = Mathf.RoundToInt(selectedPos.x + directions[i].x);
                    int yInt = Mathf.RoundToInt(selectedPos.y + directions[i].y);

                    if (!TileMapManager.instance.GetTileMap().GetTile(new Vector3Int(xInt, yInt, 0)))
                    {
                        if (!TileMapManager.instance.GetTileEffectObjectFromPosition(new Vector2(xInt, yInt)))
                        {
                            if (!candidates.Contains(selectedPos + directions[i]))
                                candidates.Add(selectedPos + directions[i]);

                            positions.Add(selectedPos + directions[i]);

                            if (count < iterations)
                            {
                                count++;
                            }
                            else
                            {
                                candidates.Clear();
                                break;
                            }
                        }
                        else
                        {
                            if (TileMapManager.instance.GetTileEffectObjectFromPosition(new Vector2(xInt, yInt)).GetForcePassThrough())
                            {
                                //Debug.Log(TileMapManager.instance.GetTileEffectObjectFromPosition(new Vector2(xInt, yInt)).name + " IS " + TileMapManager.instance.GetTileEffectObjectFromPosition(new Vector2(xInt, yInt)).GetForcePassThrough());

                                if (!candidates.Contains(selectedPos + directions[i]))
                                    candidates.Add(selectedPos + directions[i]);

                                positions.Add(selectedPos + directions[i]);

                                if (count < iterations)
                                {
                                    count++;
                                }
                                else
                                {
                                    candidates.Clear();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        candidates.Remove(selectedPos + directions[i]);
                    }
                }
            }
        }

        return positions;
    }

    public List<ExpansiveForceBoarderVisualTileController> GetExpansiveForceBoarderVisualTileControllers()
    {
        return expansiveForceVisuals;
    }

    public ExpansiveForceBoarderVisualTileController GetExpansiveForceBoarderVisualTileFromPosition(Vector2 pos)
    {
        ExpansiveForceBoarderVisualTileController expansiveForceBoarder = null;

        for (int i = 0; i < expansiveForceVisuals.Count; i++)
        {
            if (new Vector2(expansiveForceVisuals[i].transform.position.x, expansiveForceVisuals[i].transform.position.y) == pos)
            {
                expansiveForceBoarder = expansiveForceVisuals[i];
                break;
            }
        }

        return expansiveForceBoarder;
    }

    public bool GetIsExpanding()
    {
        return isExpanding;
    }

    public Vector2 GetForceDirection()
    {
        return forceDirection;
    }

    public float GetForceDirectionDegress()
    {
        forceDirectionDegrees = Vector2.Angle(forceDirection.normalized, Vector2.up);
        float angle = Vector2.Angle(forceDirection.normalized, Vector2.left);

        if (angle > 90)
            forceDirectionDegrees = 360 - forceDirectionDegrees;

        return forceDirectionDegrees;
    }

    public List<ForceInteractableObjectController> GetForceInteractableObjectControllers()
    {
        return forceInteractables;
    }

    public bool GetCanExpand()
    {
        return canExpand;
    }

    public void SetCanExpand(bool value)
    {
        canExpand = value;
    }

    public void SetIsExpanding(bool value)
    {
        isExpanding = value;
    }

    private void OnDrawGizmos()
    {
        if (expansiveForceVisuals.Count > 0)
        {
            List<Vector2> positions = new List<Vector2>();

            for (int i = 0; i < expansiveForceVisuals.Count; i++)
                positions.Add(expansiveForceVisuals[i].transform.position);
        }
    }
}
