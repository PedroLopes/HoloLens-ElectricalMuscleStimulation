using UnityEngine;

namespace MuscleDeck
{
    /**
     * Applies stimulation on fixed channels based on distance to this object's center.
     */

    public class SolidObjectColliderBehaviour : MonoBehaviour
    {
        /*
         * Private Attributes
         */

        protected float maxDistance = 1.0f;
        protected GameObject trackedObject;

        // Use this for initialization
        protected virtual void Start()
        {
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (trackedObject)
            {
                // TODO check which hand
                Side side =
                    true ?
                    Side.Right :
                    Side.Left;

                //check distance
                float distance = getDistanceToPoint(trackedObject.transform.position);

                Client.Instance.SendMessage(
                    GetMessage(distance, side)
                );
            }
        }

        protected virtual Message GetMessage(float distance, Side side)
        {
            return new Message(
                MessageType.Continuous,
                new ContinuousPayload(distance, maxDistance, side)
            );
        }

        protected virtual bool OnTriggerEnter(Collider other)
        {
            if (!CheckObject(other.gameObject))
                return false;

            trackedObject = other.gameObject;

            //save maxDistance
            maxDistance = getDistanceToPoint(other.transform.position);
            maxDistance = Mathf.Abs(maxDistance);

            TrackedHand script = other.gameObject.GetComponent<TrackedHand>();
            if (script != null)
            {
                script.Subscribe(this);
            }

            return true;
        }

        public void OnHandTrackingLost()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            Client.Instance.SendMessage(MessageType.Stop);
            trackedObject = null;
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!CheckObject(other.gameObject))
                return;

            Reset();

            TrackedHand script = other.gameObject.GetComponent<TrackedHand>();

            if (script != null)
            {
                script.Unsubscribe(this);
            }
        }

        protected virtual float getDistanceToPoint(Vector3 point)
        {
            return Vector3.Distance(transform.position, point);
        }

        protected virtual bool CheckObject(GameObject obj)
        {
            return obj.CompareTag("Hand")
                || obj.CompareTag("HandR")
                || obj.CompareTag("HandL");
        }
    }
}