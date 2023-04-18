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
    [SerializeField] private Image _vehicle;
    [SerializeField] private Transform _endPositionTransform;
    [SerializeField] private RectTransform _distanceImage;
    [SerializeField] private RectTransform _race;

    private MainSimulationController _mainSimulationController;
    private int _id;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _distance = 0;
    private float _vehicleSpeed = 0;

    private void OnEnable()
    {
        LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
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

        _startPosition = new Vector3(_race.anchoredPosition.x - _race.rect.width / 2, 0, 0);
        _endPosition = new Vector3(_endPositionTransform.position.x - (_vehicle.GetComponent<RectTransform>().rect.width / 2), 0, 0);
        _vehicle.transform.localPosition = _startPosition;
    }

    private void Update()
    {
        MoveVehicleRight();
    }

    public void SetSimulationName(string name) => _nameText.text = name;

    public void SetID(int id) => _id = id;

    public void SetDependencies(MainSimulationController mainSimulationController) => _mainSimulationController = mainSimulationController;

    public void Remove()
    {
        _mainSimulationController.Remove(_id);
    }

    private void MoveVehicleRight()
    {
        if (HasVehicleReachedFinishLine()) return;

        _vehicle.transform.localPosition += Vector3.right * _vehicleSpeed;
    }

    private bool HasVehicleReachedFinishLine()
    {
        if(_vehicle.transform.position.x >= _endPosition.x - 8)
        {
            return true;
        }

        return false;
    }
}