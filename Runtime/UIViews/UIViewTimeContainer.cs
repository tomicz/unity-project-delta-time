using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TOMICZ.DeltaTimeSimulator.UIViews
{
    public class UIViewTimeContainer : MonoBehaviour
    {
        public Action OnActionCompleted;

        [SerializeField] private Image _currentSecondImage;
        [SerializeField] private Transform _secondsContainer;

        private Vector2 _startPosition = Vector2.zero;
        private Vector3 _endPosition = Vector3.zero;

        private float _speed = 0;

        private void Awake()
        {
            //StartCoroutine(WaitFrame());


        }

        private void Start()
        {
            //_currentSecondImage.transform.localPosition = _startPosition;

        }

        public void UpdateTime()
        {
            //_currentSecondImage.transform.localPosition += Vector3.down * _speed * Time.deltaTime;

            //if (_currentSecondImage.transform.position.y <= _endPosition.y)
            //{
            //    OnActionCompleted?.Invoke();
            //    _currentSecondImage.transform.localPosition = _startPosition;
            //}
        }

        public void Reset()
        {
            _currentSecondImage.transform.localPosition = _startPosition;
        }

        private void InitiliseCurrentSecond()
        {
            //_startPosition = new Vector2(_secondsContainer.GetComponent<RectTransform>().anchoredPosition.x / 2, _secondsContainer.GetComponent<RectTransform>().anchoredPosition.y + _secondsContainer.GetComponent<RectTransform>().rect.height / 2);
            //_endPosition = new Vector2(0, _secondsContainer.GetComponent<RectTransform>().anchoredPosition.y * 2);
            //Debug.Log(_startPosition);

            //_speed = (_startPosition.y + _endPosition.y) / 1;
        }

        private IEnumerator WaitFrame()
        {
            yield return new WaitForEndOfFrame();
            InitiliseCurrentSecond();
        }
    }
}