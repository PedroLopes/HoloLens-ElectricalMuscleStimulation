using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class HoleHandler : ContinuousHandler
    {
        protected override int defaultMaxWidth { get { return 100; } }

        protected override void ApplyStimulation()
        {
            offset = 0;
            bool firstHalf = distance < 0 ? true : false;

            maxDistance = Mathf.Abs(maxDistance);
            distance = Mathf.Abs(distance);

            // Reversed: Starts strong but gets weaker in the middle
            distance = Mathf.Max(0, maxDistance - distance);

            int width = (int)Mathf.Round(getPulseWidth(distance, maxDistance, maxPulseWidth));

            //print(firstHalf ? width : -width);

            if (firstHalf)
            {
                ArmStimulation.StimulateArm(
                    Part.Biceps, // should be triceps
                    side,
                    width,
                    15
                );
                ArmStimulation.StimulateArm(
                    Part.Wrist,
                    side,
                    0,
                    15
                );
            }
            else
            {
                ArmStimulation.StimulateArm(
                    Part.Wrist,
                    side,
                    width,
                    15
                );
                ArmStimulation.StimulateArm(
                    Part.Biceps, // should be triceps
                    side,
                    0,
                    15
                );
            }
        }
    }
}