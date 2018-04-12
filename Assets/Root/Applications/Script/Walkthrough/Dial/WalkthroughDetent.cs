using System;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughDetent : MonoBehaviour
    {
        public string sender = "";
        public uint number = 0;

        //private DateTime activeTime = DateTime.Now;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Knob")
                /*|| DateTime.Now < activeTime*/
                || other.transform.parent.parent != transform.parent.parent)
                return;

            Vector3 direction = transform.InverseTransformPoint(other.transform.position);

            direction.y = number;

            Client.Instance.SendMessage(MessageType.WalkthroughDialDetent,
                new VectorPayload(direction, sender));

            //activeTime = DateTime.Now.AddMilliseconds(500);
        }
    }
}