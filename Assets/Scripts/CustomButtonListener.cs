using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoundCipher
{
    [RequireComponent(typeof(Button))]
    public class CustomButtonListener : MonoBehaviour, IPointerClickHandler
    {
        [Tooltip("Duration between 2 clicks in seconds")]
        [Range(0.01f, 0.5f)] public float doubleClickDuration = 0.5f;
        public UnityEvent onSingleClick;
        public UnityEvent onDoubleClick;

        private byte clicks = 0;
        private DateTime firstClickTime;

        private Button button;



        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            double elapsedSeconds = (DateTime.Now - firstClickTime).TotalSeconds;
            if (elapsedSeconds > doubleClickDuration)
            {
                clicks = 0;
                onSingleClick?.Invoke();
            }

            clicks++;

            if (clicks == 1)
                firstClickTime = DateTime.Now;
            else if (clicks > 1)
            {
                if (elapsedSeconds <= doubleClickDuration)
                {
                    if (button.interactable)
                        onDoubleClick?.Invoke();
                }
                clicks = 0;
            }
        }
    }
}