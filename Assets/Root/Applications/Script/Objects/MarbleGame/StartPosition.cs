using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class StartPosition : StickyCollider
    {
        public bool ready = true;

        protected override bool OnTriggerEnter(Collider other)
        {
            if (!base.OnTriggerEnter(other))
            {
                return false;
            }

            if (ready)
            {
                GetComponent<ParticleSystem>().Emit(200);
                GetComponent<AudioSource>().Play();
                ready = false;
            }

            return true;
        }

        protected override bool checkCollider(Collider other)
        {
            return other.CompareTag("Knob");
        }
    }
}