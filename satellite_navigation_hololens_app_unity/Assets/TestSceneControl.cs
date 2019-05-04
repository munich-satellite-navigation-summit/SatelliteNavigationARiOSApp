using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.ARScene;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneControl : MonoBehaviour
{
    [SerializeField] private RotateEarthControl _rotateEarthControl;
    [SerializeField] private List<SatelliteControl> _galileoSatellites;
    [SerializeField] private List<SatelliteControl> _gpsSatellites;
    [SerializeField] private List<SatelliteControl> _glonassSatellites;
    [SerializeField] private List<SatelliteControl> _beidouSatellites;

    [SerializeField] private Transform _pointBeforeCameraTransform;
    [SerializeField] private Button _backButton;
    [SerializeField] private InformationControl _informationControl;

    [SerializeField] private float _pauseTimeBeforeShowAllSatellites = 2f;

    private CanvasGroup _buttonCanvasGroup;
    [SerializeField] private ModelsControl _modelsControl;

    //[SerializeField] private SatelliteControl _sateliteGPS;

    // Start is called before the first frame update
    void Start()
    {
        _buttonCanvasGroup = _backButton.GetComponent<CanvasGroup>();

        //InitSatellites(_galileoSatellites);
        //InitSatellites(_gpsSatellites);
        //InitSatellites(_glonassSatellites);
        //InitSatellites(_beidouSatellites);
        //StartCoroutine(ShowAfterPlace());
        _modelsControl.Init();
        _modelsControl.TestShow();
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
    /// Initialize the satellites.
    /// </summary>
    /// <param name="satellites">Satellites.</param>
    private void InitSatellites(List<SatelliteControl> satellites)
    {
        for (int i = 0; i < satellites.Count; i++)
        {
            satellites[i].Init(StopRotations, _pointBeforeCameraTransform);
        }
    }

    /// <summary>
    /// Start rotate all elements.
    /// </summary>
    private void RotateAllElements()
    {
        _buttonCanvasGroup.SetActive(false);
        _rotateEarthControl.Enable();
        _rotateEarthControl.StartRotation();

        ShowAndRotate(_galileoSatellites, true);
        ShowAndRotate(_gpsSatellites, true);
        ShowAndRotate(_glonassSatellites, true);
        ShowAndRotate(_beidouSatellites, true);
    }

    /// <summary>
    /// Shows the satellites and rotate.
    /// </summary>
    /// <param name="isShow">If set to <c>true</c> is show and rotate satellites else it hide and stop rotation</param>
    private void ShowAndRotate(List<SatelliteControl> satellites, bool isShow)
    {
        for (int i = 0; i < satellites.Count; i++)
        {
            if (isShow)
            {
                satellites[i].ShowAndStartRotate();
            }
            else
            {
                satellites[i].HideAndStopRotate();
            }

        }
    }

    /// <summary>
    /// Stops the rotations.
    /// </summary>
    /// <param name="info">Info about satellite.</param>
    private void StopRotations(SatelliteInformationSO info)
    {
        _rotateEarthControl.EndRotation();
        _rotateEarthControl.Disable();
        _buttonCanvasGroup.SetActive(true);
        _informationControl.Enable();
        _informationControl.transform.parent = info.satellite.transform;
        _informationControl.transform.localPosition = Vector3.zero;
        _informationControl.transform.eulerAngles = Vector3.zero;
        _informationControl.Show(info);

        ShowAndRotate(_galileoSatellites, false);
        ShowAndRotate(_gpsSatellites, false);
        ShowAndRotate(_glonassSatellites, false);
        ShowAndRotate(_beidouSatellites, false);
    }

    /// <summary>
    /// Rotate againe from stop position
    /// </summary>
    private void ContinueMoving()
    {
        //_earth.StartRotation();
        _informationControl.Hide();
        _informationControl.Disable();
        MoveBack(_galileoSatellites);
        MoveBack(_gpsSatellites);
        MoveBack(_glonassSatellites);
        MoveBack(_beidouSatellites);
    }

    /// <summary>
    /// Moves the back satellites
    /// </summary>
    /// <param name="satellites">Satellites.</param>
    private void MoveBack(List<SatelliteControl> satellites)
    {
        for (int i = 0; i < satellites.Count; i++)
        {
            satellites[i].MoveBack(RotateAllElements);
        }
    }

    /// <summary>
    /// Shows the after place earth in AR world
    /// </summary>
    private IEnumerator ShowAfterPlace()
    {
        ShowAndRotate(_galileoSatellites, true);
        yield return new WaitForSeconds(_pauseTimeBeforeShowAllSatellites);
        ShowAndRotate(_gpsSatellites, true);
        ShowAndRotate(_glonassSatellites, true);
        ShowAndRotate(_beidouSatellites, true);
    }


}
