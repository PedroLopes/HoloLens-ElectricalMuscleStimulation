using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class DetentCollider : MonoBehaviour
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
                MessageType.DetentSlider,
                new RepulsionPayload(side)
            );
        }

        protected virtual bool checkCollider(Collider other)
        {
            return other.gameObject.CompareTag("Knob");
        }
    }
}