using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Satellite control. It make rotation and control satelite.
    /// </summary>
    public class SatelliteControl : RotateControl
    {
        #region values
        [SerializeField] private List<SatelliteClickHandler> _clickHandlers;
        [SerializeField] private GameObject _orbit;
        [SerializeField] private float _satelliteMoveToCameraSpeed = 1f;
        [SerializeField] private Vector3 _selfSpeedRotation = new Vector3(0f, 0.5f, 0f);

        [SerializeField] private float _zoomingCoefficient = 4f;
        [SerializeField] private float _zoomingSpeed = 0.5f;

        private SatelliteClickHandler _satelliteClickHandler;
        private Transform _pointPositionBeforeCamera;

        private Action<SatelliteInformationSO> _moveToCamAction;
        private Action _moveBackAction;
        private SatelliteInformationSO _informationSO;
        private Vector3 _satelliteScale;
        private Vector3 _zoomMax;
        private Vector3 _zoomMin;
        private Vector3 _satellitePositionBeforeMoving;
        private Vector2 _mouse;

        private const float CircleDegrees = 360;
        private float _rotateCoeficient;
        private float _delta;

        private bool _isMovedToCam;
        private bool _canRotate;
        private bool _isCourotineStart;
        private bool _isSelfRotation;
        private bool _isCanRotate;
        private bool _isZooming;
        private bool _canZoom;

        #endregion

        /// <summary>
        /// Init the specified moveToCamAction and pointBeforeCamera.
        /// </summary>
        /// <param name="moveToCamAction">Move to cam action.</param>
        /// <param name="pointBeforeCamera">Point before camera.</param>
        public void Init(Action<SatelliteInformationSO> moveToCamAction, Transform pointBeforeCamera)
        {
            EnableElements(false);
            _moveToCamAction = moveToCamAction;
            _pointPositionBeforeCamera = pointBeforeCamera;
            for (int i = 0; i < _clickHandlers.Count; i++)
            {
                _clickHandlers[i].AddListener(MoveSatelliteToCamera);
            }
        }


        /// <summary>
        /// Shows satelites and orbit, start rotate satellites
        /// </summary>
        public void ShowAndStartRotate()
        {
            EnableElements(true);
            StartRotation();
            for (int i = 0; i < _clickHandlers.Count; i++)
                _clickHandlers[i].Enable();
        }

        /// <summary>
        /// Hides satelites and orbit, stop rotate satellites
        /// </summary>
        public void HideAndStopRotate()
        {
            EnableElements(false);
            EndRotation();
            for (int i = 0; i < _clickHandlers.Count; i++)
                _clickHandlers[i].Disable();
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
        /// Shows the hide orbits.
        /// </summary>
        /// <param name="isShow">If set to <c>true</c> is show.</param>
        public void ShowHideOrbits(bool isShow)
        {
            _orbit.SetActive(isShow);
        }


        public void DennyClick(bool isDenny)
        {
            Debug.Log("Denny " + name);
            for (int i = 0; i < _clickHandlers.Count; i++)
                _clickHandlers[i].DennyClick(isDenny);
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
        private void MoveSatelliteToCamera(SatelliteClickHandler satellite, SatelliteInformationSO informationSO)
        {
            informationSO.satellite = satellite.gameObject;
            Debug.Log(satellite.name);
            if (_moveToCamAction != null)
                _moveToCamAction(informationSO);
            _satelliteClickHandler = satellite;

            _zoomMax = _satelliteClickHandler.transform.localScale * _zoomingCoefficient;
            _zoomMin = _satelliteClickHandler.transform.localScale / _zoomingCoefficient;

            StartCoroutine(Move());
        }

        /// <summary>
        /// Enables or disable the satellites and orbit.
        /// </summary>
        /// <param name="isEnable">If set to <c>true</c> is enable ssatelites and orbit else disable.</param>
        private void EnableElements(bool isEnable)
        {
            for (int i = 0; i < _clickHandlers.Count; i++)
            {
                if (isEnable)
                    _clickHandlers[i].Enable();
                else
                    _clickHandlers[i].Disable();
            }
            _orbit.SetActive(isEnable);
        }

        /// <summary>
        /// Move satellite to the camera point.
        /// </summary>
        IEnumerator Move()
        {
            _satellitePositionBeforeMoving = _satelliteClickHandler.transform.position;
            Debug.Log("MOVE " + _satelliteClickHandler.IsEnabled);
            Vector3 endPosition = _pointPositionBeforeCamera.position;
            Vector3 rotation = _satelliteClickHandler.transform.eulerAngles;
            _satelliteScale = _satelliteClickHandler.transform.localScale;
            float timer = 0;
            while (timer < 1f)
            {
                _satelliteClickHandler.transform.position = Vector3.Lerp(_satellitePositionBeforeMoving, _pointPositionBeforeCamera.position, timer);
                _satelliteClickHandler.transform.eulerAngles = Vector3.Lerp(rotation, Vector3.zero, timer);
                timer += Time.deltaTime * _satelliteMoveToCameraSpeed;
                yield return null;
            }
            _satelliteClickHandler.transform.position = endPosition;
            _satelliteClickHandler.transform.eulerAngles = Vector3.zero;
            _isMovedToCam = true;
            _isSelfRotation = true;
            StartCoroutine(SelfRotation());
            _isCanRotate = true;
            _canZoom = true;
            StartCoroutine(HandClickHandler());
        }

        /// <summary>
        ///  Move satellite back to the position.
        /// </summary>
        IEnumerator Back()
        {
            if (!_isMovedToCam) yield break;
            _canZoom = false;
            _isCanRotate = false;
            float timer = 0;
            Vector3 startPosition = _satelliteClickHandler.transform.position;
            Vector3 currentScale = _satelliteClickHandler.transform.localScale;
            while (timer < 1f)
            {
                _satelliteClickHandler.transform.position = Vector3.Lerp(startPosition, _satellitePositionBeforeMoving, timer);
                _satelliteClickHandler.transform.localScale = Vector3.Lerp(currentScale, _satelliteScale, timer);
                timer += Time.deltaTime * _satelliteMoveToCameraSpeed;
                yield return null;
            }

            _satelliteClickHandler.transform.localScale = _satelliteScale;
            _satelliteClickHandler.transform.position = _satellitePositionBeforeMoving;
            _isMovedToCam = false;
            _satelliteClickHandler.DennyClick();

            if (_moveBackAction != null)
                _moveBackAction();

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
                _satelliteClickHandler.transform.rotation = Quaternion.Lerp(_satelliteClickHandler.transform.rotation, _pointPositionBeforeCamera.rotation,
                    Time.deltaTime * _rotateCoeficient * _delta);
                if (Quaternion.Angle(_satelliteClickHandler.transform.rotation, _pointPositionBeforeCamera.rotation) < 0.1f)
                {
                    _satelliteClickHandler.transform.rotation = _pointPositionBeforeCamera.rotation;
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
            _pointPositionBeforeCamera.rotation = _satelliteClickHandler.transform.rotation;
            while (_isSelfRotation)
            {
                _satelliteClickHandler.transform.Rotate(_selfSpeedRotation);
                _pointPositionBeforeCamera.rotation = _satelliteClickHandler.transform.rotation;
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
                        _pointPositionBeforeCamera.rotation = _satelliteClickHandler.transform.rotation;
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

        #region ZoomIn/Out

        /// <summary>
        /// Start zooming in.
        /// </summary>
        public void ZoomInBegin()
        {
            if (!_canZoom) return;
            _isZooming = true;
            StartCoroutine(Zooming(true));
        }

        /// <summary>
        /// Stop zooming in.
        /// </summary>
        public void ZoomInEnd()
        {
            _isZooming = false;
        }

        /// <summary>
        /// Start zooming out.
        /// </summary>
        public void ZoomOutBegin()
        {
            if (!_canZoom) return;
            _isZooming = true;
            StartCoroutine(Zooming(false));
        }

        /// <summary>
        /// Stop zooming out.
        /// </summary>
        public void ZoomOutEnd()
        {
            _isZooming = false;
        }

        /// <summary>
        /// Zooming the specified isZoomIn.
        /// </summary>
        /// <returns>The zooming.</returns>
        /// <param name="isZoomIn">If set to <c>true</c> is zoom in else it zooming out.</param>
        private IEnumerator Zooming(bool isZoomIn)
        {
            Vector3 startScale = _satelliteClickHandler.transform.localScale;
            Vector3 endScale = isZoomIn ? _zoomMax : _zoomMin;
            float timer = 0;
            while (_isZooming)
            {
                Debug.Log("_satelliteClickHandler   " + _satelliteClickHandler.name + "  localScale " + _satelliteClickHandler.transform.localScale);
                _satelliteClickHandler.transform.localScale = Vector3.MoveTowards(startScale, endScale, timer);
                timer += Time.deltaTime * _zoomingSpeed;
                yield return null;
            }
        }
        #endregion
    }
}
