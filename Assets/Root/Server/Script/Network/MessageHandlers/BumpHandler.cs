using UnityEngine;
using System.Collections;
using System;

namespace MuscleDeck
{
    public class BumpHandler : ContinuousHandler
    {
        protected override int defaultMaxWidth { get { return 100; } }

        //protected override void ApplyStimulation()
        //{
        //    stopTime = DateTime.Now.AddMilliseconds(500);
        //}

        //protected override float getPulseWidth(float distance, float maxDistance, float maxValue)
        //{
        //    float result = base.getPulseWidth(distance, maxDistance, maxValue);
        //    //print(result);
        //    return result;
        //}
    }
}