using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class SatelliteClickHandler : MonoBehaviourWrapper, IClickHandler
{

    private Action _action;

    private void OnMouseUp()
    {
        Debug.Log("Click  " + name);
        if(_action != null)
        {
            _action();
        }
    }

    /// <summary>
    /// Adds the listener to the click on satellite
    /// </summary>
    /// <param name="action">Action - funclion what have to be when click  on the satellite</param>
    public void AddListener(Action action)
    {
        _action = action;
    }
}
