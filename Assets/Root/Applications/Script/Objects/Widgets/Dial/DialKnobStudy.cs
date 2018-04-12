using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class DialKnobStudy : DialKnob
    {
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                float angle = GetAngle();

                Client.Instance.SendMessage(MessageType.DialPosition,
                    new PositionPayload(angle, "dial")
                );
            }
        }
    }
}