using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI
{
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Links")]
        [SerializeField] protected Image background;
        [SerializeField] protected TMP_Text label;
        [SerializeField] protected Transform visualChild;

#if UNITY_EDITOR
        [Header("Text")] 
        [SerializeField] private string text;
#endif
        
        [Header("Background colors")]
        [SerializeField] protected Color normalBgColor;
        [SerializeField] protected Color hoverBgColor;
        [SerializeField] protected Color pressedBgColor;
        
        [Header("Text colors")]
        [SerializeField] protected Color normalTextColor = Color.white;
        [SerializeField] protected Color hoverTextColor = Color.white;
        [SerializeField] protected Color pressedTextColor = Color.white;
        
        [Header("Events")]
        public UnityEvent onPressed;
        public UnityEvent onReleased;
        public UnityEvent onHover;
        public UnityEvent onHoverEnd;

        public bool IsHovering { get; private set; } = false;
        public bool IsPressed { get; private set; } = false;

        protected bool isActive = true;

        // public
        public Transform VisualChild => visualChild;

        private void OnDisable()
        {
            ResetState();
        }

        private void Start()
        {
            SetColors(normalBgColor, normalTextColor);
        }

        #region HandlePointer
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!isActive)
                return;
            
            IsHovering = true;
            if (!IsPressed)
            {
                SetColors(hoverBgColor, hoverTextColor);
            }
            
            onHover?.Invoke();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!isActive)
                return;
            
            IsHovering = false;
            if (!IsPressed)
                SetColors(normalBgColor, normalTextColor);
            
            onHoverEnd?.Invoke();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!isActive)
                return;
            
            IsPressed = true;
            SetColors(pressedBgColor, pressedTextColor);
            
            if (IsHovering)
                onPressed?.Invoke();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!isActive)
                return;
            
            IsPressed = false;
            if (IsHovering)
                SetColors(hoverBgColor, hoverTextColor);
            else
                SetColors(normalBgColor, normalTextColor);
            
            onReleased?.Invoke();
        }

        #endregion

        private void SetColors(Color bgColor, Color labelColor)
        {
            if (background != null)
                background.color = bgColor;
            if (label != null)
                label.color = labelColor;
        }

        protected void ResetState()
        {
            IsPressed = false;
            IsHovering = false;
            SetColors(normalBgColor, normalTextColor);
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            SetColors(normalBgColor, normalTextColor);
            
            if (label != null)
                label.text = text;
            if (visualChild == null && gameObject.transform.childCount > 0)
                visualChild = gameObject.transform.GetChild(0);
        }
        #endif
    }
}
