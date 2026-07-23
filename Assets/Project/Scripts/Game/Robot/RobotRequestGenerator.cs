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

        public RobotRequestGenerator(RobotRequestSettings settings, 
            IHpNumbersService hpNumbersService)
        {
            _settings = settings;
            _hpNumbersService = hpNumbersService;
        }

        public void FillRequest(RobotRequest request)
        {
            int targetAmount = Random.Range(_settings.MinAmount, _settings.MaxAmount + 1);

            List<int> notPresentValues = GetNotPresentValues();
            List<int> presentValues = GetPresentValues();

            List<int> positiveNumbers = new();
            List<int> negativeNumbers = new();

            for (int i = 0; i < _settings.MinPositive && notPresentValues.Count > 0; i++)
            {
                int index = Random.Range(0, notPresentValues.Count);
                positiveNumbers.Add(notPresentValues[index]);
                notPresentValues.RemoveAt(index);
            }
    
            for (int i = 0; i < _settings.MinNegative && presentValues.Count > 0; i++)
            {
                int index = Random.Range(0, presentValues.Count);
                negativeNumbers.Add(presentValues[index]);
                presentValues.RemoveAt(index);
            }
            
            int currentCount = positiveNumbers.Count + negativeNumbers.Count;
            int remainingToFill = targetAmount - currentCount;
            
            for (int i = 0; i < remainingToFill; i++)
            {
                bool canTakePositive = notPresentValues.Count > 0;
                bool canTakeNegative = presentValues.Count > 0;

                if (!canTakePositive && !canTakeNegative) break;

                bool takePositive;
                if (canTakePositive && canTakeNegative)
                    takePositive = Random.value > 0.5f;
                else
                    takePositive = canTakePositive;

                if (takePositive)
                {
                    int index = Random.Range(0, notPresentValues.Count);
                    positiveNumbers.Add(notPresentValues[index]);
                    notPresentValues.RemoveAt(index);
                }
                else
                {
                    int index = Random.Range(0, presentValues.Count);
                    negativeNumbers.Add(presentValues[index]);
                    presentValues.RemoveAt(index);
                }
            }
    
            request.SetValues(positiveNumbers, negativeNumbers);
        }

        private List<int> GetPresentValues()
        {
            List<int> values = new();
            foreach (HpNumber hpNumber in _hpNumbersService.Numbers)
            {
                values.Add(hpNumber.CurrentValue);
            }
            return values;
        }

        private List<int> GetNotPresentValues()
        {
            List<int> freeNumbers = new() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            foreach (HpNumber hpNumber in _hpNumbersService.Numbers)
            {
                freeNumbers.Remove(hpNumber.CurrentValue);
            }
            return freeNumbers;
        }
    }
}