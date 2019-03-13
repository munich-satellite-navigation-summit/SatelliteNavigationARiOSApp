using System;

using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.ARScene
{
	public class PlaceDetectedMenuControl : MonoBehaviourWrapper
    {
		[SerializeField] private Button _setPointButton;
		private Vector3 _placePosition;
		private Quaternion _placeRotation;

		public void Init()
		{
            _setPointButton.gameObject.SetActive(false);
        }

		public void OnFoundPlace(Vector3 position, Quaternion rotation)
		{
			_placePosition = position;
			_placeRotation = rotation;
            _setPointButton.gameObject.SetActive(true);
        }

		public void OnLoseFocuse()
		{
			_placePosition = Vector3.zero;
			_placeRotation = Quaternion.identity;
            _setPointButton.gameObject.SetActive(false);
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
