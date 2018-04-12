using UnityEngine;
using System.Collections;

public class TapToPlaceLampButton : TapToPlace
{
    public override void Delete()
    {
        GameObject.Destroy(transform.parent.gameObject);
    }

    public override void Init()
    {
        if (initialized)
            return;

        defaultLayer = gameObject.layer;

        initialized = true;
    }
}