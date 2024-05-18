using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffectObject : MonoBehaviour
{
    protected TileType.TileCategories tileCatagory;

    public virtual TileType.TileCategories GetTileCategory()
    {
        return tileCatagory;
    }

    public virtual void UpdateTileStatus(TileEffectObject tileEffectObject)
    {
    }

    public virtual bool GetForcePassThrough()
    {
        return false;
    }
}