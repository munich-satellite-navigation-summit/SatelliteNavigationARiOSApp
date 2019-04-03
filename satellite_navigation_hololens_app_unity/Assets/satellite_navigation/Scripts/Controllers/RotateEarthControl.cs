using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace Controllers
{
    /// <summary>
    /// Rotate earth control mare rotation earth around own axis(y-axis).
    /// </summary>
    public class RotateEarthControl : RotateControl
    {
        public override void Enable()
        {
            PointRotateTransform.gameObject.SetActive(true);
        }

        public override void Disable()
        {
            PointRotateTransform.gameObject.SetActive(false);
        }
    }
}