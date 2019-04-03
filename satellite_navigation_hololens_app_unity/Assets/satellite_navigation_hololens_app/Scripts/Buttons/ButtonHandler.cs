using System;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buttons
{
    public class ButtonHandler : MonoBehaviourWrapper, IPointerDownHandler, IPointerUpHandler
    {
        private List<Action> _onPointerDown = new List<Action>();
        private List<Action> _onPointerUp = new List<Action>();

        /// <summary>
        /// Adds the listener on pointer down.
        /// </summary>
        /// <param name="action">Action what have to work when press down button</param>
        public void AddListenerOnPointerDown(Action action)
        {
            Debug.Log("AddListenerOnPointerDown " + action + "  "  + _onPointerDown.Count);
            _onPointerDown.Add(action);
        }

        /// <summary>
        /// Adds the listener on pointer up.
        /// </summary>
        /// <param name="action">Action what have to work when press up button</param>
        public void AddListenerOnPointerUp(Action action)
        {
            Debug.Log("AddListenerOnPointerUp " + action + "  " + _onPointerUp.Count);
            _onPointerUp.Add(action);
        }

        /// <summary>
        /// Removes all listeners from pointer down/up actions.
        /// </summary>
        public void RemoveAllListeners()
        {
            _onPointerUp.Clear();
            _onPointerDown.Clear();
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            for (int i = 0; i < _onPointerDown.Count; i++)
                if (_onPointerDown[i] != null)
                    _onPointerDown[i].Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            for (int i = 0; i < _onPointerUp.Count; i++)
                if (_onPointerUp[i] != null)
                    _onPointerUp[i].Invoke();
        }
    }
}
