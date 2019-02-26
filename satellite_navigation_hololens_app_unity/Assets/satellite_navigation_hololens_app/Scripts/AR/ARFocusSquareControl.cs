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

        [SerializeField] private GameObject _findingSquare;

        //[SerializeField] private LayerMask _collisionLayerMask;
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
                _findingSquare.SetActive(_squareState != FocusState.Found && _squareState != FocusState.Stoped);
            }
        }
        private FocusState _squareState;

        private bool _isTrackingInitialized;

        public void Init()
        {
            var layerIndex = LayerMask.NameToLayer("ARGameObject");
            //_collisionLayerMask = 1 << layerIndex;s
            SquareState = FocusState.Initializing;
            _isTrackingInitialized = true;
            //Disable();
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
                Debug.Log("SquareState == FocusState.Stoped");
                return;
            }
            //use center of screen for focusing
            var center = new Vector3(Screen.width / 2, Screen.height / 2, _findingSquareDist);
            //var screenPosition = Camera.main.ScreenToWorldPoint(center);



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

                //check camera forward is facing downward
                if (Vector3.Dot(_camera.transform.forward, Vector3.down) > 0)
                {
                    //position the focus finding square a distance from camera and facing up
                    _findingSquare.transform.position = _camera.ScreenToWorldPoint(center);

                    //vector from camera to focussquare
                    var vecToCamera = _findingSquare.transform.position - _camera.transform.position;

                    //find vector that is orthogonal to camera vector and up vector
                    var vecOrthogonal = Vector3.Cross(vecToCamera, Vector3.up);

                    //find vector orthogonal to both above and up vector to find the forward vector in basis function
                    var vecForward = Vector3.Cross(vecOrthogonal, Vector3.up);

                    _findingSquare.transform.rotation = Quaternion.LookRotation(vecForward, Vector3.up);
                }
                else
                {
                    //we will not display finding square if camera is not facing below horizon
                    _findingSquare.SetActive(false);
                }
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
