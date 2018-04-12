using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class StickyColliderSlider : StickyCollider
    {
        protected GameObject parentKnob;
        protected GameObject parentRail;
        protected GameObject parentMesh;

        protected float zOffset;
        public float maxDisplacement = .45f;

        protected override void Start()
        {
            parentKnob = Util.GetParentByName(gameObject, "KnobOrigin");
            parentRail = Util.GetParentByName(gameObject, "Rail");
            parentMesh = transform.parent.gameObject;
            maxDisplacement = 0.45f;
        }

        protected override bool OnTriggerEnter(Collider other)
        {
            if (!base.OnTriggerEnter(other))
                return false;

            zOffset = transform.localPosition.z * transform.parent.lossyScale.z;
            zOffset += Mathf.Sign(zOffset) * other.transform.lossyScale.z * 0.5f;

            //print(zOffset);
            // enable Tag
            parentMesh.tag = "Knob";
            return true;
        }

        protected override bool checkCollider(Collider other)
        {
            return other.gameObject.CompareTag("HandR") &&
                !parentMesh.CompareTag("Knob");
            ;
        }

        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                Vector3 pos = parentKnob.transform.localPosition;
                float z = GetDisplacement();
                // min Displacement
                z = Mathf.Max(0, z);
                // max Displacement
                z = Mathf.Min(maxDisplacement, z);

                pos.z = z;

                //parentMesh.GetComponent<Rigidbody>().MovePosition(parentRail.transform.TransformPoint(pos));
                parentKnob.transform.position = parentRail.transform.TransformPoint(pos);
            }
        }

        protected virtual float GetDisplacement()
        {
            float z = parentRail.transform.InverseTransformPoint(trackingObject.transform.position).z
                   - zOffset;

            return z;
        }

        protected override void ResetCollider()
        {
            base.ResetCollider();
            parentMesh.tag = "Untagged";
        }

        protected override bool checkDistance()
        {
            float distance = Vector3.Distance(transform.position, trackingObject.transform.position);

            return distance > 1.4f * maxDistance;
        }
    }
}