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
        public enum State
        {
            None,
            Hover,
            Pressed,
        }
        
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
        protected bool canSetVisuals = true;
        protected bool isLocked = false; // locked by lock conditions, visually the same

        // public
        public Transform VisualChild => visualChild;
        
        private void OnDisable()
        {
            ResetState();
        }

        private void Start()
        {
            SetColors(State.None);
        }

        #region HandlePointer
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!isActive || isLocked)
                return;
            
            IsHovering = true;
            if (!IsPressed)
                SetColors(State.Hover);
            
            onHover?.Invoke();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!isActive || isLocked)
                return;
            
            IsHovering = false;
            if (!IsPressed)
                SetColors(State.None);
            
            onHoverEnd?.Invoke();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!isActive || isLocked)
                return;
            
            IsPressed = true;
            SetColors(State.Pressed);
            
            if (IsHovering)
                onPressed?.Invoke();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!isActive || isLocked)
                return;
            
            IsPressed = false;
            if (IsHovering)
                SetColors(State.Hover);
            else
                SetColors(State.None);
            
            onReleased?.Invoke();
        }

        #endregion

        protected virtual void SetColors(State state)
        {
            if (!canSetVisuals)
                return;

            Color bgColor = state switch
            {
                State.None => normalBgColor,
                State.Hover => hoverBgColor,
                State.Pressed => pressedBgColor,
                _ => normalBgColor
            };
            
            Color labelColor = state switch
            {
                State.None => normalTextColor,
                State.Hover => hoverTextColor,
                State.Pressed => pressedTextColor,
                _ => normalTextColor
            };
            
            if (background != null)
                background.color = bgColor;
            if (label != null)
                label.color = labelColor;
        }

        protected void ResetState()
        {
            IsPressed = false;
            IsHovering = false;
            SetColors(State.None);
        }

        protected void SetVisualsHereAndHow()
        {
            if (IsPressed)
                SetColors(State.Pressed);
            else if (IsHovering)
                SetColors(State.Hover);
            else
                SetColors(State.None);
        }
        
        public void SetLocked(bool locked) => isLocked = locked;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            SetColors(State.None);
            
            if (label != null)
                label.text = text;
            if (visualChild == null && gameObject.transform.childCount > 0)
                visualChild = gameObject.transform.GetChild(0);
        }
        #endif
    }
}
