using System.Collections.Generic;
using Project.Core.Numbers;
using UnityEngine;

namespace Project.Game.Numbers
{
    public class HpNumbersGenerator
    {
        public List<HpNumber> CreateNumbers(int amount)
        {
            if (amount > 10)
            {
                Debug.LogError($"[HpNumbersGenerator] amount ({amount}) exceeds max unique values (10)");
                amount = 10;
            }
            
            List<int> freeNumbers = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // some hardcoded thing

            List<HpNumber> numbers = new(amount);
            
            HpNumber previousNumber = null;

            for (int i = 0; i < amount; i++)
            {
                HpNumber number = new();
                
                int randomIndex = Random.Range(0, freeNumbers.Count);
                int randomNumber = freeNumbers[randomIndex];
                freeNumbers.RemoveAt(randomIndex);
                
                numbers.Add(number);
                number.SetRawNumber(randomNumber);
                number.SetState(HpNumberState.Active);

                previousNumber?.SetPreviousNumber(number);
                previousNumber = number;
            }
            return numbers;
        }
    }
}