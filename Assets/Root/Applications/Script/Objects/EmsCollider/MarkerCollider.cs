using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class MarkerCollider : MonoBehaviour
    {
        private bool armed = true;

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
                MessageType.Electro,
                new RepulsionPayload(side)
            );
        }

        private bool checkCollider(Collider other)
        {
            return other.gameObject.CompareTag("Knob");
        }
    }
}