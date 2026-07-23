using System.Collections.Generic;
using Project.Core.Numbers;
using UnityEngine;

namespace Project.Game.Numbers
{
    public class ModMachineGenerator
    {
        public void GenerateModificators(int amount, ref List<ModMachineModificator> modificators)
        {
            if (amount > 9 || amount < 1)
            {
                Debug.LogError($"[GenerateModificators] amount ({amount}) not in range (1 - 9)");
                return;
            }
            
            modificators ??= new List<ModMachineModificator>(amount);
            
            List<int> freeNumbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            
            if (modificators.Count > amount)
                modificators.RemoveRange(amount, modificators.Count - amount);
            
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
                
                int randomIndex = Random.Range(0, freeNumbers.Count);
                int randomValue = freeNumbers[randomIndex];
                freeNumbers.RemoveAt(randomIndex);
                
                if (Random.value < 0.5f)
                    randomValue *= -1;
                
                mod.SetModValue(randomValue);
            }
        }
    }
}