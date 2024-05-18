using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceController : TileEffectObject
{
    [SerializeField] private bool forcePassThrough = true;

    public override bool GetForcePassThrough()
    {
        return forcePassThrough;
    }

    public override TileType.TileCategories GetTileCategory()
    {
        return TileType.TileCategories.Solid;
    }

    public override void UpdateTileStatus(TileEffectObject tileEffectObject)
    {
        base.UpdateTileStatus(tileEffectObject);
    }
}
