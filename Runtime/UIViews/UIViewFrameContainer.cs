using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIViewFrameContainer : MonoBehaviour
{
    [SerializeField] private Image _frameImage;
    [SerializeField] private RectTransform _framesContainer;
    [SerializeField] private TMP_InputField _targetFPSInputField;

    private RectTransform _rectTransform = null;
    private int _frameCount = 0;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _frameCount = int.Parse(_targetFPSInputField.text);
        CreateFrames();
    }

    public void SetTargetFramerate()
    {
        _frameCount = int.Parse(_targetFPSInputField.text);
        Application.targetFrameRate = _frameCount;
    }

    private void CreateFrames()
    {
        //float height = (_rectTransform.rect.height / _frameCount - 2);
        //float top = (_rectTransform.anchoredPosition.y + _rectTransform.rect.height / 2) + (height / 2);

        for (int i = 0; i < _frameCount; i++)
        {
            Image frame = Instantiate(_frameImage, transform);
            //frame.GetComponent<RectTransform>().sizeDelta = new Vector2(40, height);
            //frame.transform.localPosition = new Vector2(0, top += - height - 2);
        }

        //_framesContainer.sizeDelta = new Vector2(40, _framesContainer.sizeDelta.y);
    }
}