using Cysharp.Threading.Tasks;
using PrimeTween;
using Project.Core.Numbers;
using Project.Core.Robot;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.UI
{
    public class HpNumberView : CustomButton
    {
        private HpNumber _hpNumber;
        private IRobotService _robotService;
        
        [SerializeField] private ProvidedModificatorView providedModificator;
        [SerializeField] private ModAttachedView modAttachedView;

        [SerializeField] private Color damagedBgColor;
        [SerializeField] private RectTransform damagedRectImageTransform;
        [SerializeField] private float animationDuration = 0.25f;

        [Header("Damage prediction colors")] 
        [SerializeField] private Color damageNormalBgColor = Color.red;
        [SerializeField] private Color damageHoverBgColor = Color.red;
        [SerializeField] private Color damagePressedBgColor = Color.red;
        
        [Header("Success prediction colors")] 
        [SerializeField] private Color successNormalBgColor = Color.green;
        [SerializeField] private Color successHoverBgColor = Color.green;
        [SerializeField] private Color successPressedBgColor = Color.green;
        
        private HpNumberChecker.Prediction _currentPrediction;
        
        private Image _damagedImageFill;
        private float _targetRectImageTransformHeight;
        private Tween _bgColorChange;
        
        public event System.Action<HpNumber> OnHpNumberViewSelected;
        
        public void Init(HpNumber number, IRobotService robotService)
        {
            if (number == null)
            {
                Debug.LogError("[HpNumberView] HpNumber is null!");
                return;
            }
            
            if (providedModificator != null)
                providedModificator.gameObject.SetActive(true);
            
            if (modAttachedView != null)
                modAttachedView.gameObject.SetActive(true);

            if (damagedRectImageTransform != null)
            {
                _targetRectImageTransformHeight = damagedRectImageTransform.anchorMax.y;
                _damagedImageFill = damagedRectImageTransform.GetComponent<Image>();
                Color color = _damagedImageFill.color;
                color.a = 0f;
                _damagedImageFill.color = color;
            }
            
            _hpNumber = number;
            _hpNumber.OnNumberChanged += HandleHpNumber;
            _hpNumber.OnStateChanged += HandleState;
            _hpNumber.OnProvidedModificatorValueChanged += HandleProvidedModificator;
            _hpNumber.OnModMachineModificatorValueChanged += HandleModMachineModificator;
            
            HandleHpNumber(_hpNumber.CurrentValue);
            HandleState(_hpNumber.State);
            HandleProvidedModificator(_hpNumber.ProvidedModificator);
            HandleModMachineModificator(_hpNumber.ModMachineModificator?.Value ?? 0);
            
            // robot service
            _robotService = robotService ??
                throw new System.ArgumentNullException(nameof(robotService));
            _robotService.Request.OnRequestChanged += HandleRequestChanged;
        }

        public void SetUnactive()
        {
            _currentPrediction = HpNumberChecker.Prediction.None;
            HandleVisualFromPrediction(_currentPrediction);
            
            if (providedModificator != null)
                providedModificator.gameObject.SetActive(false);
            
            if (modAttachedView != null)
                modAttachedView.gameObject.SetActive(false);
            
            if (_hpNumber == null)
                return;
            _hpNumber.OnNumberChanged -= HandleHpNumber;
            _hpNumber.OnStateChanged -= HandleState;
            _hpNumber.OnProvidedModificatorValueChanged -= HandleProvidedModificator;
            _hpNumber.OnModMachineModificatorValueChanged -= HandleModMachineModificator;
            
            if (_robotService != null)
                _robotService.Request.OnRequestChanged += HandleRequestChanged;
        }

        #region HpNumber

        private void HandleHpNumber(int value)
        {
            if (_hpNumber.State == HpNumberState.Damaged)
            {
                label.text = "#";
                return;
            }
            
            label.text = value.ToString();
            HandleVisualFromPrediction(GetPredictionForRequest());
        }
        
        private void HandleState(HpNumberState state)
        {
            if (state == HpNumberState.Damaged)
            {
                isActive = false;
                DamagedFillRoutine().Forget();
            }
        }

        private void HandleProvidedModificator(int value)
        {
            if (providedModificator == null)
                return;
            
            if (value > 0)
                providedModificator.SetPositive();
            else if (value < 0)
                providedModificator.SetNegative();
            else
                providedModificator.SetNeutral();
        }

        private void HandleModMachineModificator(int value)
        {
            if (modAttachedView == null)
            {
                Debug.LogError("[HpNumberView] ModAttachedView is null!");
                return;
            }
            modAttachedView.SetValue(value);
        }
        
        #endregion

        #region Damaged
        
        private async UniTaskVoid DamagedFillRoutine()
        {
            canSetVisuals = false;
            DamagedRectAlphaChange(true, false);
            AnimateBackgroundColor(damagedBgColor);
            HandleHpNumber(_hpNumber.CurrentValue); // #
            
            float startHeight = 0;
            
            while (_hpNumber.State == HpNumberState.Damaged)
            {
                float t = _hpNumber.RestorationElapsedTime / _hpNumber.RestorationTime;
                float currentHeight = Mathf.Lerp(startHeight, _targetRectImageTransformHeight, t);
                
                Vector2 anchorMax = damagedRectImageTransform.anchorMax;
                anchorMax.y = currentHeight;
                damagedRectImageTransform.anchorMax = anchorMax;
                
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: destroyCancellationToken);
            }

            isActive = true;
            canSetVisuals = true;
            Vector2 finalAnchor = damagedRectImageTransform.anchorMax;
            finalAnchor.y = _targetRectImageTransformHeight;
            damagedRectImageTransform.anchorMax = finalAnchor;
            HandleHpNumber(_hpNumber.CurrentValue); // return number
            AnimateBackgroundColor(normalBgColor);
            DamagedRectAlphaChange(false, true);
        }
        
        #endregion

        #region Predictions

        private void HandleRequestChanged()
        {
            HandleVisualFromPrediction(GetPredictionForRequest());
        }

        private HpNumberChecker.Prediction GetPredictionForRequest()
        {
            if (_robotService == null || _robotService.Request == null)   
                return HpNumberChecker.Prediction.None;
            return HpNumberChecker.PredictNumberResult(_hpNumber.CurrentValue, _robotService.Request);
        }

        private void HandleVisualFromPrediction(HpNumberChecker.Prediction prediction)
        {
            if (prediction == _currentPrediction)
                return;
            
            _currentPrediction = prediction;
            SetVisualsHereAndHow();
        }

        #endregion

        #region Smooth

        private void DamagedRectAlphaChange(bool visible, bool smooth)
        {
            if (_damagedImageFill == null)
                return;
            if (!smooth)
            {
                _damagedImageFill.color = new Color(
                    _damagedImageFill.color.r,
                    _damagedImageFill.color.g,
                    _damagedImageFill.color.b,
                    visible ? 1f : 0f);
                return;
            }

            float targetAlpha = visible ? 1f : 0f;
            if (!Mathf.Approximately(_damagedImageFill.color.a, targetAlpha))
            {
                Tween.Alpha(_damagedImageFill, 
                    targetAlpha, 
                    animationDuration, 
                    ease: Ease.OutQuad);
            }
        }
        
        private void AnimateBackgroundColor(Color color)
        {
            if (background.color == color)
                return;

            if (_bgColorChange.isAlive)
                _bgColorChange.Stop();
            
            _bgColorChange = Tween.Color(background, endValue: color, 
                duration: animationDuration, ease: Ease.OutQuad);
        }

        #endregion

        protected override void SetColors(State state)
        {
            if (!canSetVisuals)
                return;
            
            // god has abandoned me so i made this thing
            Color bgColor = (state, _currentPrediction) switch
            {
                (State.None, HpNumberChecker.Prediction.None) => normalBgColor,
                (State.None, HpNumberChecker.Prediction.Damage) => damageNormalBgColor,
                (State.None, HpNumberChecker.Prediction.Success) => successNormalBgColor,

                (State.Hover, HpNumberChecker.Prediction.None) => hoverBgColor,
                (State.Hover, HpNumberChecker.Prediction.Damage) => damageHoverBgColor,
                (State.Hover, HpNumberChecker.Prediction.Success) => successHoverBgColor,

                (State.Pressed, HpNumberChecker.Prediction.None) => pressedBgColor,
                (State.Pressed, HpNumberChecker.Prediction.Damage) => damagePressedBgColor,
                (State.Pressed, HpNumberChecker.Prediction.Success) => successPressedBgColor,

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

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnHpNumberViewSelected?.Invoke(_hpNumber);
        }

        private void OnDestroy()
        {
            SetUnactive();
        }
    }
}