using UnityEngine;

namespace Script.Clock
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private TimeType type;
        [SerializeField] private RectTransform rectTransform;

        public TimeType Type => type;

        public RectTransform RectTransform => rectTransform;
    }
}