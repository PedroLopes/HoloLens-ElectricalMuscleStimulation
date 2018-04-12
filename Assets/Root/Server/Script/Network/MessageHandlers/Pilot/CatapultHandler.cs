using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class CatapultHandler : ContinuousHandler
    {
        public int maxTricepsWidth = -1;
        public int currentTriceps = 15;
        public int maxWidthReleaseT = 250;
        public int maxWidthReleaseW = 100;
        public int offsetRelease = 75;
        public int burstDurationRelease = 200;
        public int wristTricepsDelay = 200;
        public int wristBurstDurationRelease = 200;
        public float growthFactorRelease = 1f;

        public override void HandleMessage(Message msg)
        {
            switch (msg.type)
            {
                case MessageType.CatapultArm:
                    HandleArm(msg);
                    break;

                case MessageType.CatapultRelease:
                    Stop();
                    //HandleRelease(msg);
                    break;

                case MessageType.CatapultTarget:
                    HandleTarget(msg);
                    break;
            }
        }

        protected virtual void HandleArm(Message msg)
        {
            PositionPayload payload = JsonUtility.FromJson<PositionPayload>(msg.payload);

            int width = (int)Mathf.Lerp(offset, maxTricepsWidth, payload.position * growthFactor);

            ArmStimulation.StimulateArm(
                Part.Triceps,
                Side.Right,
                width,
                currentTriceps
            );

            stopTime = System.DateTime.Now.AddMilliseconds(500);
            stimulating = true;
        }

        /*
        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.B))
            {
                StimulationInfo[] infos = new StimulationInfo[]
                {
                    new StimulationInfo(
                        Part.Triceps,
                        Side.Right,
                        200,
                        15
                    ),
                    //new StimulationInfo(
                    //    Part.Wrist,
                    //    Side.Right,
                    //    width,
                    //    current
                    //),
                };
                // Ramp up shortly
                ArmStimulation.StimulateArmBurst(
                    infos,
                    burstDuration
                );
            }
        }
        */

        protected virtual void HandleRelease(Message msg)
        {
            PositionPayload payload = JsonUtility.FromJson<PositionPayload>(msg.payload);

            StartCoroutine(ReleaseStim(payload));
        }

        protected virtual IEnumerator ReleaseStim(PositionPayload payload)
        {
            float t = payload.position * growthFactorRelease;
            int width = (int)Mathf.Lerp(offsetRelease, maxWidthReleaseT, t);

            Debug.Log(width);
            // Ramp up shortly
            ArmStimulation.StimulateArmBurst(
                new StimulationInfo(
                    Part.Triceps,
                    Side.Right,
                    width,
                    currentTriceps
                ),
                burstDurationRelease
            );
            yield return null;
            //yield return new WaitForSecondsRealtime(wristTricepsDelay / 1000f);

            //int width1 = (int)Mathf.Lerp(offsetRelease, maxWidthReleaseW, t);

            //ArmStimulation.StimulateArmBurst(
            //    new StimulationInfo(
            //        Part.Wrist,
            //        Side.Right,
            //        width1,
            //        current
            //    ),
            //    wristBurstDurationRelease
            //);
        }

        protected void HandleTarget(Message msg)
        {
            VectorPayload payload = JsonUtility.FromJson<VectorPayload>(msg.payload);

            Debug.Log("Target hit:" + payload.source);
        }
    }
}   