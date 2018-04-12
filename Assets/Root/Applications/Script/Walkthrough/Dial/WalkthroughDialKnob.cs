using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughDialKnob : StickyCollider
    {
        public float minAngle = 0f;
        public float maxAngle = 180f;
        public Renderer bar;

        protected LinkedList<Vector3> samples;
        protected int sampleCount = 4;

        protected override void Start()
        {
            base.Start();

            samples = new LinkedList<Vector3>();

            Vector3 euler = bar.transform.localEulerAngles;
            euler.z = -minAngle;
            bar.transform.localEulerAngles = euler;
            bar.material.SetFloat("_Cutoff", 0.1f);

            bar.gameObject.SetActive(false);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                float angle = GetAngle();

                Vector3 euler = transform.localEulerAngles;

                euler.z = angle;
                transform.localEulerAngles = euler;

                if (!trackingObject.activeInHierarchy)
                    ResetCollider();
            }
        }

        protected void AddSample(Vector3 sample)
        {
            // delete the first entry before adding
            if (samples.Count >= sampleCount)
            {
                samples.RemoveFirst();
            }

            samples.AddLast(sample);
        }

        protected Vector3 GetAverage()
        {
            Vector3 result = new Vector3();

            foreach (Vector3 sample in samples)
            {
                result += sample;
            }

            result = result / samples.Count;

            return result;
        }

        protected float GetAngle(bool clamp = true)
        {
            if (trackingObject)
            {
                Vector3 direction = transform.parent.InverseTransformDirection(trackingObject.transform.forward);

                direction.z = 0;

                AddSample(direction);

                direction = GetAverage();

                float angle = Mathf.Rad2Deg * Mathf.Acos(direction.y / direction.magnitude);

                angle *= -Mathf.Sign(direction.x);

                if (clamp)
                    angle = Mathf.Clamp(angle, minAngle, maxAngle);

                return angle;
            }

            return 0;
        }

        protected override bool checkDistance()
        {
            return true;
        }

        protected override bool OnTriggerEnter(Collider other)
        {
            if (!base.OnTriggerEnter(other))
                return false;

            bar.gameObject.SetActive(true);

            Client.Instance.SendMessage(MessageType.WalkthroughDialStatus,
                 new GenericStringPayload(true.ToString(), GetSource()));

            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            return true;
        }

        protected override bool OnTriggerExit(Collider other)
        {
            if (!base.OnTriggerExit(other))
                return false;

            bar.gameObject.SetActive(false);
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            return true;
        }

        protected virtual string GetSource()
        {
            return "knob";
        }

        protected float getT()
        {
            if (trackingObject)
            {
                float angle = GetAngle(false);

                // check math
                float t = (angle - minAngle) / (maxAngle - minAngle);

                if (t < 0 || t > 1)
                    Client.Instance.SendMessage(
                        MessageType.WalkthroughDial,
                        new GenericStringPayload(
                            t.ToString(),
                             GetSource()
                        ));

                t = Mathf.Clamp(t, 0, 1);

                return t;
            }

            return 0f;
        }

        // Needs to be overwritten
        protected override bool checkCollider(Collider other)
        {
            return false;
        }

        protected override void ResetCollider()
        {
            base.ResetCollider();

            Client.Instance.SendMessage(MessageType.WalkthroughDialStatus,
                new GenericStringPayload(false.ToString(), GetSource()));
        }
    }
}