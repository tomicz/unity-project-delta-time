using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TOMICZ.DeltaTimeSimulator.UIViews
{
    public class UIViewTimeContainer : MonoBehaviour
    {
        [SerializeField] private Image _currentSecondImage;
        [SerializeField] private Transform _secondsContainer;

        private Vector3 _startPosition = Vector3.zero;
        private Vector3 _endPosition = Vector3.zero;

        private Image[] _secondsArray; 

        private void Awake()
        {
            GetAllSecondsIntoAnArray();
        }

        private void Start()
        {
            StartCoroutine(WaitFrame());
        }

        private void Update()
        {
            float speed = _startPosition.y - _endPosition.y / 60;
            _currentSecondImage.transform.position += Vector3.down * speed * Time.deltaTime;

            if(_currentSecondImage.transform.position.y <= _endPosition.y)
            {
                _currentSecondImage.transform.position = _startPosition;
            }
        }

        private void GetAllSecondsIntoAnArray()
        {
            _secondsArray = _secondsContainer.transform.GetComponentsInChildren<Image>();
        }

        private void InitiliseCurrentSecond()
        {
            float x = _secondsArray[0].GetComponent<RectTransform>().sizeDelta.x;
            float y = _secondsArray[0].GetComponent<RectTransform>().sizeDelta.y;

            _startPosition = _secondsArray[0].transform.position;
            _endPosition = _secondsArray[_secondsArray.Length - 1].transform.position;

            _currentSecondImage.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
            _currentSecondImage.transform.position = _startPosition;
        }

        private IEnumerator WaitFrame()
        {
            yield return new WaitForEndOfFrame();

            InitiliseCurrentSecond();
        }
    }
}