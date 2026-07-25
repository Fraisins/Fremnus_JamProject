using System.Collections.Generic;
using Project.Core.Numbers;
using Project.Core.Robot;
using UnityEngine;

namespace Project.Game.Numbers
{
    public class ModMachineGenerator
    {
        private readonly IHpNumbersService _hpNumbersService;
        private readonly IRobotService _robotService;
        
        public ModMachineGenerator(IHpNumbersService hpNumbersService, IRobotService robotService)
        {
            _hpNumbersService = hpNumbersService;
            _robotService = robotService;
        }
        
        public void GenerateModificators(int amount, ref List<ModMachineModificator> modificators)
        {
            if (amount > 9 || amount < 1)
            {
                Debug.LogError($"[GenerateModificators] amount ({amount}) not in range (1 - 9)");
                return;
            }
            
            modificators ??= new List<ModMachineModificator>(amount);

            for (int i = 0; i < amount; i++)
            {
                ModMachineModificator mod;
                
                if (i >= modificators.Count)
                {
                    mod = new ModMachineModificator();
                    modificators.Add(mod);
                }
                else
                {
                    mod = modificators[i];
                    mod.ForceReset();
                }
            }
            
            List<int> helpNumbers = GetPossibleNeededValues();
            List<int> freeNumbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<ModMachineModificator> freeModificators = new(modificators);
            
            if (helpNumbers != null && helpNumbers.Count > 0)
            {
                foreach (int helpValue in helpNumbers)
                {
                    int randomIndex = Random.Range(0, freeModificators .Count);
                    ModMachineModificator randMod = freeModificators[randomIndex];
                    randMod.SetModValue(helpValue);
                    freeNumbers.Remove(Mathf.Abs(helpValue));
                
                    freeModificators.RemoveAt(randomIndex);
                }
            }
            
            if (modificators.Count > amount)
                modificators.RemoveRange(amount, modificators.Count - amount);

            while (freeModificators.Count > 0 && freeNumbers.Count > 0)
            {
                int randomModIndex = Random.Range(0, freeModificators .Count);
                ModMachineModificator randMod = freeModificators[randomModIndex];
                
                int randomIndex = Random.Range(0, freeNumbers.Count);
                int randomValue = freeNumbers[randomIndex];
                freeNumbers.RemoveAt(randomIndex);
                
                if (Random.value < 0.5f)
                    randomValue *= -1;
                
                randMod.SetModValue(randomValue);
                freeModificators.RemoveAt(randomModIndex);
            }
        }

        private List<int> GetPossibleNeededValues()
        {
            if (_robotService?.Request?.GoodValues == null)
                return null;
            
            int randIndex = -1;
            int value = 0;
            bool found = false;

            List<int> result = new();
            List<HpNumber> hpNumbers = new(_hpNumbersService.Numbers);
            HashSet<int> usedAbsValues = new();
            
            foreach (int goodVal in _robotService.Request.GoodValues)
            {
                if (hpNumbers.Count == 0)
                    break;

                List<int> shuffledIndices = new(hpNumbers.Count); // shuffle for random picking
                for (int i = 0; i < hpNumbers.Count; i++)
                    shuffledIndices.Add(i);

                for (int i = shuffledIndices.Count - 1; i > 0; i--)
                {
                    int j = Random.Range(0, i + 1);
                    (shuffledIndices[i], shuffledIndices[j]) = (shuffledIndices[j], shuffledIndices[i]);
                }

                foreach (int idx in shuffledIndices)
                {
                    HpNumber candidate = hpNumbers[idx];
                    int minValue = Mathf.Min(goodVal, candidate.RawNumber);
                    int maxValue = Mathf.Max(goodVal, candidate.RawNumber);

                    if (maxValue == minValue) // nope, it's 0
                        continue;

                    int val1 = maxValue - minValue;
                    int val2 = 10 + minValue - maxValue;
                    
                    bool pickFirst = Random.value < 0.5f;
                    int firstPossibility = pickFirst ? val1 : val2;
                    int secondPossibility = pickFirst ? val2 : val1;

                    if (!usedAbsValues.Contains(Mathf.Abs(firstPossibility))) // check first
                    {
                        value = firstPossibility;
                        randIndex = idx;
                        found = true;
                        break;
                    }
                    if (!usedAbsValues.Contains(Mathf.Abs(secondPossibility))) // check second
                    {
                        value = secondPossibility;
                        randIndex = idx;
                        found = true;
                        break;
                    }
                }
                
                if (!found)
                    continue;

                result.Add(value);
                usedAbsValues.Add(Mathf.Abs(value));
                hpNumbers.RemoveAt(randIndex);
            }
            
            return result;
        }
    }
}