using System;
using TMPro;
using UnityEngine;

namespace Script.Clock
{
    public class LineClock : MonoBehaviour
    {
        [SerializeField] private TimeService timeService;
        [SerializeField] private TMP_InputField hourInput;
        [SerializeField] private TMP_InputField minuteInput;
        [SerializeField] private TMP_InputField secondsInput;
        private bool _isEdit;

        private void Update()
        {
            UpdateLineClock(timeService.CurrentTime);
        }
        
        public void SetEditMode(bool result)
        {
            hourInput.interactable = result;
            minuteInput.interactable = result;
            secondsInput.interactable = result;
        }

        private void UpdateLineClock(DateTime time)
        {
            if (!_isEdit && !hourInput.isFocused)
            {
                hourInput.text = time.Hour.ToString();
            }

            if (!_isEdit && !minuteInput.isFocused)
            {
                minuteInput.text = time.Minute.ToString();
            }

            if (!_isEdit && !secondsInput.isFocused)
            {
                secondsInput.text = time.Second.ToString();
            }
        }

        public void OnEndEditHour()
        {
            var isCompared = int.TryParse(hourInput.text, out var value);
            if (!isCompared) return;
            if (value >= 24) return;
            timeService.ChangeTime(TimeType.hour, value);
        }

        public void OnEndEditMin()
        {
            var isCompared = int.TryParse(minuteInput.text, out var value);
            if (!isCompared) return;
            if (value >= 60) return;
            timeService.ChangeTime(TimeType.minute, value);
        }

        public void OnEndEditSeconds()
        {
            var isCompared = int.TryParse(secondsInput.text, out var value);
            if (!isCompared) return;
            if (value >= 60) return;
            timeService.ChangeTime(TimeType.second, value);
        }
    }
}