using System;
using System.Collections.Generic;
using Project.Core.Events;
using Project.Core.Numbers;
using UnityEngine;

namespace Project.Game.Numbers
{
    public class ModMachineService : IModMachineService, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly ModMachineGenerator _generator = new();
        
        private ModMachineModificator _currentModificator;
        private List<ModMachineModificator> _modificators;
        private readonly Dictionary<ModMachineModificator, HpNumber> _modDictionary = new();

        private int _currentMaxAmount;
        
        public IReadOnlyList<ModMachineModificator> Modificators => _modificators;
        public ModMachineModificator CurrentModificator => _currentModificator;
        
        public event Action<IReadOnlyList<ModMachineModificator>> OnModificatorsCreated;
        
        public event Action<ModMachineModificator> OnModificatorSelected;
        public event Action OnModificatorUnselected;

        #region Create / Reset
        
        public void CreateModificators(int amount)
        {
            _currentMaxAmount = amount;
            RecreateModificators();
        }

        public void RecreateModificators()
        {
            ForceReset();
            _generator.GenerateModificators(_currentMaxAmount, ref _modificators);
            
            foreach (ModMachineModificator modificator in _modificators)
            {
                _modDictionary.TryAdd(modificator, null);
                modificator.OnFreeState += HandleModificatorFreed;
            }
            
            OnModificatorsCreated?.Invoke(_modificators);
        }

        public void ForceReset()
        {
            if (_modificators == null) return;
            foreach (ModMachineModificator mod in _modificators)
            {
                if (_modDictionary.TryGetValue(mod, out HpNumber hpNumber))
                {
                    hpNumber?.ClearModificator();
                }

                mod.OnFreeState -= HandleModificatorFreed;
                mod.ForceReset();
                _modDictionary[mod] = null;
            }
            UnselectModificator();
        }
        
        #endregion

        #region ModificatorSelection

        public void SelectModificator(ModMachineModificator modificator)
        {
            if (_currentModificator == modificator)
                return;
            
            _currentModificator = modificator;
            OnModificatorSelected?.Invoke(modificator);
        }

        public void UnselectModificator()
        {
            _currentModificator = null;
            OnModificatorUnselected?.Invoke();
        }

        #endregion

        #region HpConnection

        public void AddCurrentModificatorToHpNumber(HpNumber hpNumber)
        {
            if (_currentModificator == null || hpNumber == null)
                return;

            if (!CanAttachModificator(hpNumber))
                return;

            if (hpNumber.ModMachineModificator == _currentModificator)
            {
                hpNumber.ClearModificator();
                return;
            }

            if (hpNumber.ModMachineModificator != null)
                hpNumber.ClearModificator();

            if (_modDictionary[_currentModificator] != null)
                _modDictionary[_currentModificator].ClearModificator();
            
            hpNumber.SetModMachineModificator(_currentModificator);
            _modDictionary[_currentModificator] = hpNumber;
        }

        private void HandleModificatorFreed(ModMachineModificator modificator)
        {
            if (!_modDictionary.TryGetValue(modificator, out HpNumber hpNumber) 
                || hpNumber == null)
                return;
            
            _modDictionary[modificator] = null;
        }
        
        #endregion

        public void Dispose()
        {
            if (_modificators == null)
                return;
                
            foreach (ModMachineModificator modificator in _modificators)
            {
                modificator.OnFreeState -= HandleModificatorFreed;
            }
            _modificators.Clear();
            _modDictionary.Clear();
        }

        #region Utility

        private static bool CanAttachModificator(HpNumber hpNumber)
        {
            return hpNumber.State == HpNumberState.Active;
        }

        #endregion
    }
}