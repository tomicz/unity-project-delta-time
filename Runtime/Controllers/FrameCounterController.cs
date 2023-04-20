using System;
using TMPro;
using TOMICZ.DeltaTimeSimulator.UIViews;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounterController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int _frameCount = 60;

    [Header("Dependencies")]
    [SerializeField] private Image _frame;
    [SerializeField] private Image _lastFrame;
    [SerializeField] private Image _currentFrame;
    [SerializeField] private Image _deltaTime;
    [SerializeField] private TMP_InputField _targetFPSInputField;
    [SerializeField] private TMP_Text _pauseButtonText;

    [Header("Transforms")]
    [SerializeField] private Transform _frameContainer;

    [Header("Stats UI")]
    [SerializeField] private TMP_Text _fpsStatsText;
    [SerializeField] private TMP_Text _timeElapsedText;
    [SerializeField] private TMP_Text _currentDeltaTimeText;

    [Header("UIView Dependencies")]
    [SerializeField] private UIViewTimeContainer _uiViewTimeContainer;
    [SerializeField] private UIViewFrameContainer _uiViewFrameContainer;
    [SerializeField] private UIViewFixedFrameContainer _uiViewFixedFrameContainer;

    [Header("Settings")]
    [SerializeField] private Toggle _showMissedFramesToggle;
    [SerializeField] private TMP_InputField _setFixedTimestepInputField;
    [SerializeField] private TMP_InputField _setTimeScaleInputField;

    private bool _isEventStarted = false;
    private bool _isEventPaused = false;
    private float _timeElapsed = 0;
    private bool _isMissedFramesShown = false;

    private void Awake()
    {
        _uiViewTimeContainer.OnActionCompleted += OnTimeCycleCompletedEventHandler;
        Application.targetFrameRate = _frameCount;

        _showMissedFramesToggle.onValueChanged.AddListener(delegate { OnFrameMissedToggle(_showMissedFramesToggle); });
    }

    private void OnFrameMissedToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            _isMissedFramesShown = true;
        }
        else
        {
            _isMissedFramesShown = false;
        }
    }

    private void OnTimeCycleCompletedEventHandler()
    {
        _uiViewFrameContainer.ResetIndex();
    }

    public void StartEvent()
    {
        if (_isEventPaused)
        {
            _pauseButtonText.text = $"Pause";
            _isEventPaused = false;
        }

        SetTargetFramerate();
        ResetStats();

        _uiViewFrameContainer.Show();
        _isEventStarted = true;
        
    }

    public void StopEvent()
    {
        if (_isEventPaused)
        {
            _pauseButtonText.text = $"Pause";
            _isEventPaused = false;
        }
        _isEventStarted = false;
        _uiViewFrameContainer.Hide();
        _uiViewFixedFrameContainer.Stop();
        ResetStats();
    }

    public void Pause()
    {
        if (!_isEventPaused)
        {
            _isEventPaused = true;
            _pauseButtonText.text = $"Resume";
        }
        else{
            _isEventPaused = false;
            _pauseButtonText.text = $"Pause";
        }
    }

    public void SetTargetFramerate()
    {
        if(int.Parse(_targetFPSInputField.text) > 120)
        {
            _frameCount = 60;

            Application.targetFrameRate = _frameCount;
            _uiViewFrameContainer.UpdateFrameCount(_frameCount);
            _targetFPSInputField.text = "120";
            return;
        }

        _frameCount = int.Parse(_targetFPSInputField.text);
        Application.targetFrameRate = _frameCount;

        _uiViewFrameContainer.UpdateFrameCount(_frameCount);
    }

    public void SetFixedTimestep() => Time.fixedDeltaTime = float.Parse(_setFixedTimestepInputField.text);

    public void SetTimeScale() => Time.timeScale = float.Parse(_setTimeScaleInputField.text);

    private void Update()
    {
        if (!_isEventStarted) return;
        if (_isEventPaused) return;

        _uiViewFrameContainer.UpdateFrameIndexes(_frameCount);
        _uiViewTimeContainer.UpdateTime();

        _fpsStatsText.text = "FPS: " + (1 / Time.deltaTime).ToString("F1");

        _timeElapsed += Time.deltaTime;
        _timeElapsedText.text = $"Time elapsed: {_timeElapsed.ToString("F0")}s";

        _currentDeltaTimeText.text = $"deltaTime: {Time.deltaTime}";
    }

    private void FixedUpdate()
    {
        if (!_isEventStarted) return;
        if (_isEventPaused) return;

        if (_isMissedFramesShown)
        {
            _uiViewFrameContainer.UpdateMissingFrame(_isMissedFramesShown);
        }

        _uiViewFixedFrameContainer.UpdateFrames();
    }

    private void ResetStats()
    {
        _timeElapsed = 0;
        _uiViewFrameContainer.ResetIndex();
        _uiViewTimeContainer.Reset();
    }
}