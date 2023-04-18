using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIViewFixedFrameContainer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private RectTransform _frame = null;
    [SerializeField] private RectTransform _frameContainer = null;
    [SerializeField] private Color _frameColor;
    [SerializeField] private Color _missedFrameColor;

    private Transform[] _frameArray = null;
    private float _frameCount = 0;
    private int _frameIndex = 0;

    private void Awake()
    {
        StartCoroutine(WaitForGUI());
    }

    public void UpdateMissedFrames()
    {
        _frameArray[_frameIndex].GetComponent<Image>().color = _missedFrameColor;
    }

    public void UpdateFrames()
    {
        _frameArray[_frameIndex].gameObject.SetActive(true);
        _frameIndex++;

        if (_frameIndex >= _frameCount - 1)
        {
            foreach (var frame in _frameArray)
            {
                frame.gameObject.SetActive(false);
            }

            _frameIndex = 0;
        }
    }

    public void Stop()
    {
        foreach (var frame in _frameArray)
        {
            frame.gameObject.SetActive(false);
        }
    }

    private void CreateFrameArray()
    {
        _frameCount = 1f / Time.fixedDeltaTime;
        _frameArray = new Transform[(int)_frameCount];

        float frameScaleY = (_frameContainer.rect.height / _frameCount);

        for (int i = 0; i < _frameCount; i++)
        {
            RectTransform frame = Instantiate(_frame, _frameContainer);
            frame.sizeDelta = new Vector2(frame.sizeDelta.x, frameScaleY);
            frame.GetComponent<Image>().color = _frameColor;
            frame.gameObject.SetActive(false);

            _frameArray[i] = frame.transform;

        }
    }

    private IEnumerator WaitForGUI()
    {
        yield return new WaitForEndOfFrame();

        CreateFrameArray();
    }
}