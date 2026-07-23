using System;
using Project.Core.Numbers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class ModMachineModificatorView : CustomButton
    {
        private ModMachineModificator _modificator;
        public event Action<ModMachineModificator> OnSelected;

        public void Init(ModMachineModificator modificator)
        {
            _modificator = modificator ?? throw new ArgumentNullException(nameof(modificator));
            _modificator.OnModValueChanged += HandleModValueChanged;
            
            HandleModValueChanged(_modificator.Value);
        }

        private void HandleModValueChanged(int value)
        {
            if (label == null)
                return;
            
            label.text = ModHelper.ValueToString(value);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnSelected?.Invoke(_modificator);
        }

        public void Clear()
        {
            if (_modificator != null)
                _modificator.OnModValueChanged -= HandleModValueChanged;
        }

        public void OnDestroy()
        {
            Clear();
        }
    }
}