using System.Collections.Generic;
using UnityEngine;

public class MainSimulationController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private UIViewSimulationContainer _uiViewSimulationContainerPrefab = null;
    [SerializeField] private Transform _simulationsContainer = null;

    private List<UIViewSimulationContainer> _containersList = new List<UIViewSimulationContainer>();
    private int _count = 0;

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
}