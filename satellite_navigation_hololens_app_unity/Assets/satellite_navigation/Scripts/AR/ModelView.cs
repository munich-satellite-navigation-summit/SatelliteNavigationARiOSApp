using System;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class ModelView : MonoBehaviourWrapper
    {
        [Serializable]
        public enum ModelType : byte
        {
            Square,
            Earth
        }

        [SerializeField] private ModelType _modelType;

        public ModelType Type { get { return _modelType; } }
        public Vector3 Position { get { return transform.position; } }
        public Quaternion Rotation { get { return transform.rotation; } }


        public void Move(Vector3 position)
        {
            transform.position = position;
        }

        public void Rotate(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void Show()
        {
            var camera = Camera.current;
            if (camera == null)
            {
                camera = Camera.main;
            }

            Vector3 vector = new Vector3(camera.transform.position.x, this.transform.position.y,
                camera.transform.position.z);
            gameObject.SetActive(true);
            transform.LookAt(vector);
            // 90 is angle of boot rotation
            // 45 is angle what need to rotate
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90 - 45, transform.eulerAngles.z);

        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
