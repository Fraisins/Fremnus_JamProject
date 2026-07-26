using System.Collections.Generic;
using Project.Core.Numbers;
using Project.Core.Robot;
using UnityEngine;

namespace Project.Game.Robot
{
    public class RobotRequestGenerator
    {
        private readonly IHpNumbersService _hpNumbersService;
        private readonly RobotRequestSettings _settings;
        
        private readonly List<int> _notPresentValuesCache = new(10);
        private readonly List<int> _presentValuesCache = new(10);
        private readonly bool[] _presentFlagsCache = new bool[10];
        private readonly HashSet<int> _seenValuesCache = new();

        public RobotRequestGenerator(RobotRequestSettings settings, 
            IHpNumbersService hpNumbersService)
        {
            _settings = settings;
            _hpNumbersService = hpNumbersService;
        }

        public void FillRequest(RobotRequest request)
        {
            request.Clear();
            
            int targetAmount = Random.Range(_settings.MinAmount, _settings.MaxAmount + 1);

            GetNotPresentValues(_notPresentValuesCache);
            GetPresentValues(_presentValuesCache);

            int positiveCount = 0;
            int negativeCount = 0;

            for (int i = 0; i < _settings.MinPositive && _notPresentValuesCache.Count > 0; i++)
            {
                int index = Random.Range(0, _notPresentValuesCache.Count);
                request.AddGoodValue(_notPresentValuesCache[index]);
                _notPresentValuesCache.RemoveAt(index);
                positiveCount++;
            }
    
            for (int i = 0; i < _settings.MinNegative && _presentValuesCache.Count > 0; i++)
            {
                int index = Random.Range(0, _presentValuesCache.Count);
                request.AddBadValue(_presentValuesCache[index]);
                _presentValuesCache.RemoveAt(index);
                negativeCount++;
            }
            
            int currentCount = positiveCount + negativeCount;
            int remainingToFill = targetAmount - currentCount;
            
            for (int i = 0; i < remainingToFill; i++)
            {
                bool canTakePositive = _notPresentValuesCache.Count > 0 && positiveCount < _settings.MaxPositive;
                bool canTakeNegative = _presentValuesCache.Count > 0 && negativeCount < _settings.MaxNegative;

                if (!canTakePositive && !canTakeNegative) break;

                bool takePositive;
                if (canTakePositive && canTakeNegative)
                    takePositive = Random.value > 0.5f;
                else
                    takePositive = canTakePositive;

                if (takePositive)
                {
                    int index = Random.Range(0, _notPresentValuesCache.Count);
                    request.AddGoodValue(_notPresentValuesCache[index]);
                    _notPresentValuesCache.RemoveAt(index);
                    positiveCount++;
                }
                else
                {
                    int index = Random.Range(0, _presentValuesCache.Count);
                    request.AddBadValue(_presentValuesCache[index]);
                    _presentValuesCache.RemoveAt(index);
                    negativeCount++;
                }
            }
    
            request.RaiseRequestChanged();
        }

        private void GetPresentValues(List<int> resultBuffer, bool ignoreNotActive = true)
        {
            resultBuffer.Clear();
            _seenValuesCache.Clear();

            if (_hpNumbersService?.Numbers == null) return;

            foreach (HpNumber hpNumber in _hpNumbersService.Numbers)
            {
                if (ignoreNotActive && hpNumber.State != HpNumberState.Active)
                    continue;

                if (_seenValuesCache.Add(hpNumber.CurrentValue))
                {
                    resultBuffer.Add(hpNumber.CurrentValue);
                }
            }
        }

        private void GetNotPresentValues(List<int> resultBuffer)
        {
            resultBuffer.Clear();
            
            System.Array.Clear(_presentFlagsCache, 0, _presentFlagsCache.Length);
            if (_hpNumbersService?.Numbers != null)
            {
                foreach (HpNumber hpNumber in _hpNumbersService.Numbers)
                {
                    int val = hpNumber.CurrentValue;
                    if (val >= 0 && val <= 9)
                    {
                        _presentFlagsCache[val] = true;
                    }
                }
            }
            for (int i = 0; i <= 9; i++)
            {
                if (!_presentFlagsCache[i])
                    resultBuffer.Add(i);
            }
        }
    }
}