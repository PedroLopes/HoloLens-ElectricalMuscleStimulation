using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class EscapeHandler : AbstractMessageHandler
    {
        public override void HandleMessage(Message msg)
        {
            //if (!isActiveAndEnabled)
            //    return;

            Debug.Log(msg.ToString());

            switch (msg.type)
            {
                case MessageType.EscapeDetent:
                    HandleDetent(msg);
                    break;
                case MessageType.EscapeElectro:
                    HandleElectro(msg);
                    break;
            }
        }

        protected void Update() { }

        [Header("Detents")]
        public int widthDetentT = -1;
        public int currentDetentT = 15;
        public int widthDetentB = -1;
        public int currentDetentB = 15;
        public int detentDuration = 150;
        public int cooldownDetent = 500;

        protected DateTime activeDetent = DateTime.Now;

        protected void HandleDetent(Message msg)
        {
            VectorPayload payload = JsonUtility.FromJson<VectorPayload>(msg.payload);

            if (activeDetent < DateTime.Now)
            {
                if (payload.vector.x < 0)
                {
                    ArmStimulation.StimulateArmBurst(
                        new StimulationInfo(
                            Part.Biceps,
                            Side.Right,
                            widthDetentB,
                            currentDetentB
                        ),
                        detentDuration
                    );
                }
                else
                {
                    ArmStimulation.StimulateArmBurst(
                        new StimulationInfo(
                            Part.Triceps,
                            Side.Right,
                            widthDetentT,
                            currentDetentT
                        ),
                        detentDuration
                    );
                }
                activeDetent = DateTime.Now.AddMilliseconds(cooldownDetent);
            }
        }

        [Header("Repulsion")]
        public int widthBiceps = -1;
        public int currentBiceps = 15;
        public int durationBiceps = 300;

        protected void HandleElectro(Message msg)
        {
            ArmStimulation.StimulateArmBurst(
                new StimulationInfo(
                    Part.Biceps,
                    Side.Right,
                    widthBiceps,
                    currentBiceps),
                durationBiceps
            );
        }
    }
}