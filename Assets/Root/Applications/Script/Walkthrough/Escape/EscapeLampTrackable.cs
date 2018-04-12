using System.Collections;
using Vuforia;
using UnityEngine;

public class EscapeLampTrackable : DefaultTrackableEventHandler {
    protected override void OnTrackingFound()
    {
        transform.Find("ArrowWrapper").localEulerAngles = new Vector3();
        //base.OnTrackingFound();
    }

    protected override void OnTrackingLost()
    {
        //base.OnTrackingLost();
    }
}
