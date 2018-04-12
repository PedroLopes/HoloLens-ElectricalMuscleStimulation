using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public abstract class AbstractMessageHandler : MonoBehaviour, IHandlesMessages
    {
        public int maxPulseWidth { get { return (userDefinedWidth > 0) ? userDefinedWidth : defaultMaxWidth; } }
        protected virtual int defaultMaxWidth { get { return 200; } }
        public int userDefinedWidth = -1;

        public abstract void HandleMessage(Message msg);
    }
}