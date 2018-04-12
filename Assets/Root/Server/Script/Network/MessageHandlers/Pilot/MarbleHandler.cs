using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class MarbleHandler : ContinuousHandler
    {
        public int tricepsL = 250;
        public int tricepsR = 250;
        public int impactWidth;
        public int impactDuration = 300;
        public int wallImpactWidthTriceps;
        public int wallImpactWidthBiceps;

        public int wallImpactDuration = 300;
        public bool active = true;
        public int currentTriceps = 17;

        public override void HandleMessage(Message msg)
        {
            switch (msg.type)
            {
                case MessageType.MarbleImpact:
                    active = true;
                    HandleImpact(msg);
                    break;

                case MessageType.MarblePosition:
                    if (active)
                        HandlePosition(msg);
                    break;

                case MessageType.MarbleSuccess:
                    Stop();
                    active = false;
                    break;

                case MessageType.MarbleWallCollision:
                    if (active)
                        HandleWallCollision(msg);
                    break;
            }
        }

        protected virtual void HandleImpact(Message msg)
        {
            VectorPayload payload = JsonUtility.FromJson<VectorPayload>(msg.payload);

            if (payload.vector.Equals(Vector3.down))
            {
                StimulationInfo[] infos = new StimulationInfo[]
                {
                new StimulationInfo(
                    Part.Triceps,
                    Side.Right,
                    impactWidth,
                    currentTriceps
                ),
                new StimulationInfo(
                    Part.Triceps,
                    Side.Left,
                    impactWidth,
                    currentTriceps
                ),
                };
                // Ramp up shortly
                ArmStimulation.StimulateArmBurst(
                    infos,
                    impactDuration
                );
            }
        }

        public float wallThreshold = 0.05f;
        public int currentBiceps = 18;

        protected virtual void HandleWallCollision(Message msg)
        {
            VectorPayload payload = JsonUtility.FromJson<VectorPayload>(msg.payload);

            if (Mathf.Abs(payload.vector.z) < wallThreshold)
                return;

            // z-Axis looks to the left
            Side sideB = payload.vector.z > 0 ? Side.Left : Side.Right;
            Side sideT = payload.vector.z > 0 ? Side.Right : Side.Left;
            Debug.Log(payload.vector.z);

            StimulationInfo[] infos = new StimulationInfo[]
            {
            new StimulationInfo(
                Part.Triceps,
                sideT,
                wallImpactWidthTriceps,
                currentTriceps
            ),
            new StimulationInfo(
                Part.Biceps,
                sideB,
                wallImpactWidthBiceps,
                currentBiceps
            ),
            };
            // Ramp up shortly
            ArmStimulation.StimulateArmBurst(
                infos,
                wallImpactDuration
            );
        }

        public float positionFactor = 2f;

        protected virtual void HandlePosition(Message msg)
        {
            VectorPayload payload = JsonUtility.FromJson<VectorPayload>(msg.payload);

            float t = (0.5f + payload.vector.x);
            int widthL = (int)Mathf.Lerp(offset, tricepsL, (1 - t) * positionFactor);
            int widthR = (int)Mathf.Lerp(offset, tricepsR, t * positionFactor);

            ArmStimulation.StimulateArm(
                Part.Triceps,
                Side.Left,
                widthL,
                currentTriceps
            );

            ArmStimulation.StimulateArm(
                Part.Triceps,
                Side.Right,
                widthR,
                currentTriceps
            );

            stopTime = DateTime.Now.AddMilliseconds(500);
            stimulating = true;
        }
    }
}