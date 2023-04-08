using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounterController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int _frameCount = 60;
    [SerializeField] private float _moveSpeed;

    [Header("Dependencies")]
    [SerializeField] private Image _frame;
    [SerializeField] private Image _lastFrame;
    [SerializeField] private Image _currentFrame;
    [SerializeField] private Image _deltaTime;
    [SerializeField] private Image _car;
    [SerializeField] private Toggle _isDeltaTimeEnabledToggle;

    [Header("Transforms")]
    [SerializeField] private Transform _frameContainer;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;

    private Transform[] _framesArray;
    private int _frameIndex = 0;
    private bool _isEventStarted = false;
    private bool _isDeltaTimeEnabled = false;

    private void Awake()
    {
        CreateFrames();
        InitiliseMovingEntity();
    }

    public void StartEvent()
    {
        _isEventStarted = true;
        ToggleDeltaTime();
    }

    public void StopEvent()
    {
        _isEventStarted = false;
        ResetPositions();
    }

    public void EnableDeltaTime() => _isDeltaTimeEnabled = true;

    public void DisableDeltaTime() => _isDeltaTimeEnabled = false;

    private void Update()
    {
        if (!_isEventStarted) return;

        TrackLastAndCurrentFrame();
        PushMovingEntityForward();
    }

    private void InitiliseMovingEntity()
    {
        _car.transform.position = new Vector2(_framesArray[0].position.x, _framesArray[0].position.y + 60f);
    }

    private void PushMovingEntityForward()
    {
        if (_isDeltaTimeEnabled)
        {
            _car.transform.position += Vector3.right * (_moveSpeed * Time.deltaTime);
        }
        else
        {
            _car.transform.position += Vector3.right * _moveSpeed;
        }
    }

    private void ToggleDeltaTime()
    {
        if (_isDeltaTimeEnabledToggle.isOn)
        {
            _isDeltaTimeEnabled = true;
        }
        else
        {
            _isDeltaTimeEnabled = false;
        }
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

        UpdateDeltaTime(_currentFrame.transform.position.x - _lastFrame.transform.position.x);
    }

    private void UpdateDeltaTime(float frameOffset)
    {
        Vector2 size = new Vector2(frameOffset, 10);

        _deltaTime.GetComponent<RectTransform>().sizeDelta = size;
        _deltaTime.transform.position = new Vector2((_currentFrame.transform.position.x + _lastFrame.transform.position.x) / 2, _framesArray[0].transform.position.y);
        _deltaTime.GetComponentInChildren<TMP_Text>().text = Time.deltaTime.ToString("F4");
    }

    private void ResetPositions()
    {
        _lastFrame.transform.position = _framesArray[0].position;
        _currentFrame.transform.position = _framesArray[1].position;

        UpdateDeltaTime(_currentFrame.transform.position.x - _lastFrame.transform.position.x);

        _car.transform.position = new Vector2(_framesArray[0].position.x, _framesArray[0].position.y + 60f);
    }
}