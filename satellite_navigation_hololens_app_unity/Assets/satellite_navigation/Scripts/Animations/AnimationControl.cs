using System;
using System.Collections;
using Enums;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    [Header("Motion")] [SerializeField] private bool _needMotion = true;
    [SerializeField] private MotionVector _motionVector;
    [SerializeField] private float _moveTime = 1;
    [SerializeField] private float _length = 0f;
    [SerializeField] private bool _isHidden;
    [SerializeField] private bool _needHideAtStart = true;

    [Header("Scale")] [SerializeField] private bool _needScale;
    [SerializeField] private float _scaleTime = 1f;
    [Range(0, 1)] [SerializeField] private float _startSizeCoeficient = 0.5f;

    [Header("Canvas Group")]
    [SerializeField]
    private bool _needOnWithCanvas;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _alphaTime = 1f;
    [Range(0, 1)] [SerializeField] private float _startAlpha = 0f;
    [Range(0, 1)] [SerializeField] private float _endAlpha = 1f;

    private Vector2 _showPosition;
    private Vector2 _hidePosition;

    private Vector3 _startSize;
    private Vector3 _endSize;

    private Coroutine _move;
    private Coroutine _scale;
    private Coroutine _canvas;
    private Action _onEndAction;
    private bool _isHide;
    private bool isCalculated;

    private void StartCalculate()
    {
        if (isCalculated) return;
        if (_rectTransform == null)
            _rectTransform = transform as RectTransform;
        if (_needMotion)
            CalculateMotion();

        if (_needScale)
            CalculateScale();

        if (_needOnWithCanvas)
        {
            _canvasGroup.alpha = _startAlpha;
        }

        _isHide = _needHideAtStart;
        isCalculated = !isCalculated;
    }

    private void OnEnable()
    {
        _isHide = true;

        HideEmediately();
        StartCalculate();
    }
    /// <summary>
    /// Show animation with delay
    /// </summary>
    /// <param name="delay"> by default is = 0 seconds</param>
    public void Show(float delay = 0)
    {
        Debug.Log("Show");
        //if (!_rectTransform.gameObject.activeInHierarchy) return;
        if (!_isHide) return;
        if (_needMotion)
            _move = StartCoroutine(Move(_hidePosition, _showPosition, delay));
        if (_needOnWithCanvas)
            _canvas = StartCoroutine(OnOffCanvasGroup(_startAlpha, _endAlpha, delay));
        if (_needScale)
            _scale = StartCoroutine(ScaleObject(_endSize, _startSize, delay));
        StartCoroutine(WaitEndOfAnimation());
        _isHide = false;
    }

    /// <summary>
    /// Hide animation with delay
    /// </summary>
    /// <param name="delay">by default is = 0 seconds</param>
    public void Hide(float delay = 0)
    {
        //if (!_rectTransform.gameObject.activeInHierarchy)
        //{
        //    //HideEmediately();
        //    return;
        //}
        if (_isHide) return;
        if (_needMotion)
            _move = StartCoroutine(Move(_showPosition, _hidePosition, delay));

        if (_needOnWithCanvas)
            _canvas = StartCoroutine(OnOffCanvasGroup(_endAlpha, _startAlpha, delay));

        if (_needScale)
            _scale = StartCoroutine(ScaleObject(_startSize, _endSize, delay));
        StartCoroutine(WaitEndOfAnimation());
        _isHide = true;
    }

    public void HideEmediately()
    {
        StopAllCoroutines();
        //if (_isHide) return;
        if (_needMotion)
            _rectTransform.anchoredPosition = _hidePosition;

        if (_needOnWithCanvas)
            _canvasGroup.alpha = _startAlpha;

        if (_needScale)
            _rectTransform.transform.localScale = _endSize;
        _isHide = true;
    }

    public void OnEndAnimation(Action action)
    {
        _onEndAction = action;
    }

    private void CalculateMotion()
    {
        Vector2 tmp = Vector2.zero;
        Vector2 changePosition = Vector2.zero;
        Vector2 length = Vector2.zero;
        if (_length == 0)
        {
            changePosition.x = _rectTransform.rect.width;
            changePosition.y = _rectTransform.rect.height;
            length = changePosition;
        }
        else
        {
            float x = (float)(0.707 * _length); //0.707 = sin (45)
            float y = (float)(Math.Sqrt(_length * _length - x * x));
            changePosition.x = x;
            changePosition.y = y;
            length.x = _length;
            length.y = _length;
        }

        switch (_motionVector)
        {
            case MotionVector.Up:
                tmp = new Vector2(0, length.y);
                break;
            case MotionVector.Down:
                tmp = new Vector2(0, -length.y);
                break;
            case MotionVector.Left:
                tmp = new Vector2(-length.x, 0);
                break;
            case MotionVector.Right:
                tmp = new Vector2(length.x, 0);
                break;
            case MotionVector.UpLeft:
                tmp = new Vector2(-changePosition.x, changePosition.y);
                break;
            case MotionVector.UpRight:
                tmp = new Vector2(changePosition.x, changePosition.y);
                break;
            case MotionVector.DownLeft:
                tmp = new Vector2(-changePosition.x, -changePosition.y);
                break;
            case MotionVector.DownRight:
                tmp = new Vector2(changePosition.x, -changePosition.y);
                break;
        }

        if (_isHidden)
        {
            _hidePosition = _rectTransform.anchoredPosition;
            _showPosition = _rectTransform.anchoredPosition + tmp;
        }
        else
        {
            _showPosition = _rectTransform.anchoredPosition;
            _hidePosition = _rectTransform.anchoredPosition - tmp;
        }

        if (_needHideAtStart)
            _rectTransform.anchoredPosition = _hidePosition;
    }

    private void CalculateScale()
    {
        _startSize = _rectTransform.transform.localScale;
        _endSize = _startSize * _startSizeCoeficient;
        _rectTransform.transform.localScale = _endSize;
    }


    private IEnumerator Move(Vector2 start, Vector2 end, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Move");
        float timer = 0;
        while (timer < 1f)
        {
            _rectTransform.anchoredPosition = Vector2.Lerp(start, end, timer);
            timer += Time.deltaTime / (_moveTime != 0 ? _moveTime : 0.2f);
            yield return null;
        }

        _rectTransform.anchoredPosition = end;
    }


    private IEnumerator OnOffCanvasGroup(float start, float end, float delay)
    {
        yield return new WaitForSeconds(delay);
        float timer = 0;
        while (timer < 1f)
        {
            _canvasGroup.alpha = Mathf.Lerp(start, end, timer);
            timer += Time.deltaTime / _alphaTime;
            yield return null;
        }

        _canvasGroup.alpha = end;
    }


    private IEnumerator ScaleObject(Vector3 start, Vector3 end, float delay)
    {
        yield return new WaitForSeconds(delay);
        float timer = 0;
        while (timer < 1f)
        {
            _rectTransform.transform.localScale = Vector3.Lerp(start, end, timer);
            timer += Time.deltaTime / _scaleTime;
            yield return null;
        }

        _rectTransform.transform.localScale = end;
    }

    private IEnumerator WaitEndOfAnimation()
    {
        yield return _move;
        yield return _scale;
        yield return _canvas;
        if (_onEndAction != null)
        {
            _onEndAction();
            _onEndAction = null;
        }
    }

    private void SetTimeScale()
    {
        //If it was in background it equals 0,
        Time.timeScale = 1f;
    }


    void OnApplicationFocus(bool hasFocus)
    {
        SetTimeScale();
    }
}
