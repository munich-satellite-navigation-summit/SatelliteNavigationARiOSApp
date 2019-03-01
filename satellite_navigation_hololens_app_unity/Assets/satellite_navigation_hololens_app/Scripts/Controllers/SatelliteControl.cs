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
        #region values
        [SerializeField] private SatelliteClickHandler _clickHandler;
        [SerializeField] private float _satelliteMoveToCameraSpeed = 1f;
        [SerializeField] private SatelliteInformationSO _informationSO;

        [SerializeField] private Vector3 _selfSpeedRotation = new Vector3(0f, 0.5f, 0f);

        private Transform _satelliteTransform;
        private Transform _pointPositionBeforeCamera;

        private Action<SatelliteInformationSO> _moveToCamAction;
        private Action _moveBackAction;

        private Vector3 _satellitePositionBeforeMoving;
        private Vector2 _mouse;

        private const float CircleDegrees = 360;
        private float _rotateCoeficient;
        private float _delta;

        private bool _isMovedToCam;
        private bool _isClicked;
        private bool _canRotate;
        private bool _isCourotineStart;
        private bool _isSelfRotation;
        private bool _isCanRotate;
        #endregion

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
            _informationSO.satellite = _clickHandler.gameObject;
            _satelliteTransform = _clickHandler.transform;
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

        private void Awake()
        {
            //_target.rotation = _view.rotation;
            _rotateCoeficient = CircleDegrees / Screen.width;
        }

        #region Moving to the camera

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
            Vector3 endPosition = _pointPositionBeforeCamera.position;
            Vector3 rotation = _satelliteTransform.eulerAngles;
            float timer = 0;
            while (timer < 1f)
            {
                _satelliteTransform.position = Vector3.Lerp(_satellitePositionBeforeMoving, _pointPositionBeforeCamera.position, timer);
                _satelliteTransform.eulerAngles = Vector3.Lerp(rotation, Vector3.zero, timer);
                timer += Time.deltaTime * _satelliteMoveToCameraSpeed;
                yield return null;
            }
            _satelliteTransform.position = endPosition;
            _satelliteTransform.eulerAngles = Vector3.zero;
            _isMovedToCam = true;
            _isSelfRotation = true;
            StartCoroutine(SelfRotation());
            _isCanRotate = true;
            StartCoroutine(HandClickHandler());
        }

        /// <summary>
        ///  Move satellite back to the position.
        /// </summary>
        IEnumerator Back()
        {
            if (!_isMovedToCam) yield break;
            _isCanRotate = false;
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
            _clickHandler.CanClick();
            if (_moveBackAction != null)
                _moveBackAction();
            _isClicked = false;
            _isSelfRotation = false;
        }
        #endregion

        #region SelfRotation
        /// <summary>
        /// Rotate object after change target eangle
        /// </summary>
        IEnumerator Rotate()
        {
            if (_isCourotineStart) yield break;
            _isCourotineStart = true;
            while (_canRotate)
            {
                _satelliteTransform.rotation = Quaternion.Lerp(_satelliteTransform.rotation, _pointPositionBeforeCamera.rotation,
                    Time.deltaTime * _rotateCoeficient * _delta);
                if (Quaternion.Angle(_satelliteTransform.rotation, _pointPositionBeforeCamera.rotation) < 0.1f)
                {
                    _satelliteTransform.rotation = _pointPositionBeforeCamera.rotation;
                    _canRotate = false;
                }
                yield return null;
            }
            _isCourotineStart = false;
            _isSelfRotation = true;
            StartCoroutine(SelfRotation());
        }

        /// <summary>
        /// Selfs the rotation. Rotate objects all the time when hand rotation is absent.
        /// </summary>
        IEnumerator SelfRotation()
        {
            _pointPositionBeforeCamera.rotation = _satelliteTransform.rotation;
            while (_isSelfRotation)
            {
                _satelliteTransform.Rotate(_selfSpeedRotation);
                _pointPositionBeforeCamera.rotation = _satelliteTransform.rotation;
                yield return null;
            }
        }

        /// <summary>
        /// This function catch all touches for rotation
        /// </summary>
        private IEnumerator HandClickHandler()
        {
            while (_isCanRotate)
            {
                if (Input.touchCount == 1)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        _mouse = Input.mousePosition;
                        _pointPositionBeforeCamera.rotation = _satelliteTransform.rotation;
                        _canRotate = false;
                    }

                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        _isSelfRotation = false;

                        _delta = -Input.mousePosition.x + _mouse.x;
                        _pointPositionBeforeCamera.Rotate(Vector3.up, _delta * 0.1f);

                        if (_delta < 0)
                            _delta *= -1;
                        _mouse = Input.mousePosition;
                        _canRotate = true;
                        StartCoroutine(Rotate());
                    }
                }
                yield return null;
            }
        }
        #endregion
    }
}
