using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    [RequireComponent(typeof(AudioSource))]
    public class LeverDetent : MonoBehaviour
    {

        public bool isTarget;
        public ButtonReceiver receiver;
        public AudioSource clack;
        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Knob"))
            {
                if (isTarget)
                    receiver.onPress("True");

                GetComponent<AudioSource>().Play();

                Vector3 vector = transform.InverseTransformPoint(other.transform.position);
                Client.Instance.SendMessage(MessageType.EscapeDetent,
                    new VectorPayload(vector, "Detent")
                );
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Knob"))
            {
                if (isTarget)
                    receiver.onPress("False");

                clack.Play();
            }
        }
    }
}