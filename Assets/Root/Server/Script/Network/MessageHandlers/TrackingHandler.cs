using UnityEngine;
using System.Collections.Generic;
using System;

namespace MuscleDeck
{
    public class TrackingHandler : AbstractMessageHandler, IHandlesMessages
    {
        protected Dictionary<string, GameObject> trackedObjects;

        public override void HandleMessage(Message msg)
        {
            TrackingPayload payload = JsonUtility.FromJson<TrackingPayload>(msg.payload);
            string name = payload.name;

            GameObject obj;
            if (!trackedObjects.TryGetValue(name, out obj))
            {
                obj = GameObject.Find(name);
                trackedObjects.Add(name, obj);
            }

            if (obj)
            {
                obj.transform.position = payload.position;
                obj.transform.rotation = payload.rotation;
            }
        }
    }
}