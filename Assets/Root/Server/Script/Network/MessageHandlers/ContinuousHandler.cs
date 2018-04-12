using UnityEngine;
using System.Collections;
using System;

namespace MuscleDeck
{
    public class ContinuousHandler : AbstractMessageHandler
    {
        /**
         * Safety Check incase the stop packet was lost
         */
        protected DateTime stopTime;

        [Header("Scaling Stuff")]
        public int offset = 50;

        /**
         * Parameters
         */
        protected Side side;
        protected float distance;
        protected float maxDistance;
        protected bool stimulating;

        public virtual void InitParameters(string payloadString)
        {
            ContinuousPayload payload = JsonUtility.FromJson<ContinuousPayload>(payloadString);

            side = payload.side;

            distance = payload.distance;
            maxDistance = payload.maxDistance;
        }

        public override void HandleMessage(Message msg)
        {
            InitParameters(msg.payload);

            ApplyStimulation();
        }

        protected virtual void ApplyStimulation()
        {
            distance = Mathf.Abs(distance);
            maxDistance = Mathf.Abs(maxDistance);

            int width = (int)Mathf.Round(getPulseWidth(distance, maxDistance, maxPulseWidth));

            ArmStimulation.StimulateArm(
                Part.Wrist,
                side,
                width,
                15
            );

            stopTime = DateTime.Now.AddMilliseconds(500);
            stimulating = true;
        }

        protected virtual void Update()
        {
            if (stimulating && stopTime < DateTime.Now)
            {
                stimulating = false;
                Stop();
            }
        }

        protected virtual void GetPart(float distance, float maxDistance)
        {
        }

        public virtual void Stop()
        {
            //Debug.Log("Stopping");
            ChannelList.Stop();
        }

        protected virtual float getPulseWidth(float distance, float maxDistance, float maxValue)
        {
            float factor;

            factor = getLinearScaling(distance, maxDistance);

            return Mathf.Min(maxValue, factor * maxValue + offset);
        }

        /**
         * @return a value between 0 and 1
         */

        public float growthFactor = 1;

        protected virtual float getLinearScaling(float distance, float maxDistance)
        {
            return Mathf.Min(1, (1 - (distance / maxDistance)) * growthFactor);
        }
    }
}