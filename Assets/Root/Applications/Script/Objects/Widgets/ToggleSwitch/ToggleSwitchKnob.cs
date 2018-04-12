using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class ToggleSwitchKnob : DialKnob
    {
        protected float startAngle = 0;

        protected override bool OnTriggerEnter(Collider other)
        {
            bool result = base.OnTriggerEnter(other);

            if (result)
            {
                startAngle = GetAngle();
            }

            return result;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (trackingObject)
            {
                float angle = GetAngle();
                if (Client.Instance)
                {
                    Client.Instance.SendMessage(
                        MessageType.ToggleSwitch,
                        new ToggleSwitchPayload(angle, startAngle, Side.Right)
                    );
                }
            }
            else
            {
                float currentAngle = (arrowRoot.transform.localEulerAngles.z + 360) % 360;

                //float targetAngle = Mathf.LerpAngle(currentAngle, currentAngle > 180 ? 285 : 75, Time.deltaTime);

                arrowRoot.transform.localEulerAngles = new Vector3(
                    arrowRoot.transform.localEulerAngles.x,
                    arrowRoot.transform.localEulerAngles.y,
                    currentAngle);

                transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            }
        }

        protected override bool checkDistance()
        {
            float distance = Vector3.Distance(transform.position, trackingObject.transform.position);

            return distance > 0.1;
        }
    }
}