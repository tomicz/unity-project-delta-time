using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIViewSimulationContainer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TMP_Text _nameText = null;
    [SerializeField] private Toggle _enableDeltaTimeToggle = null;
    [SerializeField] private Toggle _enableFixedDeltaTimeToggle = null;
    [SerializeField] private TMP_InputField _vehicleSpeedInputField = null;
    [SerializeField] private Toggle _loopType = null;
    [SerializeField] private TMP_Text _loopTypeText = null;
    [SerializeField] private Image _vehicle;
    [SerializeField] private Transform _endPositionTransform;
    [SerializeField] private RectTransform _distanceImage;
    [SerializeField] private RectTransform _race;

    [Header("Colors")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _updateColor;
    [SerializeField] private Color _fixedUpdateColor;
    [SerializeField] private Color _textUpdateColor;
    [SerializeField] private Color _textFixedUpdateColor;

    [SerializeField] private Image[] _headerImageButtons;

    private MainSimulationController _mainSimulationController;
    private int _id;

    private bool _hasSimulatorStarted = false;
    private bool _isSimulatorPaused = false;
    private bool _isUpdateRunning = true;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _distance = 0;
    private float _vehicleSpeed = 0;

    private void Awake()
    {
        _loopType.onValueChanged.AddListener(delegate { OnLoopSwitchToggle(_loopType); });
    }

    private void OnEnable()
    {
        LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
        ChangeHeaderColors(_updateColor, _textUpdateColor);
    }

    private void Start()
    {
        StartCoroutine(WaitForUI());
        _distance = _race.rect.width;
        _vehicleSpeed = float.Parse(_vehicleSpeedInputField.text);
    }

    private IEnumerator WaitForUI()
    {
        yield return new WaitForEndOfFrame();

        _startPosition = new Vector3(_race.anchoredPosition.x - _race.rect.width / 2 + (_vehicle.GetComponent<RectTransform>().rect.width / 2), 0, 0);
        _endPosition = new Vector3(_endPositionTransform.position.x - (_vehicle.GetComponent<RectTransform>().rect.width / 2), 0, 0);
        _vehicle.transform.localPosition = _startPosition;
    }

    private void Update()
    {
        if (!_hasSimulatorStarted) return;
        if (_isSimulatorPaused) return;

        if (_isUpdateRunning)
        {
            MoveVehicleRight();
        }
    }

    private void FixedUpdate()
    {
        if (!_hasSimulatorStarted) return;
        if (_isSimulatorPaused) return;

        if (!_isUpdateRunning)
        {
            MoveVehicleRight();
        }
    }

    public void SetSimulationName(string name) => _nameText.text = name;

    public void SetID(int id) => _id = id;

    public void SetDependencies(MainSimulationController mainSimulationController) => _mainSimulationController = mainSimulationController;

    public void Remove()
    {
        _mainSimulationController.Remove(_id);

        Reset();
    }

    public void Run()
    {
        _hasSimulatorStarted = true;
        _vehicle.transform.localPosition = _startPosition;
        _isSimulatorPaused = false;
    }

    public void Pause(bool isPaused)
    {
        _isSimulatorPaused = isPaused;
    }

    public void Stop()
    {
        Reset();
    }

    public void SetSpeed()
    {
        _vehicleSpeed = float.Parse(_vehicleSpeedInputField.text);
    }

    private void OnLoopSwitchToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            _loopTypeText.text = $"Loop: FixedUpdate";
            ChangeHeaderColors(_fixedUpdateColor, _textFixedUpdateColor);
            _isUpdateRunning = false;
        }
        else
        {
            _isUpdateRunning = true;
            _loopTypeText.text = $"Loop: Update";
            ChangeHeaderColors(_updateColor, _textUpdateColor);
        }
    }

    private void MoveVehicleRight()
    {
        if (HasVehicleReachedFinishLine())
        {
            _hasSimulatorStarted = false;
        }
        else
        {
            _vehicle.transform.localPosition += Vector3.right * _vehicleSpeed;
        }
    }

    private bool HasVehicleReachedFinishLine()
    {
        if(_vehicle.transform.position.x >= _endPosition.x - 8)
        {
            return true;
        }

        return false;
    }

    private void ChangeHeaderColors(Color buttonColor, Color textColor)
    {
        foreach (var image in _headerImageButtons)
        {
            image.color = buttonColor;
            image.transform.GetChild(0).GetComponent<TMP_Text>().color = textColor;
        }
    }

    private void Reset()
    {
        _vehicle.transform.localPosition = _startPosition;
        _isSimulatorPaused = false;
        _hasSimulatorStarted = false;
    }
}