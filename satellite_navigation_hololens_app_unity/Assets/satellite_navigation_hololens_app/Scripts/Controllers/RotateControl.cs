using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Rotate control. This is parent clas for rotation classes
    /// </summary>
    public abstract class RotateControl : MonoBehaviourWrapper
    {
        /// <summary>
        /// The capsule transform is object what rotate by 23 degrees and continue in side earth
        /// </summary>
        [SerializeField] private Transform _pointRotateTransform;
        [SerializeField] [Range(0f, 2f)] private float _speedRotation;
        [SerializeField] private Vector3 _rotateAxis;

        private bool _isRotate;

        protected Transform PointRotateTransform { get { return _pointRotateTransform; } }

        /// <summary>
        /// Start Rotate point around its axis
        /// </summary>    
        public void StartRotation()
        {
            Debug.Log("StartRotation " + name);
            _isRotate = true;
            StartCoroutine(Rotate());
        }

        /// <summary>
        /// This function stops rotate 
        /// </summary>
        public void EndRotation()
        {
            Debug.Log("EndRotation " + name);
            _isRotate = false;
        }

        /// <summary>
        /// Coroutine make rotation around axis
        /// </summary>
        IEnumerator Rotate()
        {
            while (_isRotate)
            {
                _pointRotateTransform.Rotate(_rotateAxis.x * _speedRotation, _rotateAxis.y * _speedRotation, _rotateAxis.z * _speedRotation);
                yield return null;
            }
        }

     
    }
}
