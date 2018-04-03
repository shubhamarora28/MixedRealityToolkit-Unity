﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using UnityEngine;
using UnityEngine.XR.iOS;

namespace HoloToolkit.ARCapture
{
    /// <summary>
    /// Detects when an anchor has been located
    /// </summary>
    public class AnchorLocated : MonoBehaviour
    {
        /// <summary>
        /// Delegate for when an achor is located
        /// </summary>
        public delegate void AnchorLocatedEvent();

        /// <summary>
        /// The 3D marker generator
        /// </summary>
        [SerializeField] [Tooltip("The 3D marker generator")]
        private ARCAMarkerGenerator3D markerGenerator;

        /// <summary>
        /// Callback when an anchor is located by the HoloLens
        /// </summary>
        public AnchorLocatedEvent OnAnchorLocated;

        /// <summary>
        ///
        /// </summary>
        private bool transitioned;

        /// <summary>
        /// The 3D marker generator
        /// </summary>
        public ARCAMarkerGenerator3D MarkerGenerator
        {
            get { return markerGenerator; }
            set { markerGenerator = value; }
        }

        private void Start()
        {
            if (MarkerGenerator == null) MarkerGenerator = FindObjectOfType<ARCAMarkerGenerator3D>();
            UnityARSessionNativeInterface.ARFrameUpdatedEvent += FrameUpdated;
        }

        private void OnDestroy()
        {
            UnityARSessionNativeInterface.ARFrameUpdatedEvent -= FrameUpdated;
        }

        /// <summary>
        /// Called by the API. It checks whether an anchor has been located and signals
        /// the marker generator so that it can create and show an AR marker
        /// </summary>
        /// <param name="cam"></param>
        private void FrameUpdated( UnityARCamera cam )
        {
            if (cam.pointCloudData.Length > 4)
            {
                if (OnAnchorLocated != null) OnAnchorLocated();
                if (!transitioned)
                {
                    MarkerGenerator.StartTransition();
                    transitioned = true;
                }
            }
        }
    }
}
