// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// HandsManager determines if the hand is currently detected or not.
    /// </summary>
    public partial class HandsTrackingManager : Singleton<HandsTrackingManager>
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>
        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }

        public GameObject uiText;

        private Transform HandWrapper;

        private HashSet<uint> trackedHands = new HashSet<uint>();

        // Vuforia stuff
        private CustomTrackableEventHandler vuforiaHandler;

        private void Awake()
        {
            InteractionManager.SourceDetected += InteractionManager_SourceDetected;
            InteractionManager.SourceLost += InteractionManager_SourceLost;
            InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
        }

        private Quaternion toQuat;

        private void Start()
        {
            HandWrapper = transform.GetChild(0);
            vuforiaHandler = GetComponent<CustomTrackableEventHandler>();
        }

        private void InteractionManager_SourceUpdated(InteractionSourceState state)

        {
            uint id = state.source.id;
            Vector3 pos;

            if (state.source.kind == InteractionSourceKind.Hand)
            {
                if (trackedHands.Contains(state.source.id))
                {
                    if (state.properties.location.TryGetPosition(out pos))
                    {
                        toQuat = Camera.main.transform.localRotation;
                        //toQuat.x = 0;
                        //toQuat.z = 0;
                        HandWrapper.transform.position = pos;
                        HandWrapper.transform.rotation = toQuat;
                    }
                }
            }
        }

        protected virtual void InteractionManager_SourceDetected(InteractionSourceState state)
        {
            // Check to see that the source is a hand.
            if (state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }
            trackedHands.Add(state.source.id);

            Vector3 pos;
            if (state.properties.location.TryGetPosition(out pos))
            {
                HandWrapper.transform.position = pos;
                HandWrapper.BroadcastMessage("OnHandTrackingDetected", null, SendMessageOptions.DontRequireReceiver);
                HandWrapper.gameObject.SetActive(true);
                uiText.SetActive(true);
            }
        }

        private void InteractionManager_SourceLost(InteractionSourceState state)
        {
            // Check to see that the source is a hand.
            if (state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }

            if (trackedHands.Contains(state.source.id))
            {
                trackedHands.Remove(state.source.id);
            }


            if (!vuforiaHandler.isTracked)
            {
                uiText.SetActive(false);
                HandWrapper.BroadcastMessage("OnHandTrackingLost", null, SendMessageOptions.DontRequireReceiver);
            }
        }

        private void OnDestroy()
        {
            InteractionManager.SourceDetected -= InteractionManager_SourceDetected;
            InteractionManager.SourceLost -= InteractionManager_SourceLost;
            InteractionManager.SourceUpdated -= InteractionManager_SourceUpdated;
        }
    }
}