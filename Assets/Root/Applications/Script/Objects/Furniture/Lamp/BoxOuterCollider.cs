using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class BoxOuterCollider : StickyCollider
    {
        public bool active = true;

        protected override void Update()
        {
            if (trackingObject && active)
            {
                //// send ems message
                //float distance = Vector3.Distance(transform.position, trackingObject.transform.position);

                float distance =
                    transform.InverseTransformPoint(trackingObject.transform.position).z;

                string sourceName = transform.parent.name;
                float t = (1 - Mathf.Abs(distance)) / 1;

                Client.Instance.SendMessage(MessageType.BoxTouching,
                    new GenericStringPayload(t.ToString(), sourceName)
                //new ContinuousPayload(distance, maxDistance, Side.Right)
                );
            }
        }

        protected override bool OnTriggerExit(Collider other)
        {
            if (!checkCollider(other))
                return false;

            ResetCollider();
            active = true;

            return true;
        }
    }
}