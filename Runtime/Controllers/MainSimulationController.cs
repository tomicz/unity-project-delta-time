using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainSimulationController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private UIViewSimulationContainer _uiViewSimulationContainerPrefab = null;
    [SerializeField] private Transform _simulationsContainer = null;
    [SerializeField] private TMP_Text _pauseButtonText;

    private List<UIViewSimulationContainer> _containersList = new List<UIViewSimulationContainer>();
    private int _count = 0;
    private bool _areEventsPaused = false;

    private void Start()
    {
        Add();
    }

    public void Add()
    {
        if(_containersList.Count == _count)
        {
            UIViewSimulationContainer container = Instantiate(_uiViewSimulationContainerPrefab, _simulationsContainer);

            container.SetSimulationName($"Vehicle id: {_containersList.Count}");
            container.SetID(_count);
            container.SetDependencies(this);
            _containersList.Add(container);
            _count++;
        }
        else
        {
            _containersList[_count].gameObject.SetActive(true);
            _count++;
        }
    }

    public void Remove(int id)
    {
        _containersList[id].gameObject.SetActive(false);
        _count--;
    }

    public void Run()
    {
        foreach (var simulator in _containersList)
        {
            simulator.Run();
        }

        _pauseButtonText.text = "Pause";
    }

    public void Pause()
    {
        if (_areEventsPaused)
        {
            _areEventsPaused = false;
            _pauseButtonText.text = "Resume";
        }
        else
        {
            _areEventsPaused = true;
            _pauseButtonText.text = "Pause";
        }

        foreach (var simulator in _containersList)
        {
            simulator.Pause(_areEventsPaused);
        }
    }

    public void Stop()
    {
        foreach (var simulator in _containersList)
        {
            simulator.Stop();
        }

        _pauseButtonText.text = "Pause";
    }
}