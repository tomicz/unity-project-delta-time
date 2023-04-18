using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIViewBuilder : MonoBehaviour
{
    [SerializeField] private RectTransform _sideWindow;

    private void Start()
    {
        StartCoroutine(RebuildUI());
    }

    private IEnumerator RebuildUI()
    {
        yield return new WaitForEndOfFrame();

        LayoutRebuilder.ForceRebuildLayoutImmediate(_sideWindow);
    }
}
