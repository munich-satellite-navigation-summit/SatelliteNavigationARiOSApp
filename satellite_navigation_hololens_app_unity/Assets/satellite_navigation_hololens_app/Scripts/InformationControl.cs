using System.Collections;
using System.Collections.Generic;
using Helpers;
using TMPro;
using UnityEngine;

public class InformationControl : MonoBehaviourWrapper
{
    [SerializeField] private AnimationControl _panelAnimation;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _range;
    [SerializeField] private TMP_Text _speed;
    [SerializeField] private TMP_Text _other;

    public void Show(SatelliteInformationSO info)
    {
        _name.text = info.name;
        _range.text = info.range;
        _speed.text = info.speed;
        _other.text = info.other;
        _panelAnimation.Show();
    }

    public void Hide()
    {
        _panelAnimation.Hide();
        _name.text = string.Empty;
        _range.text = string.Empty;
        _speed.text = string.Empty;
        _other.text = string.Empty;
    }
}
