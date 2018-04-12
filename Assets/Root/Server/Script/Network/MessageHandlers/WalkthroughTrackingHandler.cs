using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughTrackingHandler : AbstractMessageHandler
    {
        public Transform hand;
        public Transform tracker;

        public override void HandleMessage(Message msg)
        {
            VectorPayload payload = JsonUtility.FromJson<VectorPayload>(msg.payload);

            switch (payload.source)
            {
                case "Position":
                    hand.position = tracker.TransformPoint(payload.vector);
                    break;
                case "Rotation":
                    //hand.localEulerAngles = tracker.InverseTransformDirection(payload.vector);
                    break;
            }
        }
    }
}   