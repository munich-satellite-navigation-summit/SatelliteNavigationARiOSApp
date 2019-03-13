using System;
using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buttons
{
    public class ButtonHandler : MonoBehaviourWrapper, IPointerDownHandler, IPointerUpHandler
    {
        private Action _onPointerDown;
        private Action _onPointerUp;

        /// <summary>
        /// Adds the listener on pointer down.
        /// </summary>
        /// <param name="action">Action what have to work when press down button</param>
        public void AddListenerOnPointerDown(Action action)
        {
            _onPointerDown = action;
        }

        /// <summary>
        /// Adds the listener on pointer up.
        /// </summary>
        /// <param name="action">Action what have to work when press up button</param>
        public void AddListenerOnPointerUp(Action action)
        {
            _onPointerUp = action;
        }

        /// <summary>
        /// Removes all listeners from pointer down/up actions.
        /// </summary>
        public void RemoveAllListeners()
        {
            _onPointerUp = null;
            _onPointerDown = null;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (_onPointerDown != null)
                _onPointerDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_onPointerUp != null)
                _onPointerUp();
        }


    }
}
