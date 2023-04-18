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
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _missedFrameColor;
    [SerializeField] private TMP_Text _currentFrameText;
    [SerializeField] private TMP_Text _lastFrameText;

    [Header("Properties")]
    [SerializeField] private float _frameHeight = 4;
    [SerializeField] private float _frameWidth = 20;
    [SerializeField] private int _framesAtStart = 60;

    private Image[] _framesArray;
    private int _framesIndex = 0;
    private bool _isMissedFramesEnabled = false;

    private void Start()
    {
        StartCoroutine(WaitFrame());
    }

    public void UpdateFrameIndexes(int frameCount)
    {
        _lastFrameImage.transform.position = _framesArray[_framesIndex].transform.position;
        _currentFrameImage.transform.position = _framesArray[_framesIndex + 1].transform.position;

        _lastFrameImage.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(_framesArray[_framesIndex].GetComponent<RectTransform>().rect.width, _framesArray[_framesIndex].GetComponent<RectTransform>().rect.height);
        _currentFrameImage.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(_framesArray[_framesIndex + 1].GetComponent<RectTransform>().rect.width, _framesArray[_framesIndex + 1].GetComponent<RectTransform>().rect.height);

        _currentFrameText.text = $"Last frame: {_framesIndex + 1}";
        _lastFrameText.text = $"Current frame: {_framesIndex}";

        _framesIndex++;

        if (_framesIndex >= frameCount - 1)
        {
            if (_isMissedFramesEnabled)
            {
                foreach (var frame in _framesArray)
                {
                    frame.GetComponent<Image>().color = _defaultColor;
                }
            }

            _framesIndex = 0;
        }
    }

    public void UpdateMissingFrame(bool isEnabled)
    {
        _isMissedFramesEnabled = isEnabled;

        if (_isMissedFramesEnabled)
        {
            _framesArray[_framesIndex].GetComponent<Image>().color = _missedFrameColor;
        }
    }

    public void ResetIndex()
    {
        _framesIndex = 0;
    }

    public void Show()
    {
        _lastFrameImage.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _lastFrameImage.gameObject.SetActive(false);
    }

    public void UpdateFrameCount(int frameCount)
    {
        for (int i = 0; i < _framesArray.Length; i++)
        {
            _framesArray[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < frameCount; i++)
        {
            _framesArray[i].gameObject.SetActive(true);
        }
    }

    public void CheckOnMissedFrame()
    {

    }

    private IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();

        CreateFrames();
    }

    private void CreateFrames()
    {
        _framesArray = new Image[_framesAtStart];

        for (int i = 0; i < _framesAtStart; i++)
        {
            Image frame = Instantiate(_frameImage, transform);

            _framesArray[i] = frame;
        }
    }
}