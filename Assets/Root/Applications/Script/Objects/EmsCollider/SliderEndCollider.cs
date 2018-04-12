using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class SliderEndCollider : ContinuousCollider
    {
        public override MessageType TYPE { get { return MessageType.SliderEnd; } }

        protected override float getDistanceToPoint(Vector3 point)
        {
            return Vector3.Distance(transform.position, point);
        }
    }
}