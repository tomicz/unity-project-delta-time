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

    private bool _isEventStarted = false;
    private bool _isDeltaTimeEnabled = false;
    private float _timeElapsed = 0;
    private Vector3 _carStartPosition = Vector3.zero;

    private void Awake()
    {
        _uiViewTimeContainer.OnActionCompleted += OnTimeCycleCompletedEventHandler;
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
        _uiViewFrameContainer.Show();
        _isEventStarted = true;
        ToggleDeltaTime();
    }

    public void StopEvent()
    {
        _uiViewFrameContainer.ResetIndex();
        _isEventStarted = false;
        _uiViewFrameContainer.Hide();
    }

    public void SetTargetFramerate()
    {
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

    private void Update()
    {
        if (!_isEventStarted) return;

        PushMovingEntityForward();
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
}