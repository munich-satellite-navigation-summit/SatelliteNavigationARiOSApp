using System;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

namespace Controllers.ARScene
{
    public class ARFocusSquareControl : MonoBehaviourWrapper
    {
        public enum FocusState
        {
            Initializing,
            Finding,
            Found,
            Stoped
        }

        [SerializeField] private float _findingSquareDist = 0.5f;
        [SerializeField] private Camera _camera;

        [SerializeField] private ARSessionOrigin m_SessionOrigin;

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        public event Action<Vector3, Quaternion> FoundPlace;
        public event Action LoseFocuse;
        public event Action<FocusState> ChangedFocusState;

        private FocusState SquareState
        {
            get { return _squareState; }
            set
            {
                _squareState = value;
                if (ChangedFocusState != null)
                {
                    ChangedFocusState(_squareState);
                }
            }
        }
        private FocusState _squareState;

        private bool _isTrackingInitialized;

        public void Init()
        {
            var layerIndex = LayerMask.NameToLayer("ARGameObject");
            SquareState = FocusState.Initializing;
            _isTrackingInitialized = true;
        }

        public void StartTracking()
        {
            Debug.Log("StartTracking");
            SquareState = FocusState.Finding;
        }

        public void StopTracking()
        {
            SquareState = FocusState.Stoped;
        }

        private void Update()
        {
            if (SquareState == FocusState.Stoped)
            {
                return;
            }
            //use center of screen for focusing
            var center = new Vector3(Screen.width / 2, Screen.height / 2, _findingSquareDist);

            if (HitTestWithResultType(TrackableType.FeaturePoint))
            {
                return;
            }

            if (LoseFocuse != null)
            {
                LoseFocuse.Invoke();
            }

            //if you got here, we have not found a plane, so if camera is facing below horizon, display the focus "finding" square
            if (_isTrackingInitialized)
            {
                SquareState = FocusState.Finding;
            }
        }

        private bool HitTestWithResultType(TrackableType resultTypes)
        {

            var center = new Vector3(Screen.width / 2, Screen.height / 2, _findingSquareDist);
            if (m_SessionOrigin.Raycast(center, s_Hits, resultTypes))
            {
                Debug.Log("HitTestWithResultType  " + s_Hits.Count);
                Pose hitPose = s_Hits[0].pose;
                SquareState = FocusState.Found;

                // This does not move the content; instead, it moves and orients the ARSessionOrigin
                // such that the content appears to be at the raycast hit position.

                if (FoundPlace != null)
                {
                    FoundPlace.Invoke(hitPose.position, hitPose.rotation);
                }
                return true;
            }

            return false;
        }

    }
}