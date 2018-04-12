using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class WalkthroughElectro : MonoBehaviour
    {
        protected DateTime activeTime = DateTime.Now;

        public Renderer flash;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("HandR") && activeTime < DateTime.Now)
            {
                Client.Instance.SendMessage(MessageType.EscapeElectro);
                activeTime = DateTime.Now.AddMilliseconds(500);

                Vector3 pos = transform.InverseTransformPoint(other.transform.position);
                pos.x = flash.transform.localPosition.x;
                flash.transform.localPosition = pos;

                StartCoroutine(Flash());
            }
        }

        public float fadeTime = 1f;
        protected IEnumerator Flash()
        {
            flash.GetComponent<AudioSource>().Play();

            Color color = flash.material.GetColor("_TintColor");
            //color.a = 1;
            //flash.material.SetColor("_TintColor", color);

            float time = fadeTime;

            while (time > 0)
            {
                float t = time / fadeTime;

                color.a = Mathf.Lerp(0, 1, t);
                flash.material.SetColor("_TintColor", color);

                time -= Time.deltaTime;

                yield return null;
            }


            color.a = 0;
            flash.material.SetColor("_TintColor", color);

            yield return null;
        }
    }
}   