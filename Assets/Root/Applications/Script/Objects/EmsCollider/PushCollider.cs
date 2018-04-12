using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class PushCollider : ContinuousCollider
    {
        public override MessageType TYPE { get { return MessageType.Push; } }

        private Transform root;
        private Transform geometry;
        private Transform couchLengthWrapper;
        private Vector3 startPos;

        private float speed = 0.01f;
        private float speedGeometry = 0.05f;

        protected void Start()
        {
            Init();
        }

        public void Init()
        {
            root = Util.GetParentByName(gameObject, "CouchWrapper").transform;
            geometry = Util.GetSiblingTransformByName(gameObject, "CouchGeometry");
            couchLengthWrapper = geometry.Find("CouchLengthWrapper");
            startPos = transform.localPosition;
        }

        private float lastZ = 9f;

        protected override bool Update()
        {
            transform.localScale = new Vector3
            (
                couchLengthWrapper.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            )
            ;
            if (!base.Update())
            {
                // Move Geometry back

                float delta =
                    //Mathf.Min(
                    //speedGeometry * Time.deltaTime,
                    Mathf.Abs(geometry.localPosition.z)
                    //)
                    ;
                geometry.Translate(new Vector3(0, 0, delta));

                root.Translate(new Vector3(0, 0, -delta));

                transform.localPosition = startPos;

                return false;
            }

            // slowly push away
            transform.Translate(new Vector3(0, 0, -Time.deltaTime * speed));

            lastZ += Time.deltaTime * speed;

            // Move geometry out of the way but only when pushing inwards
            float z = transform.InverseTransformPoint(trackedObject.transform.position).z;

            if (/*true ||*/ lastZ > z)
            {
                //print(z + "/" + lastZ + "/" + (z * 0.99f));

                geometry.localPosition = new Vector3(
                    0,
                    0,
                    z - 0.9f
                );

                lastZ = z;
            }
            else if (z > lastZ * 1.1f)
            {
                Reset();
            }

            return true;
        }

        protected override bool OnTriggerExit(Collider other)
        {
            if (!base.OnTriggerExit(other))
            {
                return false;
            }

            lastZ = 9f;

            return true;
        }

        protected override bool checkTag(GameObject gameObject)
        {
            return gameObject.CompareTag("HandR");
        }
    }
}