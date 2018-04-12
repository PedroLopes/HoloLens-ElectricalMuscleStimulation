using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughLampButton : LampButton
    {
        public bool sendUpdate = false;
        public bool sendContact = true;

        protected override MessageType GetUpdateMessageType()
        {
            return sendUpdate ?
                MessageType.WalkthroughButton :
                MessageType.Unknown;
            //return MessageType.WalkthroughButton;
        }

        protected override MessageType GetContactMessageType()
        {
            return sendContact ?
                MessageType.WalkthroughButtonContact :
                MessageType.Unknown;
        }

        protected override bool checkDistance()
        {
            return true;
        }
    }
}