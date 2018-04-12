using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class Lever : DialKnob
    {


        protected override bool checkDistance()
        {
            return true;
        }

        public void OnTrackingLost()
        {
            ResetCollider();
        }

        protected override float GetAngle()
        {
            return Mathf.Clamp(base.GetAngle(), 0, 90);
        }
    }
}
