using Vuforia;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughTrackableEventHandler : DefaultTrackableEventHandler
    {
        public PushBoxCollider couch;

        protected override void OnTrackingFound()
        {
            couch.ResetCube();
        }

        protected override void OnTrackingLost()
        {
            //base.OnTrackingLost();
        }
    }

}