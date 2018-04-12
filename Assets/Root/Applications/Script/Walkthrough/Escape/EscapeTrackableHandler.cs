using System.Collections;
using Vuforia;
using UnityEngine;

namespace MuscleDeck
{
    public class EscapeTrackableHandler : DefaultTrackableEventHandler
    {
        public Lever[] subscribers;

        protected override void OnTrackingFound()
        {
            //base.OnTrackingFound();
        }

        protected override void OnTrackingLost()
        {
            foreach (var lever in subscribers)
            {
                lever.OnTrackingLost();
            }
            transform.Translate(new Vector3(0, 5), Space.World);
        }
    }
}

