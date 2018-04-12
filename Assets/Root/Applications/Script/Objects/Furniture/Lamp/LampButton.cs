using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class LampButton : EmsButtonScript
    {
        protected override void OnPressed()
        {
            base.OnPressed();

            Light light = GetComponent<Light>();
            light.enabled = !light.enabled;
        }
    }
}