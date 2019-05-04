using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Helpers
{
    public abstract class MonoBehaviourWrapper : MonoBehaviour
    {
        private Transform _transform;

        public new Transform transform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = GetComponent<Transform>();
                }
                return _transform;
            }
        }

        public virtual bool IsEnabled
        {
            get { return gameObject.activeSelf; }
        }

        /// <summary>
        /// Enable this object.
        /// </summary>
        public virtual void Enable()
        {
            if (!IsEnabled)
            {
                gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Disable this object.
        /// </summary>
        public virtual void Disable()
        {
            if (IsEnabled)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
