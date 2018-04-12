using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughIntensityKnob : WalkthroughDialKnob
    {
        public WalkthroughLamp lamp;

        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                float t = getT();
                lamp.SetHaloSize(t);
            }
        }


        protected override string GetSource()
        {
            return "Intensity";
        }


        protected override bool checkCollider(Collider other)
        {
            return other.transform.parent.name.Equals("CupIntensity");
        }
    }
}
