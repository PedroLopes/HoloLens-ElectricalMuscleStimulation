using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class TrackedHand : MonoBehaviour
    {
        private HashSet<MonoBehaviour> subscribers = new HashSet<MonoBehaviour>();
        private GameObject root;

        // Use this for initialization
        private void Start()
        {
            subscribers = new HashSet<MonoBehaviour>();
            root = Util.GetParentByName(gameObject, "HandWrapper");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
                OnHandTrackingLost();
        }

        public void Subscribe(MonoBehaviour subscriber)
        {
            subscribers.Add(subscriber);
#if UNITY_EDITOR
            //print("subscribed:" + subscriber.name);
#endif
        }

        public void Unsubscribe(MonoBehaviour subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public void OnHandTrackingLost()
        {
            if (subscribers != null)
            {
                foreach (var subcriber in subscribers)
                {
                    subcriber.SendMessage("OnHandTrackingLost");
                }

                subscribers.Clear();
                subscribers.TrimExcess();
            }
            //Failsafe
            if (Client.Instance)
                Client.Instance.SendMessage(MessageType.Stop);

            if (root)
                root.SetActive(false);
        }

        public void OnHandTrackingDetected()
        {
            root.SetActive(true);
        }
    }
}