
using System;
using UnityEngine;

namespace Interfaces
{
    public interface IClickHandler
    {
        void AddListener(Action<SatelliteClickHandler, SatelliteInformationSO> action);
    }
}
