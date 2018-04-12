using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class EmsButtonScript : ButtonScript
    {
        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                float z = transform.InverseTransformPoint(trackingObject.transform.position).z - startZ;

                MessageType type = GetUpdateMessageType();
                if (type != MessageType.Unknown)
                {
                    Client.Instance.SendMessage(
                        type,
                        new ContinuousPayload(-z, maxPushDepth, Side.Right)
                    );
                }
            }
            else if (Vector3.Distance(childStartPos, child.localPosition) > 0)
            {
                Vector3 targetPos = Vector3.MoveTowards(child.localPosition, childStartPos, speed * Time.deltaTime);
                child.localPosition = targetPos;
            }
            else
            {
                pressed = false;
            }
        }

        protected virtual MessageType GetUpdateMessageType()
        {
            return MessageType.ButtonUpdate;
        }

        protected override bool OnTriggerEnter(Collider other)
        {
            if (!base.OnTriggerEnter(other))
                return false;

            MessageType type = GetContactMessageType();
            if (type != MessageType.Unknown)
            {
                Client.Instance.SendMessage(type);
            }

            return true;
        }

        protected virtual MessageType GetContactMessageType()
        {
            return MessageType.ButtonContact;
        }

        protected override void ResetCollider()
        {
            base.ResetCollider();

            Client.Instance.SendMessage(
                MessageType.Stop
            );
        }
    }
}