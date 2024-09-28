using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.Clock
{
    public class Clock : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private TimeService timeService;
        [SerializeField] private RectTransform hourArrow;
        [SerializeField] private RectTransform minuteArrow;
        [SerializeField] private RectTransform secondArrow;
        [SerializeField] private Camera camera;
        private bool _isEditMode;
        private Arrow _selectedArrow;
        private float _initialArrowAngle;
        private float _initialMouseAngle;

        private void Update()
        {
            UpdateClock(timeService.CurrentTime);
        }

        public void SetEditMode(bool result)
        {
            _isEditMode = result;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isEditMode) return;
            if (eventData == null) return;
            var clickedObject = eventData.pointerPressRaycast.gameObject;
            if (clickedObject != hourArrow.gameObject && clickedObject != minuteArrow.gameObject &&
                clickedObject != secondArrow.gameObject) return;
            _selectedArrow = clickedObject.GetComponent<Arrow>();
            _selectedArrow.RectTransform.DOKill();
            _initialArrowAngle = _selectedArrow.RectTransform.localEulerAngles.z;
            _initialMouseAngle = GetMouseAngle(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_selectedArrow == null || !_isEditMode) return;
            SaveTimeFromHands();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_selectedArrow == null || !_isEditMode) return;

            RotateArrowToMouse(eventData);
        }

        private float GetMouseAngle(PointerEventData pointerData)
        {
            Vector3 worldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(hourArrow, pointerData.position,
                pointerData.pressEventCamera, out worldPos);
            var direction = worldPos - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return angle;
        }

        private void RotateArrowToMouse(PointerEventData pointerData)
        {
            if (_selectedArrow == null || !_isEditMode) return;
            var currentMouseAngle = GetMouseAngle(pointerData);
            var deltaAngle = currentMouseAngle - _initialMouseAngle;
            _selectedArrow.RectTransform.localRotation =
                Quaternion.Euler(new Vector3(0, 0, _initialArrowAngle + deltaAngle));
        }

        private void SaveTimeFromHands()
        {
            var angle = NormalizeAngle(-_selectedArrow.RectTransform.localEulerAngles.z);
            if (_selectedArrow.Type == TimeType.hour)
            {
                timeService.ChangeTime(_selectedArrow.Type, Mathf.FloorToInt(angle / 30f) % 12, false);
            }
            else
            {
                timeService.ChangeTime(_selectedArrow.Type, Mathf.FloorToInt(angle / 6f) % 60, false);
            }

            _selectedArrow = null;
        }

        private float NormalizeAngle(float angle)
        {
            angle = angle % 360f;
            if (angle < 0) angle += 360f;
            return angle;
        }

        private void UpdateClock(DateTime time)
        {
            var selectedArrowType = TimeType.none;
            if (_selectedArrow != null) selectedArrowType = _selectedArrow.Type;
            if (selectedArrowType != TimeType.hour)
            {
                var hourAngle =
                    360f * (time.Hour % 12) / 12f + (30f * time.Minute / 60f);
                hourArrow.DORotate(new Vector3(0f, 0f, -hourAngle), 1f);
            }

            if (selectedArrowType != TimeType.minute)
            {
                var minuteAngle =
                    360f * time.Minute / 60f + (6f * time.Second / 60f);
                minuteArrow.DORotate(new Vector3(0f, 0f, -minuteAngle), 1f);
            }

            if (selectedArrowType != TimeType.second)
            {
                var secondAngle = 360f * time.Second / 60f + (6f * time.Millisecond / 1000f);
                secondArrow.DORotate(new Vector3(0f, 0f, -secondAngle), 0.5f);
            }
        }
    }
}