using System;

namespace Project.Core.Numbers
{
    public class ModMachineModificator
    {
        public int Value { get; private set; } = 0;
        public bool IsTaken { get; private set; } = false;

        public event Action<int> OnModValueChanged;
        public event Action<ModMachineModificator> OnFreeState;
        public event Action<bool> OnTakenStateChanged;

        public void SetModValue(int value)
        {
            Value = value;
            OnModValueChanged?.Invoke(value);
        }

        public void SetTaken(bool isTaken)
        {
            if (isTaken == IsTaken)
                return;
            
            if (IsTaken && !isTaken)
                OnFreeState?.Invoke(this);
            
            IsTaken = isTaken;
            OnTakenStateChanged?.Invoke(isTaken);
        }

        public void ForceReset()
        {
            Value = 0;
        }
    }
}