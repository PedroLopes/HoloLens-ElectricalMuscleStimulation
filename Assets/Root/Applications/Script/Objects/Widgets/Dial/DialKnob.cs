using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class DialKnob : StickyCollider
    {
        protected GameObject root;
        protected GameObject arrowRoot;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            root = Util.GetParentByName(gameObject, "Origin");
            arrowRoot = Util.GetParentByName(gameObject, "ArrowWrapper");
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                float angle = GetAngle();

                ApplyTransformation(angle);
            }
        }

        protected virtual void ApplyTransformation(float angle)
        {
            Vector3 eulerFinal = arrowRoot.transform.localEulerAngles;
            eulerFinal.z = angle;
            arrowRoot.transform.localEulerAngles = eulerFinal;

            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        }

        protected virtual float GetAngle()
        {
            // project onto root plane
            Vector3 pos = root.transform.InverseTransformPoint(trackingObject.transform.position);
            //minDistance = Mathf.Abs(pos.z) + 0.1f;
            pos.z = 0;

            float angle = Mathf.Rad2Deg * Mathf.Acos(pos.y / pos.magnitude);

            angle *= -Mathf.Sign(pos.x);

            return angle;
        }

        protected override bool checkCollider(Collider other)
        {
            return other.gameObject.CompareTag("FingerTip");
        }
    }
}