using TMPro;
using TOMICZ.DeltaTimeSimulator.UIViews;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounterController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int _frameCount = 60;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _carOffsetY = 60f;

    [Header("Dependencies")]
    [SerializeField] private Image _frame;
    [SerializeField] private Image _lastFrame;
    [SerializeField] private Image _currentFrame;
    [SerializeField] private Image _deltaTime;
    [SerializeField] private Image _car;
    [SerializeField] private Toggle _isDeltaTimeEnabledToggle;
    [SerializeField] private Toggle _isFixedDeltaTimeEnabledToggle;
    [SerializeField] private Toggle _loopToggle;
    [SerializeField] private TMP_InputField _targetFPSInputField;
    [SerializeField] private TMP_InputField _speedInputField;
    [SerializeField] private TMP_Text _pauseButtonText;

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
    [SerializeField] private UIViewFixedFrameContainer _uiViewFixedFrameContainer;

    private bool _isEventStarted = false;
    private bool _isDeltaTimeEnabled = false;
    private bool _isEventPaused = false;
    private bool _isFixedDeltaTimeEnabled = false;
    private bool _isUpdateRunning = false;
    private float _timeElapsed = 0;
    private Vector3 _carStartPosition = Vector3.zero;

    private void Awake()
    {
        _uiViewTimeContainer.OnActionCompleted += OnTimeCycleCompletedEventHandler;
        Application.targetFrameRate = _frameCount;
    }

    private void OnTimeCycleCompletedEventHandler()
    {
        _uiViewFrameContainer.ResetIndex();
    }

    private void OnEnable()
    {
        _carStartPosition = _car.transform.position;
    }

    public void StartEvent()
    {
        if (_isEventPaused)
        {
            _pauseButtonText.text = $"Pause";
            _isEventPaused = false;
        }

        ToggleDeltaTime();
        ToggleFixedDeltaTime();
        ToggleLoop();
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
        if(int.Parse(_targetFPSInputField.text) > 60)
        {
            _frameCount = 60;

            Application.targetFrameRate = _frameCount;
            _uiViewFrameContainer.UpdateFrameCount(_frameCount);
            _targetFPSInputField.text = "60";
            return;
        }

        _frameCount = int.Parse(_targetFPSInputField.text);
        Application.targetFrameRate = _frameCount;

        _uiViewFrameContainer.UpdateFrameCount(_frameCount);
    }

    public void SetSpeed()
    {
        _moveSpeed = float.Parse(_speedInputField.text);
    }

    public void EnableDeltaTime() => _isDeltaTimeEnabled = true;

    public void DisableDeltaTime() => _isDeltaTimeEnabled = false;

    public void EnableDeltaTimeToggle(bool enabled) => _isDeltaTimeEnabledToggle.isOn = enabled;

    public void EnableFixedDeltaTimeToggle(bool enabled) => _isFixedDeltaTimeEnabledToggle.isOn = enabled;

    private void Update()
    {
        if (!_isEventStarted) return;
        if (_isEventPaused) return;

        if (_isUpdateRunning)
        {
            if (_isDeltaTimeEnabled)
            {
                PushMovingEntityForwardDeltaTime();
            }
            else
            {
                PushMovingEntityForwardUpdate();
            }
        }


        _uiViewFrameContainer.UpdateFrameIndexes(_frameCount);
        _uiViewTimeContainer.UpdateTime();

        _fpsStatsText.text = "FPS: " + (1 / Time.deltaTime).ToString("F1");

        _timeElapsed += Time.deltaTime;
        _timeElapsedText.text = $"Time elapsed: {_timeElapsed.ToString("F0")}s";

        if (_isDeltaTimeEnabled)
        {
            _currentDeltaTimeText.text = $"deltaTime: {Time.deltaTime}";
        }
    }

    private void FixedUpdate()
    {
        if (!_isEventStarted) return;
        if (_isEventPaused) return;

        if (!_isUpdateRunning)
        {
            if (_isFixedDeltaTimeEnabled)
            {
                PushMovingEntityForwardFixedDeltaTime();
            }
            else
            {
                PushMovingEntityForwardFixedUpdate();
            }
        }

        _uiViewFixedFrameContainer.UpdateFrames();
    }

    private void PushMovingEntityForwardUpdate()
    {
        _car.transform.position += Vector3.right * _moveSpeed; ;
        _moveSpeedText.text = $"Move speed: {_moveSpeed}/frame";
        _distanceCrossedText.text = $"Distance crossed: {(_carStartPosition.x - _car.transform.position.x).ToString("F0").Substring(1)} units";
    }

    private void PushMovingEntityForwardDeltaTime()
    {
        _car.transform.position += Vector3.right * (_moveSpeed * Time.deltaTime);
        _moveSpeedText.text = $"Move speed: {_moveSpeed}/second";
        _distanceCrossedText.text = $"Distance crossed: {(_carStartPosition.x - _car.transform.position.x).ToString("F0").Substring(1)} units";
    }

    private void PushMovingEntityForwardFixedUpdate()
    {
        _car.transform.position += Vector3.right * _moveSpeed;
        _moveSpeedText.text = $"Move speed: {_moveSpeed}/phyx";
        _distanceCrossedText.text = $"Distance crossed: {(_carStartPosition.x - _car.transform.position.x).ToString("F0").Substring(1)} units";
    }

    private void PushMovingEntityForwardFixedDeltaTime()
    {
        _car.transform.position += Vector3.right * (_moveSpeed * Time.fixedDeltaTime);
        _moveSpeedText.text = $"Move speed: {_moveSpeed}/s";
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

    private void ToggleFixedDeltaTime()
    {
        if (_isFixedDeltaTimeEnabledToggle.isOn)
        {
            _isFixedDeltaTimeEnabled = true;
        }
        else
        {
            _isFixedDeltaTimeEnabled = false;
        }
    }

    private void ToggleLoop()
    {
        if (!_loopToggle.isOn)
        {
            _loopToggle.transform.parent.GetComponentInChildren<TMP_Text>().text = "Loop: Update";
            _isUpdateRunning = true;
            StopEvent();
        }
        else
        {
            _loopToggle.transform.parent.GetComponentInChildren<TMP_Text>().text = "Loop: FixedUpdate";
            _isUpdateRunning = false;
            StopEvent();
        }
    }

    private void ResetStats()
    {
        _timeElapsed = 0;
        _car.transform.position = _carStartPosition;
        _uiViewFrameContainer.ResetIndex();
        _uiViewTimeContainer.Reset();
    }
}