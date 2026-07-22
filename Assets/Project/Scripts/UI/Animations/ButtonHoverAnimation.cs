using System.Collections.Generic;
using PrimeTween;
using Project.UI;
using UnityEngine;

namespace Project.TweenAnimations
{
    [RequireComponent(typeof(CustomButton))]
    public class ButtonHoverAnimation : MonoBehaviour
    {
        [Header("Offset settings")]
        [SerializeField] private bool useOffset = true;
        [SerializeField] private Vector2 offset = new (0f, 5f);
        [SerializeField] private float offsetDuration = 0.25f;
        [SerializeField] private Ease easeOffset = Ease.InOutQuad;
        
        [Header("Scale settings")]
        [SerializeField] private bool useScale = true;
        [SerializeField] private float scale = 1.2f;
        [SerializeField] private float scaleDuration = 0.25f;
        [SerializeField] private Ease easeScale = Ease.InOutQuad;
        
        [Header("Selector settings")]
        [SerializeField] private bool useHighlight = true;
        [SerializeField] private RectTransform highlight;
        [SerializeField] private float highlightWidth = 100f;
        [SerializeField] private float highlightDuration = 0.25f;
        [SerializeField] private Ease easeHighlight = Ease.InOutQuad;
        
        private CustomButton _button;
        private Vector3 _originalPosition;
        private Vector3 _originalScale;
        private float _originalHighlightWidth;
        private Sequence _animationSequence;
        
        private Transform ButtonAnimTransform => _button.VisualChild.transform;

        private void OnEnable()
        {
            _button.onHover.AddListener(HoverStart);
            _button.onHoverEnd.AddListener(HoverCancel);
            
            Reset(); // otherwise it will stay in animation when we return to the menu page
        }

        private void OnDisable()
        {
            _button.onHover.RemoveListener(HoverStart);
            _button.onHoverEnd.RemoveListener(HoverCancel);
        }
        
        private void Awake()
        {
            _button = GetComponent<CustomButton>();
            _originalPosition = ButtonAnimTransform.localPosition;
            _originalScale = ButtonAnimTransform.localScale;
            
            if (highlight != null)
                _originalHighlightWidth = highlight.sizeDelta.x;
        }

        private void HoverStart() => PlayHoverAnimation(true);
        private void HoverCancel() => PlayHoverAnimation(false);

        #region Animation

        private void PlayHoverAnimation(bool isHovering)
        {
            if (ButtonAnimTransform == null)
            {
                Debug.LogError("[ButtonHoverAnimation]: ButtonAnimTransform is null");
                return;
            }

            if (_animationSequence.isAlive)
                _animationSequence.Stop();
            
            List<Tween> tweens = new(3);
            if (useOffset) 
                tweens.Add(MakeOffsetTween(isHovering));
            if (useScale) 
                tweens.Add(MakeScaleTween(isHovering));
            if (useHighlight) 
                tweens.Add(MakeHighlightWidthTween(isHovering));

            if (tweens.Count == 0) return;

            _animationSequence = Sequence.Create(useUnscaledTime: true);
            foreach (Tween tween in tweens)
                _animationSequence.Group(tween);
        }

        private Tween MakeOffsetTween(bool isHovering)
        {
            Vector3 v3Offset = new(offset.x, offset.y, 0f);
            Vector3 targetPosition = isHovering ? _originalPosition + v3Offset : _originalPosition;

            float maxDistance = offset.magnitude;
            float currentDistance = Vector3.Distance(targetPosition, ButtonAnimTransform.localPosition);
            float calculatedDuration = Mathf.Clamp(offsetDuration * currentDistance / maxDistance, 0f, offsetDuration);

            return Tween.LocalPosition(
                ButtonAnimTransform,
                targetPosition,
                calculatedDuration,
                easeOffset
            );
        }

        private Tween MakeScaleTween(bool isHovering)
        {
            Vector3 targetScale = isHovering ? _originalScale * scale : _originalScale;
            float currentDistance = Vector3.Distance(targetScale, ButtonAnimTransform.localScale);
            float maxDistance = Vector3.Distance(_originalScale * scale, _originalScale);
            float calculatedDuration = Mathf.Clamp(scaleDuration * currentDistance / maxDistance, 0f, scaleDuration);

            return Tween.Scale(
                ButtonAnimTransform,
                targetScale,
                calculatedDuration,
                easeScale
            );
        }
        
        private Tween MakeHighlightWidthTween(bool isHovering)
        {
            if (highlight == null)
            {
                Debug.LogError("[ButtonHoverAnimation]: Selector is null");
                return default;
            }
            
            float targetWidth = isHovering ? highlightWidth : _originalHighlightWidth;
            float currentWidth = highlight.sizeDelta.x;
            
            float maxDistance = Mathf.Abs(highlightWidth - _originalHighlightWidth);
            float currentDistance = Mathf.Abs(targetWidth - currentWidth);
            float calculatedDuration =
                Mathf.Clamp(scaleDuration * currentDistance / maxDistance, 0f, scaleDuration);

            return Tween.Custom(
                startValue: currentWidth,
                endValue: targetWidth,
                duration: highlightDuration,
                onValueChange: width => highlight.sizeDelta = new Vector2(width, highlight.sizeDelta.y),
                easeHighlight
            );
        }
        
        #endregion

        #region Reset

        public void Reset()
        {
            if (useOffset)
                ButtonAnimTransform.localPosition = _originalPosition;
            if (useScale)
                ButtonAnimTransform.localScale = _originalScale;
            if (useHighlight)
                highlight.sizeDelta = new Vector2(_originalHighlightWidth, highlight.sizeDelta.y);
        }

        #endregion

        private void OnDestroy()
        {
            if (_animationSequence.isAlive)
                _animationSequence.Stop();
        }
    }
}
