using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private List<SatelliteControl> _satellites;
        [SerializeField] private Transform _pointBeforeCameraTransform;
        [SerializeField] private Button _backButton;
        [SerializeField] private InformationControl _informationControl;

        private CanvasGroup _buttonCanvasGroup;
        private Dictionary<ModelView.ModelType, ModelView> _models;

        public void Init()
        {
            _models = _modelList.ToDictionary(obj => obj.Type, obj => obj);
            _buttonCanvasGroup = _backButton.GetComponent<CanvasGroup>();
            Hide();
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
                    for (int i = 0; i < _satellites.Count; i++)
                    {
                        _satellites[i].SetData(StopRotations, _pointBeforeCameraTransform);
                    }
                    RotateAllElements();
                }
                else
                {
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
            _backButton.onClick.AddListener(ContinueMoving);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(ContinueMoving);
        }

        /// <summary>
        /// Start rotate all elements.
        /// </summary>
        private void RotateAllElements()
        {
            _buttonCanvasGroup.SetActive(false);
            _rotateEarthControl.Enable();
            _rotateEarthControl.StartRotation();
      
            for (int i = 0; i < _satellites.Count; i++)
            {
                _satellites[i].Enable();
                _satellites[i].StartRotation();
            }
        }

        /// <summary>
        /// Stops the rotations.
        /// </summary>
        /// <param name="info">Info about satellite.</param>
        private void StopRotations(SatelliteInformationSO info)
        {
            _rotateEarthControl.EndRotation();
            _rotateEarthControl.Disable();
            _buttonCanvasGroup.SetActive(true);
            _informationControl.Enable();
            _informationControl.transform.parent = info.satellite.transform;
            _informationControl.transform.localPosition = Vector3.zero;
            _informationControl.transform.eulerAngles = Vector3.zero;
            _informationControl.Show(info);
            for (int i = 0; i < _satellites.Count; i++)
            {
                _satellites[i].EndRotation();
                _satellites[i].Disable();
            }
        }

        /// <summary>
        /// Rotate againe from stop position
        /// </summary>
        private void ContinueMoving()
        {
            //_earth.StartRotation();
            _informationControl.Hide();
            _informationControl.Disable();
            for (int i = 0; i < _satellites.Count; i++)
            {
                _satellites[i].MoveBack(RotateAllElements);
            }
        }
        #endregion
    }
}
