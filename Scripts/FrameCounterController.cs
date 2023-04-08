using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounterController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Image _frame;
    [SerializeField] private Image _lastFrame;
    [SerializeField] private Image _currentFrame;
    [SerializeField] private Image _deltaTime;

    [Header("Transforms")]
    [SerializeField] private Transform _frameContainer;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;

    [Header("Properties")]
    [SerializeField] private int _frameCount = 60;

    private Transform[] _framesArray;
    private int _frameIndex = 0;

    private void Awake()
    {
        CreateFrames();
    }

    public void StartEvent()
    {
        //InitiliseFrames();
        //StartCoroutine(StartFrameCounting());
    }

    public void StopEvent()
    {
        //StopAllCoroutines();
        //InitiliseFrames();
    }

    private void Update()
    {
        TrackLastAndCurrentFrame();
    }

    private void CreateFrames()
    {
        Application.targetFrameRate = _frameCount;

        _framesArray = new Transform[_frameCount];

        float fixedOffset = (_startPosition.position.x + _endPosition.position.x) / _frameCount;
        float variableOffset = _startPosition.position.x;

        for (int i = 0; i < _frameCount; i++)
        {
            variableOffset += fixedOffset;
            Vector3 position = new Vector3(variableOffset, _startPosition.transform.position.y, _startPosition.transform.position.z);
            Image frame = Instantiate(_frame, _frameContainer);

            frame.transform.position = position;

            _framesArray[i] = frame.transform;
        }
    }

    private void TrackLastAndCurrentFrame()
    {
        _frameIndex++;

        if(_frameIndex >= _frameCount - 1)
        {
            _frameIndex = 0;
        }

        _lastFrame.transform.position = _framesArray[_frameIndex].position;
        _currentFrame.transform.position = _framesArray[_frameIndex + 1].position;

        UpdateDeltaTime((_currentFrame.transform.position.x - _lastFrame.transform.position.x));
    }

    private void UpdateDeltaTime(float frameOffset)
    {
        Vector2 size = new Vector2(frameOffset, 10);

        _deltaTime.GetComponent<RectTransform>().sizeDelta = size;
        _deltaTime.transform.position = new Vector2((_currentFrame.transform.position.x + _lastFrame.transform.position.x) / 2, _framesArray[0].transform.position.y);
        _deltaTime.GetComponentInChildren<TMP_Text>().text = Time.deltaTime.ToString("F4");
    }
}