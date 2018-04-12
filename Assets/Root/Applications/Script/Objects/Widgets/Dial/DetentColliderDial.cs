using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class DetentColliderDial : DetentCollider
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!checkCollider(other))
                return;

            // TODO check which hand
            Side side =
                true ?
                Side.Right :
                Side.Left;

            Client.Instance.SendMessage(
                MessageType.DetentDial,
                new RepulsionPayload(side)
            );
        }
    }
}