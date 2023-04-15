using System;
using TMPro;
using TOMICZ.DeltaTimeSimulator.UIViews;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounterController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int _frameCount = 60;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _carOffsetY = 60f;

    [Header("Dependencies")]
    [SerializeField] private Image _frame;
    [SerializeField] private Image _lastFrame;
    [SerializeField] private Image _currentFrame;
    [SerializeField] private Image _deltaTime;
    [SerializeField] private Image _car;
    [SerializeField] private Toggle _isDeltaTimeEnabledToggle;
    [SerializeField] private TMP_InputField _targetFPSInputField;
    [SerializeField] private TMP_InputField _speedInputField;

    [Header("Transforms")]
    [SerializeField] private Transform _frameContainer;

    [Header("Stats UI")]
    [SerializeField] private TMP_Text _fpsStatsText;
    [SerializeField] private TMP_Text _deltaTimeText;
    [SerializeField] private TMP_Text _currentFrameText;
    [SerializeField] private TMP_Text _lastFrameText;
    [SerializeField] private TMP_Text _moveSpeedText;
    [SerializeField] private TMP_Text _distanceCrossedText;
    [SerializeField] private TMP_Text _timeElapsedText;
    [SerializeField] private TMP_Text _currentDeltaTimeText;

    [Header("UIView Dependencies")]
    [SerializeField] private UIViewTimeContainer _uiViewTimeContainer;
    [SerializeField] private UIViewFrameContainer _uiViewFrameContainer;

    private Transform[] _framesArray;
    private int _frameIndex = 0;
    private bool _isEventStarted = false;
    private bool _isDeltaTimeEnabled = false;
    private float _timeElapsed = 0;
    private Vector3 _carStartPosition = Vector3.zero;

    private void Awake()
    {
        _uiViewTimeContainer.OnActionCompleted += OnTimeCycleCompletedEventHandler;
        //EnableDeltaTimeObjects(false);
        Application.targetFrameRate = _frameCount;
    }

    private void OnTimeCycleCompletedEventHandler()
    {

    }

    private void OnEnable()
    {
        _carStartPosition = _car.transform.position;
    }

    public void StartEvent()
    {
        //EnableDeltaTimeObjects(true);
        _uiViewFrameContainer.Show();
        _isEventStarted = true;
        ToggleDeltaTime();

        Debug.Log(Application.targetFrameRate);

    }

    public void StopEvent()
    {
        _frameIndex = 0;
        _isEventStarted = false;
        //ResetPositions();
        _uiViewFrameContainer.Hide();
    }

    public void SetTargetFramerate()
    {
        //_frameCount = int.Parse(_targetFPSInputField.text);
        //Application.targetFrameRate = _frameCount;

        PoolFrames();
    }

    public void SetSpeed()
    {
        _moveSpeed = float.Parse(_speedInputField.text);
    }

    public void EnableDeltaTime() => _isDeltaTimeEnabled = true;

    public void DisableDeltaTime() => _isDeltaTimeEnabled = false;

    private void Update()
    {
        if (!_isEventStarted) return;

        //TrackLastAndCurrentFrame();
        //_frameIndex++;
        PushMovingEntityForward();
        _uiViewFrameContainer.UpdateFrameIndexes();
        _uiViewTimeContainer.UpdateTime();

        _fpsStatsText.text = "FPS: " + (1 / Time.deltaTime).ToString("F1");

        _timeElapsed += Time.deltaTime;
        _timeElapsedText.text = $"Time elapsed: {_timeElapsed.ToString("F0")}s";

        if (_isDeltaTimeEnabled)
        {
            _currentDeltaTimeText.text = $"deltaTime: {Time.deltaTime}";
        }
    }

    private void EnableDeltaTimeObjects(bool isEnabled)
    {
        _uiViewFrameContainer.Show();
    }

    private void PushMovingEntityForward()
    {
        if (_isDeltaTimeEnabled)
        {
            _car.transform.position += Vector3.right * (_moveSpeed * Time.deltaTime);

            _moveSpeedText.text = $"Move speed: {_moveSpeed}/s";
        }
        else
        {
            _car.transform.position += Vector3.right * _moveSpeed; ;

            _moveSpeedText.text = $"Move speed: {_moveSpeed}/f";
        }

        _distanceCrossedText.text = $"Distance crossed: {(_carStartPosition.x - _car.transform.position.x).ToString("F0").Substring(1)} units";
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

        _deltaTimeText.text = $"Delta time enabled: {_isDeltaTimeEnabled}";
    }

    private void PoolFrames()
    {
        for (int i = 0; i < _framesArray.Length; i++)
        {
            _framesArray[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _frameCount; i++)
        {
            _framesArray[i].gameObject.SetActive(true);
        }

        ResetPositions();
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

        UpdateDeltaTime(Mathf.Abs(_currentFrame.transform.position.y - _lastFrame.transform.position.y));

        _lastFrameText.text = $"Last frame: {_frameIndex}";
        _currentFrameText.text = $"Current frame: {_frameIndex + 1}";
    }

    private void UpdateDeltaTime(float frameOffset)
    {
        Vector2 size = new Vector2(10, frameOffset);

        _deltaTime.GetComponent<RectTransform>().sizeDelta = size;
        _deltaTime.transform.position = new Vector2(_framesArray[0].transform.position.x, (_currentFrame.transform.position.y + _lastFrame.transform.position.y) / 2);
        _deltaTime.GetComponentInChildren<TMP_Text>().text = Time.deltaTime.ToString("F4");
    }

    private void ResetPositions()
    {
        //_lastFrame.transform.position = _framesArray[0].position;
        //_currentFrame.transform.position = _framesArray[1].position;

        UpdateDeltaTime(_currentFrame.transform.position.x - _lastFrame.transform.position.x);

        _car.transform.position = _carStartPosition;
        _timeElapsed = 0;
    }
}