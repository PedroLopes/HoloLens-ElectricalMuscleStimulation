using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public abstract class StickyCollider : MonoBehaviour
    {
        public GameObject trackingObject;
        protected float maxDistance;
        protected bool tracking = false;

        protected virtual void Start()
        {
        }

        protected virtual bool OnTriggerEnter(Collider other)
        {
            if (!checkCollider(other)
               || trackingObject)
                return false;

            trackingObject = other.gameObject;

            TrackedHand script = other.GetComponent<TrackedHand>();

            tracking = true;

            if (script)
                script.Subscribe(this);
            return true;
        }

        protected virtual void Update()
        {
            if (trackingObject
                && !tracking
                && checkDistance())
            {
                ResetCollider();
            }
        }

        protected virtual bool OnTriggerExit(Collider other)
        {
            if (!checkCollider(other))
                return false;

            maxDistance = Vector3.Distance(transform.position, other.transform.position);

            tracking = false;

            return true;
        }

        protected virtual bool checkDistance()
        {
            float distance = Vector3.Distance(transform.position, trackingObject.transform.position);

            return distance > 1.2f * maxDistance;
        }

        protected virtual bool checkCollider(Collider other)
        {
            return other.gameObject.CompareTag("HandR")
                ;
        }

        protected virtual void OnHandTrackingLost()
        {
            ResetCollider();
        }

        protected virtual void ResetCollider()
        {
            trackingObject = null;
        }
    }
}