using System;
using UnityEngine;

namespace MuscleDeck
{
        [Serializable]

    public class Message
    {
        public MessageType type = MessageType.Unknown;
        public string payload;

        public Message(MessageType type)
        {
            this.type = type;
            payload = "";
        }

        public Message(MessageType type, string payload)
        {
            this.type = type;
            this.payload = payload;
        }

        public Message(MessageType type, BasicPayload payload)
        {
            this.type = type;
            this.payload = payload.ToString();
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    public enum MessageType
    {
        Unknown = 0,
        Bump,
        Hole,
        Solid,
        Electro,
        SliderEnd,
        Tracking,
        Continuous,
        Stop,
        DetentSlider,
        Push,
        ToggleSwitch,
        ButtonContact,
        ButtonUpdate,
        DetentDial,

        // User Study

        SliderPosition,
        DialPosition,
        ButtonGridCode,
        ButtonGridSuccess,
        GridButtonContact,
        GridButtonUpdate,
        GridButtonCodeContact,
        GridButtonCodeUpdate,

        //Catapult

        PullSlider,
        Munition,
        CatapultArm,
        CatapultRelease,
        CatapultTarget,

        //Box Game

        BoxTouching,
        BoxMoving,
        BoxLedge,

        // Marble Game

        MarbleImpact,
        MarblePosition,
        MarbleSuccess,
        MarbleWallCollision,

        // Walkthrough

        WalkthroughDial,
        WalkthroughDialDetent,
        WalkthroughDialStatus,
        WalkthroughButton,
        WalkthroughButtonContact,
        WalkthroughButtonPosition,

        // Escape

        EscapeElectro,
        EscapeDetent,

    }
}