using System;
using System.Collections;
using UnityEngine;

namespace MuscleDeck
{
    [RequireComponent(typeof(AudioSource))]
    public class ElectroColliderBehaviour : MonoBehaviour
    {
        private DateTime lastShock;

        /**
         * in ms
         */
        public float cooldown = 1000f;

        public Flash flashEffect;

        private void OnTriggerStay(Collider other)
        {
            if (!
                (other.gameObject.CompareTag("Hand")
                || other.gameObject.CompareTag("HandR")
                || other.gameObject.CompareTag("HandL")
                )
                || (DateTime.Now - lastShock).Milliseconds < cooldown
            )
                return;

            // TODO check which hand

            Side side =
                other.gameObject.CompareTag("HandR") ?
                Side.Right :
                Side.Left;

            //Client.Instance.SendString(ColliderTypeConstants.ELEC + "#" + side + ",hand");
            Client.Instance.SendMessage(
                MessageType.Electro,
                new RepulsionPayload(side)
            );

            lastShock = DateTime.Now;
            GetComponent<AudioSource>().Play();
            flashEffect.alpha = 1;
        }
    }
}