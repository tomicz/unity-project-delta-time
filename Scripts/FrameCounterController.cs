using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounterController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Image _lastFrame;
    [SerializeField] private Image _currentFrame;
    [SerializeField] private Image _deltaTime;

    [Header("Properties")]
    [SerializeField] private int _frameCount = 60;
    [SerializeField] private float _frameOffset = 80f;
    [SerializeField] private float _frameDelay = 1f;

    [SerializeField] private GameObject[] _framesArray;

    private int _frameCounter = 1;

    public void StartEvent()
    {
        InitiliseFrames();
        StartCoroutine(StartFrameCounting());
    }

    public void StopEvent()
    {
        StopAllCoroutines();
        InitiliseFrames();
    }

    private void InitiliseFrames()
    {
        _frameCounter = 1;
        _lastFrame.transform.position = _framesArray[0].transform.position;
        _currentFrame.transform.position = _framesArray[0].transform.position;

        _deltaTime.GetComponentInChildren<TMP_Text>().text = 0.ToString();
        _deltaTime.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        _deltaTime.transform.position = new Vector2(_currentFrame.transform.position.x, _framesArray[0].transform.position.y);
    }

    private IEnumerator StartFrameCounting()
    {
        while(_frameCounter < _frameCount)
        {
            yield return new WaitForSeconds(_frameDelay);

            UpdateFrame();
            _frameCounter++;
        }

        yield return new WaitForSeconds(_frameDelay);

        InitiliseFrames();
    }

    private void UpdateFrame()
    {
        _lastFrame.transform.position = _framesArray[_frameCounter - 1].transform.position;
        _currentFrame.transform.position = _framesArray[_frameCounter].transform.position;
        SetDeltaTime();
    }

    private void SetDeltaTime()
    {
        Vector2 size = new Vector2(_frameOffset, 10);

        _deltaTime.GetComponent<RectTransform>().sizeDelta = size;
        _deltaTime.transform.position = new Vector2((_currentFrame.transform.position.x + _lastFrame.transform.position.x) / 2, _framesArray[0].transform.position.y);
        _deltaTime.GetComponentInChildren<TMP_Text>().text = Random.Range(0.01f, 0.03f).ToString("F4");
    }
}