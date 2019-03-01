using System.Collections;
using System.Collections.Generic;

using Helpers;

using TMPro;

using UnityEngine;

public class InformationControl : MonoBehaviourWrapper
{
    [SerializeField] private List<AnimationControl> _panelAnimations;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _range;
    [SerializeField] private TMP_Text _speed;
    [SerializeField] private TMP_Text _other;


    /// <summary>
    /// Show the specified info.
    /// </summary>
    /// <param name="info">Info.</param>
    public void Show(SatelliteInformationSO info)
    {
        _name.text = info.name;
        _range.text = info.range;
        _speed.text = info.speed;
        _other.text = info.other;
        for (int i = 0; i < _panelAnimations.Count; i++)
        {
            _panelAnimations[i].Show(1f);
        }
    }

    /// <summary>
    /// Hide informations.
    /// </summary>
    public void Hide()
    {
        for (int i = 0; i < _panelAnimations.Count; i++)
        {
            _panelAnimations[i].Hide();
        }
        _name.text = string.Empty;
        _range.text = string.Empty;
        _speed.text = string.Empty;
        _other.text = string.Empty;
    }
}
