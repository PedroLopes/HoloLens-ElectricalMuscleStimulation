using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughThermoKnob : WalkthroughDialKnob
    {

        protected override bool checkCollider(Collider other)
        {
            return other.gameObject.CompareTag("HandL")
                || other.gameObject.CompareTag("HandR");
        }

        protected override string GetSource()
        {
            return "Thermo";
        }
    }
}