using System.Collections;
using System.Collections.Generic;
using Controllers.ARScene;
using Helpers;
using UnityEngine;

/// <summary>
/// Scene control script init at the start app all scripts what have to start first time.
/// It need add to the empty object
/// </summary>
public class SceneControl : MonoBehaviourWrapper
{

    [SerializeField] private PlacementState _placementState;
    [SerializeField] private ModelsControl _modelControl;

    /// <summary>
    /// This function initialize scripts what nned for start AR Kit tracking
    /// </summary>
    private IEnumerator Start()
    {
        _placementState.Init();

        _modelControl.Init();
        yield return null;
        yield return _placementState.EnterState();
    }
}
