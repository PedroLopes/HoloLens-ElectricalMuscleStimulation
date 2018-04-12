using System;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class VerticalWall : MonoBehaviour
    {
        protected DateTime enableTime = DateTime.Now;

        protected virtual bool OnCollisionEnter(Collision col)
        {
            if (!col.gameObject.CompareTag("Knob")
                || !gameObject.activeInHierarchy
                || enableTime > DateTime.Now)
                return false;

            Vector3 direction = transform.InverseTransformPoint(col.transform.position);

            direction.Normalize();

            direction *= col.relativeVelocity.magnitude;

            Debug.Log(direction.z);

            Client.Instance.SendMessage(
                MessageType.MarbleWallCollision,
                new VectorPayload(direction, "VerticalWall")
            );

            enableTime = DateTime.Now.AddMilliseconds(1000);

            return true;
        }
    }
}