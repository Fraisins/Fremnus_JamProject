using System;
using System.Collections.Generic;

namespace Project.Core.Numbers
{
    public interface IModMachineService
    {
        public IReadOnlyList<ModMachineModificator> Modificators { get; }
        public ModMachineModificator CurrentModificator { get; }

        public event Action<IReadOnlyList<ModMachineModificator>> OnModificatorsCreated;
        
        public event Action<ModMachineModificator> OnModificatorSelected;
        public event Action OnModificatorUnselected;
        
        void CreateModificators(int amount);
        void RecreateModificators();
        void ForceReset();
        
        void SelectModificator(ModMachineModificator modificator);
        void UnselectModificator();
        
        void AddCurrentModificatorToHpNumber(HpNumber hpNumber);
    }
}