using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : TileEffectObject
{
    [SerializeField] private TileType.TileCategories tileCategory;
    [SerializeField] private bool forcePassThrough = true;

    public override TileType.TileCategories GetTileCategory()
    {
        return tileCategory;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        MovementController movementController = collision.GetComponent<MovementController>();

        if (movementController)
        {
            movementController.OnClimbEnter();
            movementController.SetOnGround(true);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        MovementController movementController = collision.GetComponent<MovementController>();

        if (movementController)
        {
            movementController.OnClimbEnter();
            movementController.SetOnGround(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        MovementController movementController = collision.GetComponent<MovementController>();

        if (movementController)
        {
            movementController.OnClimbExit();
            movementController.SetOnGround(false);
        }
    }

    public override bool GetForcePassThrough()
    {
        return forcePassThrough;
    }
}
