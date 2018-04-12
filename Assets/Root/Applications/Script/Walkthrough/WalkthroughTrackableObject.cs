using Vuforia;
using UnityEngine;

public class WalkthroughTrackableObject : DefaultTrackableEventHandler
{
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            OnTrackingLost();
    }


    protected override void OnTrackingFound()
    {
        //couch.ResetCube();
    }

    protected override void OnTrackingLost()
    {
        //base.OnTrackingLost();
#if !UNITY_EDITOR
        transform.Translate(new Vector3(0, .5f, 0), Space.Self);
#endif
    }
}
