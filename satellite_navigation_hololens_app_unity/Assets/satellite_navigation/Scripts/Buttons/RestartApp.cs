using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Restart app. This screep has to be situate on the restart button. It will loand SpleshScreen scene and simulate restart app.
/// </summary>
[RequireComponent(typeof(Button))]
public class RestartApp : MonoBehaviourWrapper, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadSceneAsync(Scenes.Main.ToString());
    }
}
