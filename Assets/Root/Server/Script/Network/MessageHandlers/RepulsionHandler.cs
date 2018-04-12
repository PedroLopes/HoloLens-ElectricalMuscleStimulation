using UnityEngine;
using System.Collections;
using System;

namespace MuscleDeck
{
    public class RepulsionHandler : AbstractMessageHandler
    {
        protected Side side;

        public virtual void InitParameters(string payloadString)
        {
            RepulsionPayload payload = JsonUtility.FromJson<RepulsionPayload>(payloadString);

            side = payload.side;
        }

        public override void HandleMessage(Message msg)
        {
            InitParameters(msg.payload);

            ApplyStimulation();
        }

        protected virtual void ApplyStimulation()
        {
            for (int i = 0; i < 5; i++)
            {
                ArmStimulation.StimulateArmSinglePulse(
                    Part.Wrist,
                    side,
                    maxPulseWidth,
                    15
                );
            }
        }
    }
}