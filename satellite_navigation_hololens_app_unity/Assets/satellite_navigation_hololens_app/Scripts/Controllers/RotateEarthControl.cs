using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Rotate earth control mare rotation earth around own axis(y-axis).
/// </summary>
public class RotateEarthControl : MonoBehaviour
{
    /// <summary>
    /// The capsule transform is object what rotate by 23 degrees and continue in side earth
    /// </summary>
    [SerializeField] private Transform _capsuleTransform;
    [SerializeField] [Range(0f, 2f)] private float _speedRotation;

    private bool _isRotateEarth;

    /// <summary>
    /// Start Rotate Earth around its axis
    /// </summary>    
    public void StartRotation()
    {
        _isRotateEarth = true;
        StartCoroutine(RotateEarth());

    }

    /// <summary>
    /// This function stops rotate the Earth
    /// </summary>
    public void EndRotation()
    {
        _isRotateEarth = false;
    }

    /// <summary>
    /// Coroutine make rotation around axis
    /// </summary>
    IEnumerator RotateEarth()
    {
        while (_isRotateEarth)
        {
            _capsuleTransform.Rotate(0, 1 * _speedRotation, 0);
            yield return null;
        }
    }
}
