using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansiveForceDirectionVisualTileController : MonoBehaviour
{
    private SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateOrigin(Vector2 vector)
    {
        renderer.material.SetVector("_RippleCenter", vector);
    }

    public void ClearTiles()
    {
    }
}
