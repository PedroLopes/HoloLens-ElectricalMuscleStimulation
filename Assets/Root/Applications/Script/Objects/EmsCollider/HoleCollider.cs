using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class HoleCollider : ContinuousCollider
    {
        public override MessageType TYPE { get { return MessageType.Hole; } }
    }
}   