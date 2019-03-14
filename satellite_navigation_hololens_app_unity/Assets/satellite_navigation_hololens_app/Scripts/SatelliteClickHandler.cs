using System;
using Helpers;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class SatelliteClickHandler : MonoBehaviourWrapper, IClickHandler
{

    [SerializeField] private SatelliteInformationSO _informationSO;
    [SerializeField] private LinePointer _linePointer;

    private Action<SatelliteClickHandler, SatelliteInformationSO> _action;
    private bool _isClicked;

    /// <summary>
    /// Ons the mouse up.
    /// </summary>
    private void OnMouseUp()
    {
        if (_isClicked) return;
        _isClicked = true;
        _linePointer.SetHideRay();
        if (_action != null)
        {
            _action(this, _informationSO);
        }
    }

    /// <summary>
    /// Adds the listener to the click on satellite
    /// </summary>
    /// <param name="action">Action - funclion what have to be when click  on the satellite</param>
    public void AddListener(Action<SatelliteClickHandler, SatelliteInformationSO> action)
    {
        _action = action;
    }

    /// <summary>
    /// Set boolean to false for start click again.
    /// </summary>
    public void CanClick()
    {
        _isClicked = false;
    }

    public override void Enable()
    {
        base.Enable();
    }

    public override void Disable()
    {
        if(!_isClicked)
            base.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ShowRay")
        {
            _linePointer.SetShowRay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ShowRay")
        {
            _linePointer.SetHideRay();
        }
    }


}
