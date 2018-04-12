using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughColorKnob : WalkthroughDialKnob
    {
        public WalkthroughLamp lamp;

        protected override void Start()
        {
            base.Start();

            Vector3 euler = bar.transform.localEulerAngles;
            euler.z = -minAngle;
            bar.transform.localEulerAngles = euler;

            bar.material.SetFloat("_Cutoff", 0.1f);
        }
        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                float t = getT();
                lamp.SetHaloColor(t);
            }
        }


        protected override string GetSource()
        {
            return "Color";
        }


        protected override bool checkCollider(Collider other)
        {
            return other.transform.parent.name.Equals("CupColor");
        }
    }
}