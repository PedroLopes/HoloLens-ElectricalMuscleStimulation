using Vuforia;
using UnityEngine;

namespace MuscleDeck{
    public class CouchTrackableEventBehaviour : DefaultTrackableEventHandler
    {
        public PushBoxCollider pushCollider;

        protected override void OnTrackingLost()
        {
            //base.OnTrackingLost();
        }

        protected override void OnTrackingFound()
        {
            pushCollider.ResetCube();
            pushCollider.threshold = 0f;
        }
    }
}