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
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text label;
        [SerializeField] private Transform visualChild;

#if UNITY_EDITOR
        [Header("Text")] 
        [SerializeField] private string text;
#endif
        
        [Header("Background colors")]
        [SerializeField] private Color normalBgColor;
        [SerializeField] private Color hoverBgColor;
        [SerializeField] private Color pressedBgColor;
        
        [Header("Text colors")]
        [SerializeField] private Color normalTextColor = Color.white;
        [SerializeField] private Color hoverTextColor = Color.white;
        [SerializeField] private Color pressedTextColor = Color.white;
        
        [Header("Events")]
        public UnityEvent onPressed;
        public UnityEvent onReleased;
        public UnityEvent onHover;
        public UnityEvent onHoverEnd;

        public bool IsHovering { get; private set; } = false;
        public bool IsPressed { get; private set; } = false;

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
            IsHovering = true;
            if (!IsPressed)
                SetColors(hoverBgColor, hoverTextColor);
            
            onHover?.Invoke();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            IsHovering = false;
            if (!IsPressed)
                SetColors(normalBgColor, normalTextColor);
            
            onHoverEnd?.Invoke();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            SetColors(hoverBgColor, hoverTextColor);
            
            if (IsHovering)
                onPressed?.Invoke();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp");
            
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
            if (this.label != null)
                this.label.color = labelColor;
        }

        private void ResetState()
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
