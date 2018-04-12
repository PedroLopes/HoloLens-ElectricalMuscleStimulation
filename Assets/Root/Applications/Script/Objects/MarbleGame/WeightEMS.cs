using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WeightEMS : StickyCollider
    {
        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                //
                Vector3 pos = transform.InverseTransformPoint(trackingObject.transform.position);

                // apply weight ems
                Client.Instance.SendMessage(MessageType.MarblePosition,
                    new VectorPayload(pos, "MarbleGame")
                );
            }
        }

        protected override bool checkCollider(Collider other)
        {
            return other.CompareTag("Knob");
        }

        protected override bool OnTriggerEnter(Collider other)
        {
            if (!base.OnTriggerEnter(other))
                return false;

            //GetComponent<AudioSource>().Play();

            Client.Instance.SendMessage(MessageType.MarbleImpact,
                new VectorPayload(Vector3.down, "InitialImpact")
            );

            return true;
        }

        protected override bool checkDistance()
        {
            return true;
        }

        //protected override bool OnTriggerExit(Collider other)
        //{
        //    if (!checkCollider(other))
        //        return false;

        //    ResetCollider();

        //    return true;
        //}
    }
}