using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansiveForceBackgroundEffect : MonoBehaviour
{
    private ExpansiveForce expansiveForce;
    private SpriteRenderer spriteRenderer;
    private BurstForceController burstForceController;

    [SerializeField] private bool isRunning;
    [SerializeField] private float elapsed;
    [SerializeField] private float duration = 1;

    [SerializeField] private float power = 0.25f;

    [SerializeField] private Vector2 topLeft;
    [SerializeField] private Vector2 topRight;
    [SerializeField] private Vector2 bottomLeft;
    [SerializeField] private Vector2 bottomRight;
    [SerializeField] private Vector2 shapeTotal;

    private void Awake()
    {
        expansiveForce = FindObjectOfType<ExpansiveForce>();
        burstForceController = FindObjectOfType<BurstForceController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        expansiveForce.OnExpandEnter += EnterExpansiveForceBackground;
        expansiveForce.OnExpandStay += UpdateExpansiveForceBackground;
        expansiveForce.OnExpandExit += ExitExpansiveForceBackground;

        burstForceController.OnForceBurstEnter += StartBackgroundEffectRoutine;
    }

    public Vector2 GetShapeFromPositions(Vector2 origin, List<Vector2> positions)
    {
        Vector2 shape = Vector2.zero;

        Vector2 minXPos = origin;
        Vector2 maxXPos = origin;
        Vector2 minYPos = origin;
        Vector2 maxYPos = origin;

        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i].x < minXPos.x)
                minXPos = positions[i];

            if (positions[i].x > maxXPos.x)
                maxXPos = positions[i];

            if (positions[i].y < minYPos.y)
                minYPos = positions[i];

            if (positions[i].y > maxYPos.y)
                maxYPos = positions[i];
        }

        float xDistance = maxXPos.x - minXPos.x;
        float yDistance = maxYPos.y - minYPos.y;

        shape = new Vector2(Mathf.Round(xDistance) + 1, Mathf.Round(yDistance) + 1);

        return shape;
    }

    public void StartBackgroundEffectRoutine(BurstForceController burstForceController, Vector3 pos)
    {
        if (!isRunning)
        {
            StartCoroutine("BackgroundEffectRoutine");
        }
    }

    IEnumerator BackgroundEffectRoutine()
    {
        isRunning = true;

        spriteRenderer.material.SetFloat("_In", power);
        spriteRenderer.material.SetFloat("_Progress", (0f));

        float degrees = expansiveForce.GetForceDirectionDegress();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            spriteRenderer.material.SetFloat("_Progress", (elapsed / duration));
            spriteRenderer.material.SetFloat("_Degrees", degrees);
            yield return null;
        }

        spriteRenderer.material.SetFloat("_Progress", (0));
        spriteRenderer.material.SetFloat("_Degrees", 0);
        spriteRenderer.material.SetFloat("_In", 0f);
        elapsed = 0;

        isRunning = false;
    }

    private void CancelBackgroundEffectRoutine()
    {
        StopCoroutine("BackgroundEffectRoutine");

        spriteRenderer.material.SetFloat("_Progress", (0));
        spriteRenderer.material.SetFloat("_Degrees", 0);
        spriteRenderer.material.SetFloat("_In", 0f);
        elapsed = 0;

        isRunning = false;
    }

    public void SetCornersInPositions(Vector2 origin, List<Vector2> positions)
    {
        topLeft = origin;
        topRight = origin;
        bottomLeft = origin;
        bottomRight = origin;

        List<Vector2> corners = new List<Vector2>();

        float xMinValue = origin.x;
        float xMaxValue = origin.x;
        float yMinValue = origin.y;
        float yMaxValue = origin.y;

        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i].x < xMinValue)
                xMinValue = positions[i].x;

            if (positions[i].x > xMaxValue)
                xMaxValue = positions[i].x;

            if (positions[i].y < yMinValue)
                yMinValue = positions[i].y;

            if (positions[i].y > yMaxValue)
                yMaxValue = positions[i].y;
        }

        topLeft = new Vector2(xMinValue, yMaxValue);
        bottomLeft = new Vector2(xMinValue, yMinValue);
        topRight = new Vector2(xMaxValue, yMaxValue);
        bottomRight = new Vector2(xMaxValue, yMinValue);
    }

    public void EnterExpansiveForceBackground(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        spriteRenderer.enabled = true;
    }

    public void UpdateExpansiveForceBackground(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        List<Vector2> positions = new List<Vector2>();

        for (int i = 0; i < expansiveForce.GetExpansiveForceBoarderVisualTileControllers().Count; i++)
            positions.Add(expansiveForce.GetExpansiveForceBoarderVisualTileControllers()[i].transform.position);

        SetCornersInPositions(GetCentreOfMultipleVectors(positions), positions);

        List<Vector2> corners = new List<Vector2>();

        corners.Add(topLeft);
        corners.Add(topRight);
        corners.Add(bottomLeft);
        corners.Add(bottomRight);

        shapeTotal = GetShapeFromPositions(GetCentreOfMultipleVectors(positions), positions);

        if(!float.IsNaN(GetCentreOfMultipleVectors(corners).x) && !float.IsNaN(GetCentreOfMultipleVectors(corners).y))
            transform.position = GetCentreOfMultipleVectors(corners);

        if (!float.IsNaN(shapeTotal.x) && !float.IsNaN(shapeTotal.y))
            transform.localScale = new Vector2(shapeTotal.x, shapeTotal.y);
    }

    public void ExitExpansiveForceBackground(LoopingSound loopingSound, ExpansiveForce expansiveForce, Vector3 pos)
    {
        spriteRenderer.enabled = false;
        CancelBackgroundEffectRoutine();
    }

    public Vector2 GetCentreOfMultipleVectors(List<Vector2> positions)
    {
        Vector2 total = Vector2.zero;

        for (int i = 0; i < positions.Count; i++)
            total += positions[i];

        total /= positions.Count;

        return total;
    }
}
