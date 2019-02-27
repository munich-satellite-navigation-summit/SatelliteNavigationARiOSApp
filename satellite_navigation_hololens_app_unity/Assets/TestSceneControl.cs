using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneControl : MonoBehaviour
{
    [SerializeField] private RotateEarthControl _earth;
    [SerializeField] private List<SatelliteControl> _satellites;
    [SerializeField] private Transform _pointBeforeCameraTransform;
    [SerializeField] private Button _backButton;
    [SerializeField] private InformationControl _panelInfo;
    //[SerializeField] private SatelliteControl _sateliteGPS;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _satellites.Count; i++)
        {
            _satellites[i].SetData(StopRotations, _pointBeforeCameraTransform);
        }
        RotateAllElements();
    }


    private void OnEnable()
    {
        _backButton.onClick.AddListener(ContinueMoving);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(ContinueMoving);
    }

    /// <summary>
    /// Start rotate all elements.
    /// </summary>
    private void RotateAllElements()
    {
        _earth.Enable();
        _earth.StartRotation();
        _panelInfo.Hide();
        for (int i = 0; i < _satellites.Count; i++)
        {
            _satellites[i].Enable();
            _satellites[i].StartRotation();
        }
    }

    /// <summary>
    /// Stops to rotate the earth.
    /// </summary>
    private void StopRotations(SatelliteInformationSO informationSO)
    {
        _earth.EndRotation();
        _earth.Disable();
        _panelInfo.Show(informationSO);
        for (int i = 0; i < _satellites.Count; i++)
        {
            _satellites[i].EndRotation();
            _satellites[i].Disable();
        }
    }

    /// <summary>
    /// Rotate againe from stop position
    /// </summary>
    private void ContinueMoving()
    {
        //_earth.StartRotation();
        for (int i = 0; i < _satellites.Count; i++)
        {
            _satellites[i].MoveBack(RotateAllElements);
        }
 
    }
}
