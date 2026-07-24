using System;
using PrimeTween;
using Project.Core.Numbers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class ModMachineModificatorView : CustomButton
    {
        public ModMachineModificator Modificator { get; private set; }
        public event Action<ModMachineModificatorView> OnSelected;
        
        private bool _isSelected;
        private Tween _selectedAnimation;
        private Vector3 _originalLabelScale;
        [SerializeField] private float selectedScaleMultiplier = 0.95f;
        [SerializeField] private float selectedAnimationDuration = 0.1f;

        public void Init(ModMachineModificator modificator)
        {
            Modificator = modificator ?? throw new ArgumentNullException(nameof(modificator));
            Modificator.OnModValueChanged += HandleModValueChanged;
            
            HandleModValueChanged(Modificator.Value);
        }

        private void Awake()
        {
            _originalLabelScale = label.transform.localScale;
        }

        private void HandleModValueChanged(int value)
        {
            if (label == null)
                return;
            
            label.text = ModHelper.ValueToString(value);
        }
        
        public void Clear()
        {
            if (Modificator != null)
                Modificator.OnModValueChanged -= HandleModValueChanged;
        }

        #region Select

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_isSelected)
                return;
            
            _isSelected = true;
            base.OnPointerDown(eventData);
            
            canSetVisuals = false; // so it stay selected
            OnSelected?.Invoke(this);
            PlaySelectedAnimation();
        }

        public void Unselect()
        {
            if (!_isSelected)
                return;

            _isSelected = false;
            canSetVisuals = true;
            SetVisualsHereAndHow();
            PlayUnselectedAnimation();
        }

        #endregion

        #region Aniamtion

        private void PlaySelectedAnimation()
        {
            if (_selectedAnimation.isAlive)
                _selectedAnimation.Stop();

            _selectedAnimation = Tween.Scale(
                label.transform, 
                label.transform.localScale * selectedScaleMultiplier,
                selectedAnimationDuration,
                ease: Ease.InOutQuad);
        }

        private void PlayUnselectedAnimation()
        {
            if (_selectedAnimation.isAlive)
                _selectedAnimation.Stop();
            
            _selectedAnimation = Tween.Scale(
                label.transform, 
                _originalLabelScale,
                selectedAnimationDuration,
                ease: Ease.InOutQuad);
        }

        #endregion

        public void OnDestroy()
        {
            Clear();
            
            if (_selectedAnimation.isAlive)
                _selectedAnimation.Stop();
        }
    }
}