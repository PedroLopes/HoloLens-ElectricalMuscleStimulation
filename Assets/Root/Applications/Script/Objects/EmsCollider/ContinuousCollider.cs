using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class ContinuousCollider : MonoBehaviour
    {
        /*
        * Private Attributes
        */

        protected float maxDistance = 1.0f;

        protected GameObject trackedObject;

        public virtual MessageType TYPE { get { return MessageType.Continuous; } }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!checkCollider(other))
                return;

            //save maxDistance
            maxDistance = getDistanceToPoint(other.transform.position);

            TrackedHand script = other.gameObject.GetComponent<TrackedHand>();

            if (script != null)
            {
                script.Subscribe(this);
            }

            trackedObject = other.gameObject;
        }

        protected virtual bool Update()
        {
            if (trackedObject)
            {
                if (!checkTag(trackedObject))
                {
                    trackedObject = null;
                    return false;
                }

                // TODO check which hand
                Side side =
                    true ?
                    Side.Right :
                    Side.Left;

                //check distance
                float distance = getDistanceToPoint(trackedObject.transform.position);

                Client.Instance.SendMessage(
                    TYPE,
                    new ContinuousPayload(distance, maxDistance, side)
                );

                return true;
            }

            return false;
        }

        public virtual void OnHandTrackingLost()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            Client.Instance.SendMessage(MessageType.Stop);
            trackedObject = null;
        }

        protected virtual bool OnTriggerExit(Collider other)
        {
            if (!checkCollider(other))
                return false;

            Reset();

            TrackedHand script = other.gameObject.GetComponent<TrackedHand>();

            if (script != null)
            {
                script.Unsubscribe(this);
            }

            return true;
        }

        protected virtual float getDistanceToPoint(Vector3 point)
        {
            //print(transform.InverseTransformPoint(point).z);
            return transform.InverseTransformPoint(point).z;
        }

        protected virtual bool checkCollider(Collider other)
        {
            return checkTag(other.gameObject);
        }

        protected virtual bool checkTag(GameObject gameObject)
        {
            return gameObject.CompareTag("Knob");
        }
    }
}