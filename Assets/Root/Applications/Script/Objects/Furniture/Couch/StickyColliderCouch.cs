using UnityEngine;
using System.Collections;
using System;

namespace MuscleDeck
{
    public class StickyColliderCouch : StickyCollider
    {
        private Transform lengthWrapper;
        private Transform root;
        private float offset;
        protected GameObject knob;

        protected override void Start()
        {
            lengthWrapper = Util.GetSiblingTransformByName(transform.parent.gameObject, "CouchLengthWrapper");

            root = Util.GetParentByName(gameObject, "CouchCenter").transform;

            knob = Util.GetSiblingByName(transform.parent.gameObject, "Knob");
        }

        protected override bool OnTriggerEnter(Collider other)
        {
            if (!base.OnTriggerEnter(other))
                return false;

            knob.tag = "Knob";
            offset = 0.0f;
            offset = transform.parent.lossyScale.x;
            offset += Mathf.Sign(offset) * other.transform.lossyScale.z;

            return true;
        }

        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                Vector3 scale = lengthWrapper.localScale;
                scale.x = (
                    Math.Abs(root.InverseTransformPoint(trackingObject.transform.position).x * 2)
                    - offset
                );
                lengthWrapper.localScale = scale;
            }
        }

        /**
         * Returns true if object is too far away
         */

        protected override bool checkDistance()
        {
            float distance = Vector3.Distance(transform.position, trackingObject.transform.position);

            return distance > 1.5f * maxDistance;
        }

        protected override void ResetCollider()
        {
            base.ResetCollider();
            knob.tag = "Untagged";
        }
    }
}