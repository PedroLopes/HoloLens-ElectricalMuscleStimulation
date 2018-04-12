using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class Goal : StickyCollider
    {
        public bool ready = true;
        public int nextLevel;

        protected override bool OnTriggerEnter(Collider other)
        {
            if (!base.OnTriggerEnter(other)
                || !gameObject.activeInHierarchy)
            {
                return false;
            }

            // Victory, send message to Server
            print("Victory");
            Client.Instance.SendMessage(MessageType.MarbleSuccess);
            if (ready)
            {
                GetComponent<AudioSource>().Play();
                ready = false;
                if (nextLevel >= 0)
                    StartCoroutine(toNextLevel());
            }

            return true;
        }

        protected IEnumerator toNextLevel()
        {
            yield return new WaitForSecondsRealtime(2);
            LevelChanger.ChangeLevel(nextLevel);
        }

        protected override bool checkCollider(Collider other)
        {
            return other.CompareTag("Knob");
        }
    }
}