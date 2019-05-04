using System.Collections;
using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers.ARScene
{
    public class RotateModelControl : MonoBehaviourWrapper, IPointerClickHandler
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _view;
        [SerializeField] private Transform _object;
        [SerializeField] private Vector3 _rotationVector;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private int _repeat = 6;
        [SerializeField] private Animation _animation;

        private Vector2 _mouse;

        private bool _canRotate;
        private bool _isCourotineStart;

        private float _rotateCoeficient;
        private const float CircleDegrees = 360;
        private float _delta;
        private bool _issStartedAnimation;

        IEnumerator Rotate()
        {
            if (_isCourotineStart) yield break;
            _isCourotineStart = true;
            while (_canRotate)
            {
                _view.rotation = Quaternion.Lerp(_view.rotation, _target.rotation,
                    Time.deltaTime * _rotateCoeficient * _delta);
                if (Quaternion.Angle(_view.rotation, _target.rotation) < 0.1f)
                {
                    _view.rotation = _target.rotation;
                    _canRotate = false;
                }

                yield return null;
            }
            _isCourotineStart = false;
        }



        private IEnumerator RotateObject()
        {
            if (_issStartedAnimation) yield break;

            _animation.Play();
            _issStartedAnimation = true;

            bool right = false;
            int counter = 0;
            Vector3 beginState = _object.transform.eulerAngles;
            float timer = 0;

            while (counter < _repeat)
            {
                 timer = 0;

                Vector3 start = _object.transform.eulerAngles;
                Vector3 end = right ? _rotationVector : -_rotationVector;
                var rotation = Quaternion.Euler(end);
                while (timer < 1f)
                {
                    _object.rotation = Quaternion.Slerp(_object.rotation, rotation, timer);
                    timer += Time.deltaTime * _rotationSpeed;
                    yield return null;
                }

                _object.rotation = rotation;

                _object.eulerAngles = end;
                right = !right;
                counter++;
                yield return null;
            }

            Vector3 current = _object.transform.eulerAngles;

            timer = 0;

            while (timer < 1f)
            {
                _object.rotation = Quaternion.Slerp(_object.rotation, Quaternion.Euler(beginState), timer);
                timer += Time.deltaTime * _rotationSpeed;
                yield return null;
            }
            _object.transform.eulerAngles = beginState;
            _issStartedAnimation = false;
            Enable();
        }

        public  void Init()
        {
            _target.rotation = _view.rotation;
            _rotateCoeficient = CircleDegrees / Screen.width;
            Disable();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StartCoroutine(RotateObject());
            Disable();
        }
    }
}