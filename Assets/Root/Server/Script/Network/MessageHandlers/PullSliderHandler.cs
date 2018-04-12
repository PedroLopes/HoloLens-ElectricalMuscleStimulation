using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class PullSliderHandler : ContinuousHandler
    {
        public int pecsPW = 200;
        public int wristPW = 150;

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
                    diff, maxPen, wristPW));

                width1 = (int)Mathf.Round(getPulseWidth(
                    diff, maxPen, pecsPW));
            }

            ArmStimulation.StimulateArm(
                 Part.Wrist,
                 side,
                 width,
                 15
             );

            ArmStimulation.StimulateArm(
                Part.Triceps,
                side,
                width1,
                15
            );

            stopTime = DateTime.Now.AddMilliseconds(500);
        }
    }
}