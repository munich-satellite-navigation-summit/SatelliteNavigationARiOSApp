using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buttons;
using Helpers;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace Controllers.ARScene
{
    public class ModelsControl : MonoBehaviour
    {
        [SerializeField] private List<ModelView> _modelList;
        [SerializeField] private RotateEarthControl _rotateEarthControl;


        [SerializeField] private Transform _pointBeforeCameraTransform;
        [SerializeField] private Button _backButton;
        [SerializeField] private InformationControl _informationControl;

        [Header("Lists of satellites")]
        [SerializeField] private List<SatelliteControl> _galileoSatellites;
        [SerializeField] private List<SatelliteControl> _gpsSatellites;
        [SerializeField] private List<SatelliteControl> _glonassSatellites;
        [SerializeField] private List<SatelliteControl> _beidouSatellites;
        [SerializeField] private float _pauseTimeBeforeShowAllSatellites = 2f;

        [Header("ZoomIn ZoomOut")]
        [SerializeField] private ButtonHandler _zoomIn;
        [SerializeField] private ButtonHandler _zoomOut;
        [SerializeField] private Vector3 _zoomMax = new Vector3(2f, 2f, 2f);
        [SerializeField] private Vector3 _zoomMin = new Vector3(0.5f, 0.5f, 0.5f);
        [SerializeField] private float _zoomingSpeed = 0.5f;

        [Header("Show orbits")]
        [SerializeField] private Button _showHideOrbitsButton;
        [SerializeField] private Text _showHideOrbitsText;
        [SerializeField] private string _showText = "Show";
        [SerializeField] private string _hideText = "Hide";

        [Header("Scenarios Button")]
        [SerializeField] private Button _weiterStepButton;
        [SerializeField] private Button _zuruckStepButton;


        private CanvasGroup _buttonCanvasGroup;
        private Dictionary<ModelView.ModelType, ModelView> _models;
        private List<List<SatelliteControl>> _scenariosObjects = new List<List<SatelliteControl>>();

        private bool _isZooming;
        private bool _canZoom;
        private bool _isShowOrbits;

        private int _scenariosIndex;


        public void Init()
        {
            _models = _modelList.ToDictionary(obj => obj.Type, obj => obj);
            _buttonCanvasGroup = _backButton.GetComponent<CanvasGroup>();
            _buttonCanvasGroup.SetActive(false);
            InitSatellites(_galileoSatellites);
            InitSatellites(_gpsSatellites);
            InitSatellites(_glonassSatellites);
            InitSatellites(_beidouSatellites);
            _showHideOrbitsButton.gameObject.SetActive(false);
            _zoomIn.gameObject.SetActive(false);
            _zoomOut.gameObject.SetActive(false);
            Hide();
            _weiterStepButton.gameObject.SetActive(false);
            _zuruckStepButton.gameObject.SetActive(false);
            _isShowOrbits = true;
            //Create Scenarios
            _scenariosObjects.Add(_galileoSatellites);
            _scenariosObjects.Add(_gpsSatellites);
            _scenariosObjects.Add(_glonassSatellites);
            _scenariosObjects.Add(_beidouSatellites);
        }

        #region Show

        public void Show(ModelView.ModelType type, Vector3 position, Quaternion rotation)
        {
            try
            {
                Move(type, position);
                Rotate(type, rotation);
                _models[type].Show();
                if (type == ModelView.ModelType.Earth)
                {
                    StartCoroutine(ShowAfterPlace());
                }
                else
                {
                    _showHideOrbitsButton.gameObject.SetActive(false);
                    _zoomIn.gameObject.SetActive(false);
                    _zoomOut.gameObject.SetActive(false);
                    Hide(ModelView.ModelType.Earth);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void Show(ModelView.ModelType type, Vector3 position)
        {
            Show(type, position, Quaternion.identity);
        }

        public void Show(ModelView.ModelType type)
        {
            Show(type, Vector3.zero, Quaternion.identity);
        }

        #endregion

        #region Hide

        public void Hide(ModelView.ModelType type)
        {
            try
            {
                _models[type].Hide();
            }
            catch (Exception e)
            {

                Debug.LogError("Try to Hide " + type + "\n" + e);
            }
        }

        public void Hide()
        {
            foreach (var item in _models)
            {
                item.Value.Hide();
            }
        }

        #endregion

        #region Transform

        public void Move(ModelView.ModelType type, Vector3 position)
        {
            _models[type].Move(position);
        }

        public void Rotate(ModelView.ModelType type, Quaternion rotation)
        {
            _models[type].Rotate(rotation);
        }

        public Vector3 GetPostion(ModelView.ModelType type)
        {
            return _models[type].Position;
        }

        public Quaternion GetRotation(ModelView.ModelType type)
        {
            return _models[type].Rotation;
        }

        #endregion



        #region Models Controll actions
        private void OnEnable()
        {
            _zoomIn.AddListenerOnPointerDown(ZoomInBegin);
            _zoomIn.AddListenerOnPointerUp(ZoomInEnd);
            _zoomOut.AddListenerOnPointerDown(ZoomOutBegin);
            _zoomOut.AddListenerOnPointerUp(ZoomOutEnd);
            _backButton.onClick.AddListener(ContinueMoving);
            _showHideOrbitsButton.onClick.AddListener(ShowHideOrbits);
            AddZoomingListeners(_galileoSatellites);
            _weiterStepButton.onClick.AddListener(NextState);
            _zuruckStepButton.onClick.AddListener(PrevState);
        }

        private void OnDisable()
        {
            _showHideOrbitsButton.onClick.RemoveListener(ShowHideOrbits);
            _zoomIn.RemoveAllListeners();
            _zoomOut.RemoveAllListeners();
            _backButton.onClick.RemoveListener(ContinueMoving);
            _weiterStepButton.onClick.RemoveListener(NextState);
            _zuruckStepButton.onClick.RemoveListener(PrevState);
        }

        /// <summary>
        /// Adds the zooming listeners.
        /// </summary>
        /// <param name="satellites">Satellites.</param>
        private void AddZoomingListeners(List<SatelliteControl> satellites)
        {
            for (int i = 0; i < satellites.Count; i++)
            {
                _zoomIn.AddListenerOnPointerDown(satellites[i].ZoomInBegin);
                _zoomIn.AddListenerOnPointerUp(satellites[i].ZoomInEnd);
                _zoomOut.AddListenerOnPointerDown(satellites[i].ZoomOutBegin);
                _zoomOut.AddListenerOnPointerUp(satellites[i].ZoomOutEnd);
            }
        }

        /// <summary>
        /// Initialize the satellites.
        /// </summary>
        /// <param name="satellites">Satellites.</param>
        private void InitSatellites(List<SatelliteControl> satellites)
        {
            for (int i = 0; i < satellites.Count; i++)
            {
                satellites[i].Init(StopRotations, _pointBeforeCameraTransform);
            }
            DennyClick(satellites);
        }

        /// <summary>
        /// Start rotate all elements.
        /// </summary>
        private void RotateAllElements()
        {
            _canZoom = true;
            _buttonCanvasGroup.SetActive(false);
            _rotateEarthControl.Enable();
            _rotateEarthControl.StartRotation();

            ShowAndRotate(_galileoSatellites, true);
            ShowAndRotate(_gpsSatellites, true);
            ShowAndRotate(_glonassSatellites, true);
            ShowAndRotate(_beidouSatellites, true);
            _showHideOrbitsButton.gameObject.SetActive(true);
            //_zoomIn.gameObject.SetActive(true);
            //_zoomOut.gameObject.SetActive(true);
        }

        /// <summary>
        /// Shows the satellites and rotate.
        /// </summary>
        /// <param name="isShow">If set to <c>true</c> is show and rotate satellites else it hide and stop rotation</param>
        private void ShowAndRotate(List<SatelliteControl> satellites, bool isShow)
        {
            for (int i = 0; i < satellites.Count; i++)
            {
                if (isShow)
                {
                    satellites[i].ShowAndStartRotate();
                }
                else
                {
                    satellites[i].HideAndStopRotate();
                }
            }
        }

        /// <summary>
        /// Denies the click.
        /// </summary>
        /// <param name="satellites">Satellites.</param>
        /// <param name="isDeny">If set to <c>true</c> is deny.</param>
        private void DennyClick(List<SatelliteControl> satellites, bool isDeny = true)
        {
            for (int i = 0; i < satellites.Count; i++)
            {
                satellites[i].DennyClick(isDeny);
            }
        }

        /// <summary>
        /// Stops the rotations.
        /// </summary>
        /// <param name="info">Info about satellite.</param>
        private void StopRotations(SatelliteInformationSO info)
        {
            _canZoom = false;
            _rotateEarthControl.EndRotation();
            _rotateEarthControl.Disable();
            _buttonCanvasGroup.SetActive(true);
            _informationControl.Enable();
            _informationControl.transform.parent = info.satellite.transform;
            _informationControl.transform.localPosition = Vector3.zero;
            _informationControl.transform.eulerAngles = Vector3.zero;
            _informationControl.Show(info);
            _showHideOrbitsButton.gameObject.SetActive(false);
            //_zoomIn.gameObject.SetActive(false);
            //_zoomOut.gameObject.SetActive(false);
            ShowAndRotate(_galileoSatellites, false);
            ShowAndRotate(_gpsSatellites, false);
            ShowAndRotate(_glonassSatellites, false);
            ShowAndRotate(_beidouSatellites, false);
        }

        /// <summary>
        /// Rotate againe from stop position
        /// </summary>
        private void ContinueMoving()
        {
            _buttonCanvasGroup.SetActive(false);
            //_earth.StartRotation();
            _informationControl.Hide();
            _informationControl.Disable();
            MoveBack(_galileoSatellites);
            MoveBack(_gpsSatellites);
            MoveBack(_glonassSatellites);
            MoveBack(_beidouSatellites);
        }

        /// <summary>
        /// Moves the back satellites
        /// </summary>
        /// <param name="satellites">Satellites.</param>
        private void MoveBack(List<SatelliteControl> satellites)
        {
            for (int i = 0; i < satellites.Count; i++)
            {
                satellites[i].MoveBack(RotateAllElements);
            }

        }

        /// <summary>
        /// Shows the after place earth in AR world
        /// </summary>
        private IEnumerator ShowAfterPlace()
        {
            _buttonCanvasGroup.SetActive(false);
            _rotateEarthControl.Enable();
            _rotateEarthControl.StartRotation();

            //ShowAndRotate(_galileoSatellites, true);
            yield return new WaitForSeconds(_pauseTimeBeforeShowAllSatellites);
            _weiterStepButton.gameObject.SetActive(true);
            //_zuruckStepButton.gameObject.SetActive(true);
            _zoomIn.gameObject.SetActive(true);
            _zoomOut.gameObject.SetActive(true);
        }

        /// <summary>
        /// Shows the buttons.
        /// </summary>
        /// <param name="isShow">If set to <c>true</c> is show.</param>
        private void ShowButtons(bool isShow)
        {
            _showHideOrbitsButton.gameObject.SetActive(isShow);
            //_zoomIn.gameObject.SetActive(isShow);
            //_zoomOut.gameObject.SetActive(isShow);

            _canZoom = isShow;
        }

        /// <summary>
        /// Show next object from scenarios
        /// </summary>
        private void NextState()
        {
            if (_scenariosIndex < _scenariosObjects.Count)
            {
                if (!_zuruckStepButton.gameObject.activeSelf)
                    _zuruckStepButton.gameObject.SetActive(true);

                if (_scenariosIndex == -1)
                {
                    _scenariosIndex++;
                }

                ShowAndRotate(_scenariosObjects[_scenariosIndex], true);
                ShowOrbits(_scenariosObjects[_scenariosIndex], _isShowOrbits);
                _scenariosIndex++;
                if (_scenariosIndex >= _scenariosObjects.Count)
                {
                    _weiterStepButton.gameObject.SetActive(false);
                    _zuruckStepButton.gameObject.SetActive(true);
                }
                ShowButtons(true);
            }
        }

        /// <summary>
        /// Hide last the state.
        /// </summary>
        private void PrevState()
        {
            if (_scenariosIndex >= 0)
            {
                _scenariosIndex--;
                if (_scenariosIndex == -1)
                {
                    return;
                }
                if (!_weiterStepButton.gameObject.activeSelf)
                    _weiterStepButton.gameObject.SetActive(true);

                if (_scenariosIndex == 0)
                {
                    ShowButtons(false);
                    _zuruckStepButton.gameObject.SetActive(false);
                }

                DennyClick(_scenariosObjects[_scenariosIndex], false);
                ShowAndRotate(_scenariosObjects[_scenariosIndex], false);
                ShowOrbits(_scenariosObjects[_scenariosIndex], false);
            }

        }
        #endregion

        #region ZoomIn/Out

        /// <summary>
        /// Start zooming in.
        /// </summary>
        private void ZoomInBegin()
        {
            if (!_canZoom) return;
            _isZooming = true;
            StartCoroutine(Zooming(true));
        }

        /// <summary>
        /// Stop zooming in.
        /// </summary>
        private void ZoomInEnd()
        {
            _isZooming = false;
        }

        /// <summary>
        /// Start zooming out.
        /// </summary>
        private void ZoomOutBegin()
        {
            if (!_canZoom) return;
            _isZooming = true;
            StartCoroutine(Zooming(false));
        }

        /// <summary>
        /// Stop zooming out.
        /// </summary>
        private void ZoomOutEnd()
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
            Vector3 startScale = _rotateEarthControl.transform.localScale;
            Vector3 endScale = isZoomIn ? _zoomMax : _zoomMin;
            float timer = 0;
            while (_isZooming)
            {
                _rotateEarthControl.transform.localScale = Vector3.MoveTowards(startScale, endScale, timer);
                timer += Time.deltaTime * _zoomingSpeed;
                yield return null;
            }
        }
        #endregion

        #region Show/Hide Orbits

        /// <summary>
        /// Shows the hide orbits on button click.
        /// </summary>
        private void ShowHideOrbits()
        {
            _isShowOrbits = !_isShowOrbits;

            ShowOrbits(_galileoSatellites, _isShowOrbits);
            ShowOrbits(_gpsSatellites, _isShowOrbits);
            ShowOrbits(_glonassSatellites, _isShowOrbits);
            ShowOrbits(_beidouSatellites, _isShowOrbits);
            _showHideOrbitsText.text = _isShowOrbits ? _hideText : _showText;
        }

        /// <summary>
        /// Shows the orbites if it need. 
        /// </summary>
        /// <param name="satellites">Satellites.</param>
        /// <param name="isShow">If set to <c>true</c> is show.</param>
        private void ShowOrbits(List<SatelliteControl> satellites, bool isShow)
        {
            for (int i = 0; i < satellites.Count; i++)
            {
                satellites[i].ShowHideOrbits(isShow);
            }
        }

        #endregion
    }
}
