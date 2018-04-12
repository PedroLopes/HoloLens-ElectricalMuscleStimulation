using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class BoxHandler : ContinuousHandler
    {
        public int offsetTouchShoulder = -1;
        public int offsetTouchWrist = 0;
        public int touchShoulder = -1;
        public int touchWrist = 200;
        public int couchTouchShoulder = 200;
        public int couchTouchWrist = 200;

        public int currentShoulder = 15;
        public int currentWrist = 15;

        public override void HandleMessage(Message msg)
        {
            switch (msg.type)
            {
                case MessageType.BoxTouching:
                    HandleBoxTouchMsg(msg);
                    break;

                case MessageType.BoxMoving:
                    HandleBoxMovingMsg(msg);
                    break;

                case MessageType.BoxLedge:
                    HandleBoxLedgeMsg(msg);
                    break;
            }
        }

        protected void HandleBoxTouchMsg(Message msg)
        {
            GenericStringPayload payload = JsonUtility.FromJson<GenericStringPayload>(msg.payload);

            float t = float.Parse(payload.payload);
            string src = payload.source;

            int width = (int)Mathf.Lerp(
                offsetTouchShoulder,
                src.Equals("Cube") ? touchShoulder : couchTouchShoulder,
                t * growthFactor);

            int width1 = (int)Mathf.Lerp(
                offsetTouchWrist,
                src.Equals("Cube") ? touchWrist : couchTouchWrist,
                t * growthFactor);

            if (t > 0)
                Debug.Log("Touching: " + (t * growthFactor));

            ArmStimulation.StimulateArm(
                //Part.Triceps,
                Part.Shoulder,
                Side.Right,
                width,
                currentShoulder
            );

            ArmStimulation.StimulateArm(
                Part.Wrist,
                Side.Right, // Wrist Extensor Right
                width1,
                currentWrist
            );

            stopTime = DateTime.Now.AddMilliseconds(500);
            stimulating = true;
        }

        public int moveShoulder = 200;
        public int moveWrist = 200;
        public float moveOffsetShoulder = 50;
        public float moveOffsetWrist = 50;
        public float moveFactor = 1f;

        public int moveCouchShoulder = 200;
        public int moveCouchWrist = 200;
        public float moveCouchOffsetShoulder = 50;
        public float moveCouchOffsetWrist = 50;
        public float moveCouchFactor = 1f;

        protected void HandleBoxMovingMsg(Message msg)
        {
            GenericStringPayload payload = JsonUtility.FromJson<GenericStringPayload>(msg.payload);

            float t = float.Parse(payload.payload);

            Debug.Log("Moving:" + t);

            string src = payload.source;

            int width = (int)Mathf.Lerp(
                src.Equals("Cube") ? moveOffsetShoulder : moveCouchOffsetShoulder,
                src.Equals("Cube") ? moveShoulder : moveCouchShoulder,
                t);
            int width1 = (int)Mathf.Lerp(
                src.Equals("Cube") ? moveOffsetWrist : moveCouchOffsetWrist,
                src.Equals("Cube") ? moveWrist : moveCouchWrist,
                t);

            ArmStimulation.StimulateArm(
                //Part.Triceps,
                Part.Shoulder,
                Side.Right,
                width,
                currentShoulder
            );

            ArmStimulation.StimulateArm(
                Part.Wrist,
                Side.Right,
                //Side.Left, // Wrist extensor
                width1,
                currentWrist
            );

            stopTime = DateTime.Now.AddMilliseconds(500);
        }

        public int ledgeDuration;
        public int ledgeWidthWrist;
        public int ledgeWidthShoulder;

        public void HandleBoxLedgeMsg(Message msg)
        {
            if (ledgeDuration > 0)
            {
                Debug.Log("Ledge");
                int current = 15;
                StimulationInfo[] infos = new StimulationInfo[]
                {
            new StimulationInfo(
                Part.Wrist,
                Side.Right,
                //Side.Left, // Wrist extensor
                ledgeWidthWrist,
                current
            ),
            new StimulationInfo(
                Part.Shoulder,
                Side.Right,
                ledgeWidthShoulder,
                currentWrist
            ),
                };
                // Ramp up shortly
                ArmStimulation.StimulateArmBurst(
                    infos,
                    ledgeDuration
                );
            }
        }
    }
}