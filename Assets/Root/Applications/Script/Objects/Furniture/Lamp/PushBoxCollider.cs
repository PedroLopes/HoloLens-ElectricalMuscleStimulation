using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class PushBoxCollider : StickyCollider
    {
        protected Rigidbody rigidBody;
        protected float startDistance;
        public BoxOuterCollider outerCol;
        public float maxSpeed = 2;
        public float minSpeed = 0;
        public float maxDistanceb4Reset = 3f;
        public float threshold = 0.1f;

        public float stickyTime = 0.0f;

        protected Vector3 startPos;
        protected Vector3 startEuler;

        protected override bool OnTriggerEnter(Collider other)
        {
            if (base.OnTriggerEnter(other))
            {
                startDistance = transform.InverseTransformPoint(trackingObject.transform.position).z / transform.localScale.z;

                outerCol.active = false;

                return true;
            }
            return false;
        }

        protected override void Start()
        {
            base.Start();
            rigidBody = transform.parent.GetComponent<Rigidbody>();

            startPos = transform.parent.localPosition;
            startEuler = transform.parent.localEulerAngles;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            if (trackingObject)
            {
                if (stickyTime <= 0)
                {
                    Vector3 direction = transform.InverseTransformPoint(trackingObject.transform.position);

                    float t = Mathf.Min(1,
                        1.2f -
                        Mathf.Abs(direction.z)
                    );

                    //print("Z " + Mathf.Abs(direction.z));
                    //print("scale" + transform.localScale);
                    //print("T" + t);

                    string sourceName = transform.parent.name;

                    if (threshold > 0 && t <= threshold)
                    {
                        t = t / threshold;
                        Client.Instance.SendMessage(MessageType.BoxMoving,
                            new GenericStringPayload(t.ToString(), sourceName));
                        t = 0;
                    }
                    else
                    {
                        Client.Instance.SendMessage(MessageType.BoxMoving,
                            new GenericStringPayload("0", sourceName));
                        threshold = 0;
                    }

                    float speed = Mathf.Lerp(minSpeed, maxSpeed, t) * Time.deltaTime;


                    // apply Force
                    transform.parent.Translate(new Vector3(0, 0, speed), Space.Self);
                }
                else
                {
                    stickyTime -= Time.deltaTime;
                }
            }

            if (transform.parent.localPosition.magnitude > maxDistanceb4Reset)
                ResetCube();
        }

        protected override bool checkDistance()
        {
            return true;
            //Vector3 direction = transform.parent.InverseTransformPoint(trackingObject.transform.position);

            //return Mathf.Abs(direction.z) > Mathf.Abs(startDistance) * 1.1;
        }

        public void ResetCube()
        {
            rigidBody.isKinematic = true;
            transform.parent.localPosition = startPos;
            transform.parent.localEulerAngles = startEuler;
            rigidBody.isKinematic = false;
        }

        protected override void ResetCollider()
        {
            base.ResetCollider();
            Client.Instance.SendMessage(MessageType.Stop);
        }
    }
}