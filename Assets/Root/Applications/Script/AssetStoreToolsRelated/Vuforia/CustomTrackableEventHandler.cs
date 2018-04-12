using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using Vuforia;

public class CustomTrackableEventHandler : DefaultTrackableEventHandler
{
    public bool isTracked = false;
    public GameObject hand;

    //public void OnTrackableStateChanged(
    //          TrackableBehaviour.Status previousStatus,
    //          TrackableBehaviour.Status newStatus)
    //{
    //    switch (newStatus)
    //    {
    //        case TrackableBehaviour.Status.NOT_FOUND:
    //        case TrackableBehaviour.Status.UNDEFINED:
    //        case TrackableBehaviour.Status.UNKNOWN:
    //            OnTrackingLost();
    //            break;

    //        case TrackableBehaviour.Status.DETECTED:
    //        case TrackableBehaviour.Status.TRACKED:
    //            OnTrackingFound();
    //            break;
    //    }
    //}

    private void Update()
    {
        if (hand && !HandsTrackingManager.Instance.HandDetected && isTracked)
        {
            hand.SetActive(true);
            hand.transform.position = transform.position;
            //hand.transform.rotation = Quaternion.LookRotation(-transform.up, transform.forward);
            hand.transform.rotation = Camera.main.transform.localRotation;
        }
    }

    protected override void OnTrackingFound()
    {
        isTracked = true;
        //base.OnTrackingFound();
    }

    protected override void OnTrackingLost()
    {
        isTracked = false;
        //base.OnTrackingLost();

        if (hand && !HandsTrackingManager.Instance.HandDetected)
        {
            hand.BroadcastMessage("OnHandTrackingLost", null, SendMessageOptions.DontRequireReceiver);

            HandsTrackingManager.Instance.uiText.SetActive(false);
        }
    }
}