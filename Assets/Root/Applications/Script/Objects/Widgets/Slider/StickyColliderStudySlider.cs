using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class StickyColliderStudySlider : StickyColliderSlider
    {
        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                float z = parentRail.transform.InverseTransformPoint(trackingObject.transform.position).z
                   - zOffset;
                // min Displacement
                z = Mathf.Max(0, z);
                // max Displacement
                z = Mathf.Min(0.45f, z);

                Client.Instance.SendMessage(MessageType.SliderPosition,
                    new PositionPayload(z, "Slider"));
            }
        }
    }
}   