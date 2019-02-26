using System;

using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.ARScene
{
	public class PlaceDetectedMenuControl : MonoBehaviourWrapper
    {
		[SerializeField] private Button _setPointButton;
        [SerializeField] private CanvasGroup _findUi;
		private Vector3 _placePosition;
		private Quaternion _placeRotation;

		public void Init()
		{
            _findUi.SetActive(true);
            _setPointButton.gameObject.SetActive(false);
            //Disable();
        }

		public void OnFoundPlace(Vector3 position, Quaternion rotation)
		{
			_placePosition = position;
			_placeRotation = rotation;
            _findUi.SetActive(false);
            _setPointButton.gameObject.SetActive(true);
            //Enable();
        }

		public void OnLoseFocuse()
		{
			_placePosition = Vector3.zero;
			_placeRotation = Quaternion.identity;
            _findUi.SetActive(true);
            _setPointButton.gameObject.SetActive(false);
            //Disable();
		}

		public void AddOnClickSetPointListener(Action<Vector3,Quaternion> action)
		{
			_setPointButton.onClick.AddListener(() => action(_placePosition, _placeRotation));
		}

        public override void Disable()
        {
            Debug.Log("PlaceDetectedMenuControl Disable");
            base.Disable();
            Debug.Log("PlaceDetectedMenuControl IsEnabled " + IsEnabled);
        }
    }
}
