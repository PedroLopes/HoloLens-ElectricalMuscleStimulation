using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck{
    public class SphereSpawn : MonoBehaviour
    {
        public Transform sphere;
        public AudioSource countdown;
        public float maxDistance = 5f;
        public Goal goalScript;
        public StartPosition startScript;
        private bool spawning = false;
        private float timer = 2.5f;

        // Use this for initialization
        private void Start()
        {
            // maxDistance = 5f;
        }

        public void StartGame()
        {
            print("starting");
            sphere.GetComponent<Rigidbody>().isKinematic = true;
            timer = 2.5f;
            //sphere.position = transform.position;
            spawning = true;
            countdown.Play();
            startScript.ready = true;
            goalScript.ready = true;
        }

        // Update is called once per frame
        private void Update()
        {
            if (spawning)
            {
                sphere.position = transform.position;
                if (timer <= 0)
                {
                    sphere.GetComponent<Rigidbody>().isKinematic = false;
                    spawning = false;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
            else
            {
                if (transform.parent.InverseTransformPoint(sphere.position).magnitude > maxDistance)
                {
                    StartGame();
                }
            }
        }
    }
}