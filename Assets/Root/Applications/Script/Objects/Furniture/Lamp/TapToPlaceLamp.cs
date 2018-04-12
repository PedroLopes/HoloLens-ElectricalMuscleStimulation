using UnityEngine;
using System.Collections;

public class TapToPlaceLamp : TapToPlace
{
    public TapToPlace button;

    public override void FinishPlacing()
    {
        if (placing)
        {
            base.FinishPlacing();
            button.gameObject.SetActive(true);
            button.Init();
            button.StartPlacing();
        }
    }

    public override void Delete()
    {
        GameObject.Destroy(transform.parent.gameObject);
    }

    protected override Vector3 GetDefaultPos()
    {
        return new Vector3(0, -0.05f, 2.0f);
    }
}