using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MuscleDeck
{
    public class ButtonTracking : StickyCollider
    {
        protected DateTime activeTime = DateTime.Now;
        public int cooldown = 100;

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (trackingObject
                && (activeTime < DateTime.Now))
            {
                Vector3 pos = transform.InverseTransformPoint(trackingObject.transform.position);

                Client.Instance.SendMessage(
                    MessageType.WalkthroughButtonPosition,
                    new VectorPayload(pos, "Position")
                );

                Vector3 euler = transform.InverseTransformDirection(trackingObject.transform.eulerAngles);

                Client.Instance.SendMessage(
                    MessageType.WalkthroughButtonPosition,
                    new VectorPayload(euler, "Rotation")
                );

                activeTime = DateTime.Now.AddMilliseconds(cooldown);
            }
        }
    }
}