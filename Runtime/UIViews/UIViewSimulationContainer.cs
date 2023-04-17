using TMPro;
using UnityEngine;

public class UIViewSimulationContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;

    private MainSimulationController _mainSimulationController;
    private int _id;

    public void SetSimulationName(string name) => _nameText.text = name;

    public void SetID(int id) => _id = id;

    public void SetDependencies(MainSimulationController mainSimulationController) => _mainSimulationController = mainSimulationController;

    public void Remove()
    {
        _mainSimulationController.Remove(_id);
    }
}