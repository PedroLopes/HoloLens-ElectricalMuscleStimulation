using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public interface IHandlesMessages
    {
        void HandleMessage(Message msg);
    }
}