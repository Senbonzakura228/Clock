using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script
{
    public class EditButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Clock.LineClock lineClock;
        [SerializeField] private Clock.Clock clock;
        [SerializeField] private TMP_Text tmp;
        [SerializeField] private string nonEditModeText;
        [SerializeField] private string editModeText;
        private bool _isEditMode;

        public void OnPointerClick(PointerEventData eventData)
        {
            _isEditMode = !_isEditMode;
            tmp.text = _isEditMode ? editModeText : nonEditModeText;
            clock.SetEditMode(_isEditMode);
            lineClock.SetEditMode(_isEditMode);
        }
    }
}