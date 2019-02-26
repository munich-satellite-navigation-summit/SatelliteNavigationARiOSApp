using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Views;

namespace Controllers.ARScene
{
    public class ModelsControl : MonoBehaviour
    {
        [SerializeField] private List<ModelView> _modelList;
        [SerializeField] private RotateEarthControl _rotateEarthControl;
        
        private Dictionary<ModelView.ModelType, ModelView> _models;

        public void Init()
        {
            _models = _modelList.ToDictionary(obj => obj.Type, obj =>obj);
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
                    _rotateEarthControl.StartRotation();
                else
                    _rotateEarthControl.EndRotation();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void Show(ModelView.ModelType type, Vector3 position)
        {
            Show(type,position,Quaternion.identity);
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
                
                Debug.LogError("Try to Hide "+type +"\n"+ e);
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
        
    }
}
