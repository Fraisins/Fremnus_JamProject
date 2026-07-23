using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Core.Numbers
{
    public class HpNumber
    {
        private HpNumber _previousNumber = null;
        
        private HpNumberState _state = HpNumberState.Idle;
        
        private int _rawNumber = 0; // real current value
        private int _providedModificator = 0; // provided bonus
        private ModMachineModificator _modMachineModificator = null;
        
        private float _restorationElapsedTime = 0f;
        private float _restorationTime = 0f;
        private CancellationTokenSource _restorationTokenSource;
        
        public HpNumber PreviousNumber => _previousNumber;
        
        public int RawNumber => _rawNumber;
        public ModMachineModificator ModMachineModificator => _modMachineModificator;
        public int ProvidedModificator => _providedModificator;
        public int FullNumber => _rawNumber + GetModMachineModificatorValue() + GetLogicalModificator();
        public int CurrentValue => (FullNumber % 10 + 10) % 10;
        
        public float RestorationElapsedTime => _restorationElapsedTime;
        public float RestorationTime => _restorationTime;
        
        public HpNumberState State => _state;

        public event System.Action<int> OnNumberChanged;
        public event System.Action<HpNumberState> OnStateChanged;
        public event System.Action<int> OnProvidedModificatorValueChanged;
        public event System.Action<int> OnModMachineModificatorValueChanged;
        private event System.Action<HpNumber> OnProvidedModificatorChanged;
        
        #region Number

        public void SetRawNumber(int number)
        {
            if (number > 9 || number < 0)
            {
                Debug.LogError($"[HpNumber] is out of range 0 - 9. Number: {number}");
            }
            
            _rawNumber = number;
            CalculateProvidedModificator();
            OnNumberChanged?.Invoke(CurrentValue);
        }
        
        #endregion

        #region Modificators

        public void SetModMachineModificator(ModMachineModificator mod)
        {
            if (_modMachineModificator == mod || mod == null) 
                return;
            
            _modMachineModificator = mod;
            _modMachineModificator.SetTaken(true);
            
            CalculateProvidedModificator();
            OnNumberChanged?.Invoke(CurrentValue);
            OnModMachineModificatorValueChanged?.Invoke(mod.Value);
        }

        public void ClearModificator()
        {
            if (_modMachineModificator == null)
                return;
            _modMachineModificator.SetTaken(false);
            _modMachineModificator = null;
            
            CalculateProvidedModificator();
            OnModMachineModificatorValueChanged?.Invoke(0);
            OnNumberChanged?.Invoke(CurrentValue);
        }

        public int GetModMachineModificatorValue()
        {
            if (_modMachineModificator == null)
                return 0;
            return _modMachineModificator.Value;
        }

        public int GetLogicalModificator()
        {
            if (_previousNumber == null || _previousNumber.State != HpNumberState.Active)
                return 0;
            return _previousNumber.ProvidedModificator;
        }

        private void CalculateProvidedModificator()
        {
            int modificator = FullNumber switch
            {
                > 9 => 1,
                < 0 => -1,
                _ => 0
            };

            if (_providedModificator == modificator) return;
            
            _providedModificator = modificator;
            OnProvidedModificatorValueChanged?.Invoke(_providedModificator);
            OnProvidedModificatorChanged?.Invoke(this);
        }

        private void HandleProvidedModificatorChanged(HpNumber provider)
        {
            if (provider.State != HpNumberState.Active) return;
            OnNumberChanged?.Invoke(CurrentValue);
        }
        
        #endregion

        #region Next / Previous

        public void SetPreviousNumber(HpNumber previousNumber)
        {
            if (_previousNumber != null)
                _previousNumber.OnProvidedModificatorChanged -= HandleProvidedModificatorChanged;
            
            _previousNumber = previousNumber;
            if (previousNumber != null)
                _previousNumber.OnProvidedModificatorChanged += HandleProvidedModificatorChanged;
        }
        
        #endregion

        #region Damaged

        public void DamageNumber(float timeToRestore)
        {
            _restorationTime = timeToRestore;
            _restorationElapsedTime = 0f;
            SetState(HpNumberState.Damaged);
            _restorationTokenSource = new CancellationTokenSource();
            DamageLoop(_restorationTokenSource.Token).Forget();
        }

        private async UniTaskVoid DamageLoop(CancellationToken token)
        {
            while (_restorationElapsedTime < _restorationTime)
            {
                if (token.IsCancellationRequested)
                {
                    SetState(HpNumberState.Idle);
                    return;
                }
                
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken : token);
                _restorationElapsedTime += Time.deltaTime;
            }
            SetState(HpNumberState.Active);
        }

        private void CancelRestoration()
        {
            _restorationTokenSource?.Cancel();
            _restorationTokenSource?.Dispose();
            _restorationTokenSource = null;
        }

        #endregion

        public void SetState(HpNumberState state)
        {
            if (state == _state) return;
            
            _state = state;
            if (_state != HpNumberState.Active)
                ClearModificator();
            
            OnStateChanged?.Invoke(state);
        }
        
        public void Dispose()
        {
            _state = HpNumberState.Idle;
            if (_previousNumber != null)
                _previousNumber.OnProvidedModificatorChanged -= HandleProvidedModificatorChanged;
            _previousNumber = null;
            _modMachineModificator = null;
            
            CancelRestoration();
        }
    }
}