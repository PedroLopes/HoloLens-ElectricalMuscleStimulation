using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class StickyLedge : MonoBehaviour
    {
        public float threshold = 0.25f;
        public float stickyTime = 0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "InnerCollider")
            {
                PushBoxCollider script = other.GetComponent<PushBoxCollider>();
                if (script)
                {
                    script.threshold = threshold;
                    script.stickyTime = stickyTime;

                    Client.Instance.SendMessage(MessageType.BoxLedge);
                }
            }
        }
    }
}