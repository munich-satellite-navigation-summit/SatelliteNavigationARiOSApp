using System;
using System.Collections;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Satellite control. It make rotation and control satelite.
    /// </summary>
    public class SatelliteControl : RotateControl
    {

        [SerializeField] private SatelliteClickHandler _clickHandler;
        [SerializeField] private float _satelliteMoveToCameraSpeed = 1f;
        [SerializeField] private SatelliteInformationSO _informationSO;


        private Transform _satelliteTransform;
        private Transform _satelliteParentTransform;
        private Transform _pointPositionBeforeCamera;

        private Action<SatelliteInformationSO> _moveToCamAction;
        private Action _moveBackAction;
        private Vector3 _satellitePositionBeforeMoving;

        private bool _isMovedToCam;
        private bool _isClicked;

        /// <summary>
        /// Sets action and point position for the satellite
        /// </summary>
        /// <param name="moveToCamAction">Action - callback function when click on the satellite</param>
        /// <param name="pointBeforeCamera">Position of point before camera where satellite has to move</param>
        public void SetData(Action<SatelliteInformationSO> moveToCamAction, Transform pointBeforeCamera)
        {
            _moveToCamAction = moveToCamAction;
            _pointPositionBeforeCamera = pointBeforeCamera;
            _clickHandler.AddListener(MoveSatelliteToCamera);
            _satelliteTransform = _clickHandler.transform;
            _satelliteParentTransform = _satelliteTransform.parent;
        }

        /// <summary>
        /// Move back the satellite.
        /// </summary>
        /// <param name="moveBackAction">It call function when satellite moved back.</param>
        public void MoveBack(Action moveBackAction)
        {
            _moveBackAction = moveBackAction;
            StartCoroutine(Back());
        }

        /// <summary>
        /// Enable satellite.
        /// </summary>
        public override void Enable()
        {
            _satelliteTransform.gameObject.SetActive(true);
        }

        /// <summary>
        /// Disable satellite if it was not clicked.
        /// </summary>
        public override void Disable()
        {
            if (!_isClicked) _satelliteTransform.gameObject.SetActive(false);
        }


        /// <summary>
        /// Callback function. when click on the satellite  it call this function and start move to the poin before the camera
        /// and call function what was setted in SetDataFunction
        /// </summary>
        private void MoveSatelliteToCamera()
        {
            _isClicked = true;
            if (_moveToCamAction != null)
                _moveToCamAction(_informationSO);
            StartCoroutine(Move());
        }

        /// <summary>
        /// Move satellite to the camera point.
        /// </summary>
        IEnumerator Move()
        {
            _satellitePositionBeforeMoving = _satelliteTransform.position;
            //_satelliteTransform.parent = _pointPositionBeforeCamera;
            Vector3 endPosition = _pointPositionBeforeCamera.position;
            float timer = 0;
            while (timer < 1f)
            {
                _satelliteTransform.position = Vector3.Lerp(_satellitePositionBeforeMoving, _pointPositionBeforeCamera.position, timer);
                timer += Time.deltaTime * _satelliteMoveToCameraSpeed;
                yield return null;
            }
            _satelliteTransform.position = endPosition;
            _isMovedToCam = true;
        }

        /// <summary>
        ///  Move satellite back to the position.
        /// </summary>
        IEnumerator Back()
        {
            if (!_isMovedToCam) yield break;
            //_satelliteTransform.parent = _satelliteParentTransform;
            float timer = 0;
            Vector3 startPosition = _satelliteTransform.position;
            while (timer < 1f)
            {
                _satelliteTransform.position = Vector3.Lerp(startPosition, _satellitePositionBeforeMoving, timer);
                timer += Time.deltaTime * _satelliteMoveToCameraSpeed;
                yield return null;
            }
            _satelliteTransform.position = _satellitePositionBeforeMoving;
            _isMovedToCam = false;
            if (_moveBackAction != null)
                _moveBackAction();
            _isClicked = false;
        }

    }
}
