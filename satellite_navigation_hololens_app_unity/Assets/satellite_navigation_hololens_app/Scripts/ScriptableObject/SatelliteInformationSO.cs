
using UnityEngine;

/// <summary>
/// Satellite information scriptable object for information about satellite
/// </summary>
[CreateAssetMenu(fileName = "Assets/satellite_navigation_hololens_app/Recources/NewSatelliteInformation", menuName = "Satellites/Information", order = 1)]
public class SatelliteInformationSO : ScriptableObject
{
    public string objectName = "New Satellite name";
    public string range;
    public string speed;
    [Multiline]public string other;
}
