using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class BumpCollider : ContinuousCollider
    {
        public override MessageType TYPE { get { return MessageType.Bump; } }
    }
}