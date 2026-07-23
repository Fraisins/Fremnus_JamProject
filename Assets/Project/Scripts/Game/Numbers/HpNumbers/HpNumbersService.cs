using System;
using System.Collections.Generic;
using Project.Core.Events;
using Project.Core.Numbers;
using UnityEngine;

namespace Project.Game.Numbers
{
    public class HpNumbersService : IHpNumbersService, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly HpNumbersGenerator _generator = new();
        
        private GameSettingsSO.HpNumbers _settings;
        
        private List<HpNumber> _numbers = new();
        
        public IReadOnlyList<HpNumber> Numbers => _numbers;
        public event Action<IReadOnlyList<HpNumber>> OnHpNumbersCreated;
        
        public HpNumbersService(GameSettingsSO settingsSo, IEventBus eventBus)
        {
            _settings = settingsSo.hpNumbers;
            _eventBus = eventBus;
        }

        #region Numbers list

        public void CreateNumbers(int amount)
        {
            _numbers = _generator.CreateNumbers(amount);
            OnHpNumbersCreated?.Invoke(_numbers);
        }
        
        #endregion
        
        #region Search

        public IReadOnlyList<HpNumber> GetNumbersWithValue(int value)
        {
            if (_numbers == null || _numbers.Count == 0)
                return null;
            
            List<HpNumber> result = new();
            foreach (HpNumber number in _numbers)
            {
                if (number.CurrentValue == value && number.State == HpNumberState.Active)
                    result.Add(number);
            }
            
            return result;
        }

        #endregion

        #region Damage
        
        public void DamageNumber(HpNumber number)
        {
            number.DamageNumber(_settings.damageTimer);
            TryTriggerDeath();
        }

        private void TryTriggerDeath()
        {
            foreach (HpNumber number in _numbers)
            {
                if (number.State != HpNumberState.Damaged)
                    return;
            }
            _eventBus.Publish<GameOverEvent>(default);
        }
        #endregion

        #region Clear

        public void Clear()
        {
            foreach (HpNumber number in _numbers)
                number.Dispose();
            
            _numbers.Clear();
        }

        #endregion

        public void Dispose()
        {
            Clear();
        }
    }
}