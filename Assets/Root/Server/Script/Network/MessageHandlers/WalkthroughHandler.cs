using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughHandler : AbstractMessageHandler
    {
        public int current = 15;
        public int outwardsR = 0;
        public int inwardsR = 0;
        public int outwardsL = 0;
        public int inwardsL = 0;

        public int currentColor = -1;
        public int currentIntensity = -1;
        protected int targetIntensity = -1;

        protected bool stimulatingIntensity = false;
        protected bool stimulatingColor = false;
        protected WalkthroughTrackingHandler trackingHandler;
        protected EscapeHandler escapeHandler;

        public void Start()
        {
            trackingHandler = gameObject.GetComponent<WalkthroughTrackingHandler>();
            escapeHandler = gameObject.GetComponent<EscapeHandler>();
        }

        public void Update()
        {
            if (changing && invalidCombination())
            {
                ArmStimulation.StimulateArm(
                    Part.InwardsWrist,
                    Side.Right,
                    inDetentR,
                    current
                );
            }
        }

        public int ButtonI = 200;

        public override void HandleMessage(Message msg)
        {
            if (!isActiveAndEnabled)
                return;

            switch (msg.type)
            {
                case MessageType.WalkthroughDial:
                    HandleDial(msg);
                    break;
                case MessageType.WalkthroughDialDetent:
                    HandleDialDetent(msg);
                    break;
                case MessageType.WalkthroughDialStatus:
                    HandleStatus(msg);
                    break;
                case MessageType.WalkthroughButton:
                    HandleButton(msg);
                    break;
                case MessageType.WalkthroughButtonContact:
                    if (activeButton < DateTime.Now)
                    {
                        //ArmStimulation.StimulateArmBurst(
                        //    new StimulationInfo(
                        //        Part.Biceps,
                        //        Side.Right,
                        //        ButtonI,
                        //        current
                        //    ),
                        //    1000
                        //);

                        ArmStimulation.StimulateArmBurst(
                            new StimulationInfo(
                                Part.InwardsWrist,
                                Side.Right,
                                inwardsR,
                                current
                            ),
                            detentDuration
                        );

                        Debug.Log("Button Press");

                        activeButton = DateTime.Now.AddSeconds(1);
                    }
                    break;
                case MessageType.WalkthroughButtonPosition:
                    trackingHandler.HandleMessage(msg);
                    break;
                case MessageType.EscapeElectro:
                case MessageType.EscapeDetent:
                    escapeHandler.HandleMessage(msg);
                    break;
            }
        }
        protected DateTime activeButton = DateTime.Now;

        public int buttonOffset = -1;
        public int buttonWidth = -1;

        protected void HandleButton(Message msg)
        {
            ContinuousPayload payload = JsonUtility.FromJson<ContinuousPayload>(msg.payload);

            float t = (payload.distance / payload.maxDistance) - 1;
            //Debug.Log(t);
            //return;

            int width = (int)Mathf.Lerp(buttonOffset, buttonWidth, t);

            ArmStimulation.StimulateArm(
                Part.Biceps,
                Side.Right,
                width,
                current
            );
        }

        protected void HandleDial(Message msg)
        {
            GenericStringPayload payload = JsonUtility.FromJson<GenericStringPayload>(msg.payload);

            float t = float.Parse(payload.payload);

            //print(payload.source + ":" + t);

            int width = 0;

            switch (payload.source)
            {
                case "Intensity":
                    if (t > 1 && activeIntensity)
                    {
                        width = (int)Mathf.Lerp(0, inwardsR, t);
                        ArmStimulation.StimulateArm(
                            Part.InwardsWrist,
                            Side.Right,
                            width,
                            current
                        );
                        stimulatingIntensity = true;
                    }
                    else if (t < 0 && activeIntensity)
                    {
                        width = (int)Mathf.Lerp(0, outwardsR, -t);
                        ArmStimulation.StimulateArm(
                            Part.OutwardsWrist,
                            Side.Right,
                            width,
                            current
                        );
                        stimulatingIntensity = true;
                    }
                    else if (stimulatingIntensity)
                    {
                        stimulatingIntensity = false;
                        ArmStimulation.StimulateArm(
                               Part.InwardsWrist,
                               Side.Right,
                               0,
                               0
                           );
                        ArmStimulation.StimulateArm(
                               Part.OutwardsWrist,
                               Side.Right,
                               0,
                               0
                           );
                    }
                    break;
                case "Color":

                    if (t > 1 && activeColor)
                    {
                        width = (int)Mathf.Lerp(0, inwardsL, t);
                        ArmStimulation.StimulateArm(
                            Part.InwardsWrist,
                            Side.Left,
                            width,
                            current
                        );
                        stimulatingColor = true;
                    }
                    else if (t < 0 && activeColor)
                    {
                        width = (int)Mathf.Lerp(0, outwardsL, -t);
                        ArmStimulation.StimulateArm(
                            Part.OutwardsWrist,
                            Side.Left,
                            width,
                            current
                        );
                        stimulatingColor = true;
                    }
                    else if (stimulatingColor)
                    {
                        ArmStimulation.StimulateArm(
                               Part.InwardsWrist,
                               Side.Left,
                               0,
                               0
                           );
                        ArmStimulation.StimulateArm(
                               Part.OutwardsWrist,
                               Side.Left,
                               0,
                               0
                           );

                        stimulatingColor = false;
                    }
                    break;
                case "Thermo":

                    if (t > 1 && activeThermo)
                    {
                        width = (int)Mathf.Lerp(0, inwardsR, t);
                        ArmStimulation.StimulateArm(
                            Part.InwardsWrist,
                            Side.Right,
                            width,
                            current
                        );
                        stimulatingIntensity = true;
                    }
                    else if (t < 0 && activeThermo)
                    {
                        width = (int)Mathf.Lerp(0, outwardsR, -t);
                        ArmStimulation.StimulateArm(
                            Part.OutwardsWrist,
                            Side.Right,
                            width,
                            current
                        );
                        stimulatingIntensity = true;
                    }
                    else if (stimulatingIntensity)
                    {
                        stimulatingIntensity = false;
                        ArmStimulation.StimulateArm(
                               Part.InwardsWrist,
                               Side.Right,
                               0,
                               0
                           );
                        ArmStimulation.StimulateArm(
                               Part.OutwardsWrist,
                               Side.Right,
                               0,
                               0
                           );
                    }
                    break;
            }
        }

        protected DateTime activeTimeIntensity = DateTime.Now;
        protected DateTime activeTimeColor = DateTime.Now;
        protected DateTime activeTimeThermo = DateTime.Now;
        public bool activeIntensity;
        public bool activeColor;
        public bool activeThermo;


        public int cooldown = 500;

        public int outDetentsR = 0;
        public int inDetentR = 0;
        public int outDetentL = 0;
        public int inDetentL = 0;
        public int detentDuration = 150;

        public bool changing;

        public bool invalidCombination()
        {
            return currentColor == 1 && currentIntensity == 3;
        }

        protected void HandleDialDetent(Message msg)
        {
            VectorPayload payload = JsonUtility.FromJson<VectorPayload>(msg.payload);


            Debug.Log(payload.source + (payload.vector.x < 0).ToString());

            switch (payload.source)
            {
                case "Thermo":
                    if (!activeThermo)
                        break;

                    if (activeTimeThermo < DateTime.Now)
                    {
                        if (payload.vector.x < 0)
                        {
                            ArmStimulation.StimulateArmBurst(
                                new StimulationInfo(
                                    Part.InwardsWrist,
                                    Side.Right,
                                    inDetentR,
                                    current
                                ),
                                detentDuration
                            );
                        }
                        else
                        {
                            ArmStimulation.StimulateArmBurst(
                                new StimulationInfo(
                                    Part.OutwardsWrist,
                                    Side.Right,
                                    outDetentsR,
                                    current
                                ),
                                detentDuration
                            );
                        }

                        activeTimeThermo = DateTime.Now.AddMilliseconds(cooldown);
                    }
                    break;
                case "Intensity":
                    currentIntensity = (int)payload.vector.y;

                    if (targetIntensity == currentIntensity)
                    {
                        ChannelList.Stop();
                        targetIntensity = -1;
                        Debug.Log("Snapped to last Intensity");
                    }

                    if (changing && !invalidCombination())
                    {
                        ChannelList.Stop();

                        Debug.Log("Changed To Valid Combination");
                        changing = false;
                    }
                    else if (invalidCombination())
                    {
                        Debug.Log("Invalid Combination");
                        changing = true;
                    }

                    if (!activeIntensity)
                        break;

                    if (activeTimeIntensity < DateTime.Now)
                    {
                        if (payload.vector.x < 0)
                        {
                            ArmStimulation.StimulateArmBurst(
                                new StimulationInfo(
                                    Part.InwardsWrist,
                                    Side.Right,
                                    inDetentR,
                                    current
                                ),
                                detentDuration
                            );
                        }
                        else
                        {
                            ArmStimulation.StimulateArmBurst(
                                new StimulationInfo(
                                    Part.OutwardsWrist,
                                    Side.Right,
                                    outDetentsR,
                                    current
                                ),
                                detentDuration
                            );
                        }

                        activeTimeIntensity = DateTime.Now.AddMilliseconds(cooldown);
                    }
                    break;
                case "Color":
                    if (!activeColor)
                        break;

                    currentColor = (int)payload.vector.y;

                    // snap to intensity 2
                    if (invalidCombination())
                    {
                        Debug.Log("Invalid Combination");
                        changing = true;
                    }

                    if (activeTimeColor < DateTime.Now)
                    {
                        if (payload.vector.x < 0)
                        {
                            ArmStimulation.StimulateArmBurst(
                                new StimulationInfo(
                                    Part.InwardsWrist,
                                    Side.Left,
                                    outDetentL,
                                    current
                                ),
                                detentDuration
                            );
                        }
                        else
                        {
                            ArmStimulation.StimulateArmBurst(
                                new StimulationInfo(
                                    Part.OutwardsWrist,
                                    Side.Left,
                                    inDetentL,
                                    current
                                ),
                                detentDuration
                            );
                        }

                        activeTimeColor = DateTime.Now.AddMilliseconds(cooldown);
                    }
                    break;
            }
        }

        protected void HandleStatus(Message msg)
        {
            GenericStringPayload payload = JsonUtility.FromJson<GenericStringPayload>(msg.payload);

            bool value = bool.Parse(payload.payload);

            switch (payload.source)
            {
                case "Intensity":
                    activeIntensity = value;
                    // check last Detent
                    if (activeIntensity && currentIntensity >= 0)
                    {
                        //go to last detent
                        ArmStimulation.StimulateArm(
                            Part.OutwardsWrist,
                            Side.Right,
                            inDetentR,
                            current
                        );

                        targetIntensity = currentIntensity;
                    }
                    break;
                case "Color":
                    activeColor = value;
                    break;
                case "Thermo":
                    activeThermo = value;
                    break;
            }
        }
    }
}