using System;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck{
    public class MarbleGameTrackableEventHandler : Vuforia.DefaultTrackableEventHandler
    {
        protected DateTime disableTime = DateTime.Now;
        public bool tracked = true;
        public SphereSpawn spawn;
        protected bool startable = true;

        //public Goal goalScript;

        protected override void OnTrackingFound()
        {
            if (startable)
            {
                //spawn.StartGame();
                LevelChanger.Reset(disableTime.AddSeconds(7) < DateTime.Now);
                startable = false;
            }
            tracked = true;
            transform.parent.SendMessage("Enable");
        }

        protected void Update()
        {
            if (tracked && Vector3.Angle(transform.up, Vector3.down) < 45)
            {
                OnTrackingLost();
            }

            if (!tracked && disableTime < DateTime.Now)
            {
                startable = true;
                transform.parent.SendMessage("Disable");
            }
        }

        protected override void OnTrackingLost()
        {
            disableTime = DateTime.Now.AddSeconds(3);
            tracked = false;
            Debug.Log("Lost Tracking");
        }
    }
}