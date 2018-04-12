using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class ButtonCollider : SolidObjectColliderBehaviour
    {
        protected override Message GetMessage(float distance, Side side)
        {
            return new Message(
                MessageType.ButtonUpdate,
                new ContinuousPayload(distance, maxDistance, side)
            );
        }

        protected override bool OnTriggerEnter(Collider other)
        {
            if (!base.OnTriggerEnter(other))
                return false;

            Client.Instance.SendMessage(MessageType.ButtonContact);

            return true;
        }
    }
}   