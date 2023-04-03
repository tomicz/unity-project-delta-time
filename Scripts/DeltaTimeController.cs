using UnityEngine;
using TMPro;

public class DeltaTimeController : MonoBehaviour
{
    [Header("Basic properties")]
    [SerializeField] private ushort _targetFps = 60;
    [SerializeField] private float _targetSpeed = 1;

    [Header("Dependencies")]
    [SerializeField] private Transform _objectA;
    [SerializeField] private Transform _objectB;

    [Header("UI")]
    [SerializeField] private TMP_Text _targetFPSText;
    [SerializeField] private TMP_Text _currentFPSText;


    private void Awake()
    {
        Application.targetFrameRate = _targetFps;
    }

    private void Update()
    {
        _objectA.transform.position += new Vector3(0f, _targetSpeed, 0f);
        _objectB.transform.position += new Vector3(0f, _targetSpeed, 0f) * Time.deltaTime;

        UpdateUI();
    }

    private void UpdateUI()
    {
        float fps = Time.frameCount / Time.time;
        _targetFPSText.text = $"Target fps: {_targetFps}";
        _currentFPSText.text = $"Current fps: {fps.ToString("F1")}";
    }
}