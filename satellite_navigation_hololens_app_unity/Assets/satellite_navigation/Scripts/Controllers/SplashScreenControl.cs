using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Splash screen control. Show at start an image what is inside CanvasGroup
/// </summary>
public class SplashScreenControl : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _splashScreenTime = 3f;
    [SerializeField] private float _speedAnimation = 0.5f;

    /// <summary>
    /// Set alpha == 0, show splash image, wait _splashScreenTime, hide splash image and load next scene (main)
    /// </summary>
    private IEnumerator Start()
    {
        _canvasGroup.alpha = 0;
        yield return Show(true);
        yield return new WaitForSeconds(_splashScreenTime);
        yield return Show(false);
        SceneManager.LoadSceneAsync(Scenes.Main.ToString());
    }

    /// <summary>
    /// This coroutine show or hide canvas group. It using simple lert and Timde.deltaTime to show/hide canvas.
    /// </summary>
    /// <returns>After end it set canvas group alpha to 1 or 0.</returns>
    /// <param name="isShow">If set to <c>true</c> is show else it hide.</param>
    private IEnumerator Show(bool isShow)
    {
        float start = isShow ? 0 : 1;
        float end = isShow ? 1 : 0;

        float timer = 0;

        _canvasGroup.alpha = start;

        while (timer < 1f)
        {
            _canvasGroup.alpha = Mathf.Lerp(start, end, timer);
            timer += Time.deltaTime * _speedAnimation;
            yield return null;
        }
        _canvasGroup.alpha = end;
    }
}
