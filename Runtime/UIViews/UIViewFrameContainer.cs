using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIViewFrameContainer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Image _frameImage;
    [SerializeField] private RectTransform _framesContainer;
    [SerializeField] private TMP_InputField _targetFPSInputField;
    [SerializeField] private Image _lastFrameImage;
    [SerializeField] private Image _currentFrameImage;
    [SerializeField] private Image _deltaTimeImage;

    [Header("Properties")]
    [SerializeField] private float _frameHeight = 4;
    [SerializeField] private float _frameWidth = 20;
    [SerializeField] private int _framesAtStart = 60;

    private RectTransform _rectTransform = null;
    private Image[] _framesArray;
    private int _framesIndex = 0;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine(WaitFrame());
    }

    public void UpdateFrameIndexes()
    {
        _framesIndex++;

        if (_framesIndex >= _framesAtStart - 1)
        {
            _framesIndex = 0;
        }

        _lastFrameImage.transform.position = _framesArray[_framesIndex].transform.position;
        _currentFrameImage.transform.position = _framesArray[_framesIndex + 1].transform.position;

        UpdateDeltaTime(Mathf.Abs(_currentFrameImage.transform.position.y - _lastFrameImage.transform.position.y));
    }

    public void Show()
    {
        _lastFrameImage.gameObject.SetActive(true);
        _currentFrameImage.gameObject.SetActive(true);
        _deltaTimeImage.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _lastFrameImage.gameObject.SetActive(false);
        _currentFrameImage.gameObject.SetActive(false);
        _deltaTimeImage.gameObject.SetActive(false);
    }

    private IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();

        CreateFrames();
    }

    private void CreateFrames()
    {
        _framesArray = new Image[_framesAtStart];

        Vector3 top = new Vector3(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y - _frameHeight + _rectTransform.rect.height) / 2;
        Vector3 bottom = new Vector3(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y + _frameHeight - _rectTransform.rect.height) / 2;
        Vector3 distance = top - bottom;
        Vector3 height = (distance / _framesAtStart);
        Vector3 position = Vector3.zero;

        for (int i = 0; i < _framesAtStart; i++)
        {
            Image frame = Instantiate(_frameImage, transform);
            frame.GetComponent<RectTransform>().sizeDelta = new Vector2(_frameWidth, _frameHeight);
            frame.transform.localPosition = top - position;

            position += height;

            _framesArray[i] = frame;
        }

        Image lastFrame = Instantiate(_frameImage, transform);
        lastFrame.GetComponent<RectTransform>().sizeDelta = new Vector2(_frameWidth, _frameHeight);
        lastFrame.transform.localPosition = bottom;

        _framesArray[_framesArray.Length - 1] = lastFrame;
    }

    private void UpdateDeltaTime(float frameOffset)
    {
        Vector2 size = new Vector2(10, frameOffset);

        _deltaTimeImage.GetComponent<RectTransform>().sizeDelta = size;
        _deltaTimeImage.transform.position = new Vector2(_framesArray[0].transform.position.x, (_currentFrameImage.transform.position.y + _lastFrameImage.transform.position.y) / 2);
    }
}