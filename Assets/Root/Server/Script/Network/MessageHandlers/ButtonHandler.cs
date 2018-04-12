using UnityEngine;
using System;
using System.Collections;

namespace MuscleDeck
{
    public class ButtonHandler : ContinuousHandler
    {
        //public int wristContactPW = 120;
        //public int shoulderContactPW = 120;
        //public int contactPC = 15;

        //public int updatePC = 15;
        public int shoulderUpdatePW = 200;

        public int wristUpdatePW = 200;

        protected override void ApplyStimulation()
        {
            // how far the button was pressed
            distance = Mathf.Max(0, distance);
            // max push depth
            maxDistance = Mathf.Abs(maxDistance);
            int width, width1;

            if (distance < maxDistance)
            {
                // constant while able to push
                width = offset;
                width1 = offset;
            }
            else
            {
                // scale for up to 10cm penetration
                float maxPen = 0.1f;
                float diff = Mathf.Max(0, maxDistance + maxPen - distance);

                width = (int)Mathf.Round(getPulseWidth(
                    diff, maxPen, wristUpdatePW));

                width1 = (int)Mathf.Round(getPulseWidth(
                    diff, maxPen, shoulderUpdatePW));
            }

            ArmStimulation.StimulateArm(
                 Part.Wrist,
                 side,
                 width,
                 15
             );

            ArmStimulation.StimulateArm(
                Part.Shoulder,
                side,
                width1,
                15
            );

            stopTime = DateTime.Now.AddMilliseconds(500);
        }

        public override void HandleMessage(Message msg)
        {
            if (!gameObject.activeInHierarchy)
                return;
            switch (msg.type)
            {
                case MessageType.ButtonContact:
                    /*
                    ContinuousPayload payload = JsonUtility.FromJson<ContinuousPayload>(msg.payload);

                    // send single pulse
                    ArmStimulation.StimulateArmSinglePulse(
                        Part.Wrist,
                        payload.side,
                        wristContactPW,
                        15
                    );
                    ArmStimulation.StimulateArmSinglePulse(
                        Part.Shoulder,
                        payload.side,
                        wristContactPW,
                        15
                    );
                    //*/
                    break;

                case MessageType.ButtonUpdate:
                    base.HandleMessage(msg);
                    break;
            }
        }
    }
}