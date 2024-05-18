using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansiveForceBoarderVisualTileController : MonoBehaviour
{
    private SpriteRenderer renderer;

    [SerializeField] private ExpansiveForce expansiveForce;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void DetectTiles()
    {
        Vector3Int tileNorth = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 1));
        Vector3Int tileEast = new Vector3Int(Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y));
        Vector3Int tileSouth = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1));
        Vector3Int tileWest = new Vector3Int(Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y));

        if (TileMapManager.instance.GetTileMap().GetTile(tileNorth) || !expansiveForce.GetExpansiveForceBoarderVisualTileFromPosition(new Vector2(tileNorth.x, tileNorth.y)))
            renderer.material.SetVector("_DirectionNorth", new Vector2(0, 1));
        else
            renderer.material.SetVector("_DirectionNorth", Vector2.zero);

        if (TileMapManager.instance.GetTileMap().GetTile(tileEast) || !expansiveForce.GetExpansiveForceBoarderVisualTileFromPosition(new Vector2(tileEast.x, tileEast.y)))
            renderer.material.SetVector("_DirectionEast", new Vector2(1, 0));
        else
            renderer.material.SetVector("_DirectionEast", Vector2.zero);

        if (TileMapManager.instance.GetTileMap().GetTile(tileSouth) || !expansiveForce.GetExpansiveForceBoarderVisualTileFromPosition(new Vector2(tileSouth.x, tileSouth.y)))
            renderer.material.SetVector("_DirectionSouth", new Vector2(0, -1));
        else
            renderer.material.SetVector("_DirectionSouth", Vector2.zero);

        if (TileMapManager.instance.GetTileMap().GetTile(tileWest) || !expansiveForce.GetExpansiveForceBoarderVisualTileFromPosition(new Vector2(tileWest.x, tileWest.y)))
            renderer.material.SetVector("_DirectionWest", new Vector2(-1, 0));
        else
            renderer.material.SetVector("_DirectionWest", Vector2.zero);
    }

    public void ClearTiles()
    {
        renderer.material.SetVector("_DirectionNorth", Vector2.zero);
        renderer.material.SetVector("_DirectionEast", Vector2.zero);
        renderer.material.SetVector("_DirectionSouth", Vector2.zero);
        renderer.material.SetVector("_DirectionWest", Vector2.zero);
    }

    private void OnDisable()
    {
        ClearTiles();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ForceInteractableObjectController forceInteractableObject = collision.transform.GetComponent<ForceInteractableObjectController>();

        if (forceInteractableObject)
        {
            if (forceInteractableObject.gameObject.layer == 3 || forceInteractableObject.gameObject.layer == 9)
            {
                if (!expansiveForce.GetForceInteractableObjectControllers().Contains(collision.transform.GetComponent<ForceInteractableObjectController>()))
                {
                    expansiveForce.GetForceInteractableObjectControllers().Add(collision.transform.GetComponent<ForceInteractableObjectController>());
                    forceInteractableObject.SetIsInfluenced(true);
                    forceInteractableObject.UpdateInfluenceState();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ForceInteractableObjectController forceInteractableObject = collision.transform.GetComponent<ForceInteractableObjectController>();
        
        if (forceInteractableObject)
        {
            if (expansiveForce.GetForceInteractableObjectControllers().Contains(collision.transform.GetComponent<ForceInteractableObjectController>()))
            {
                expansiveForce.GetForceInteractableObjectControllers().Remove(collision.transform.GetComponent<ForceInteractableObjectController>());
                forceInteractableObject.SetIsInfluenced(false);
                forceInteractableObject.UpdateInfluenceState();
            }
        }
    }

    public void SetExpansiveForce(ExpansiveForce value)
    {
        expansiveForce = value;
    }
}
