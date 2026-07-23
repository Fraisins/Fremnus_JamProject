using System;

namespace Project.Core.Numbers
{
    public class ModMachineModificator
    {
        private int _value = 0;
        public int Value => _value;
        
        private bool _isTaken = false;
        public bool IsTaken => _isTaken;
        
        public event Action<int> OnModValueChanged;
        public event Action<ModMachineModificator> OnFreeState;
        public event Action<bool> OnTakenStateChanged;

        public void SetModValue(int value)
        {
            _value = value;
            OnModValueChanged?.Invoke(value);
        }

        public void SetTaken(bool isTaken)
        {
            if (isTaken == _isTaken)
                return;
            
            if (_isTaken && !isTaken)
                OnFreeState?.Invoke(this);
            
            _isTaken = isTaken;
            OnTakenStateChanged?.Invoke(isTaken);
        }

        public void ForceReset()
        {
            _value = 0;
        }
    }
}