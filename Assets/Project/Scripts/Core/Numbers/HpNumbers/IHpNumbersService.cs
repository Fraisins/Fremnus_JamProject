using System;
using System.Collections.Generic;

namespace Project.Core.Numbers
{
    public interface IHpNumbersService
    {
        IReadOnlyList<HpNumber> Numbers { get; }
        
        void CreateNumbers(int amount);
        void Clear();
        void DamageNumber(HpNumber number);
        
        event Action<IReadOnlyList<HpNumber>> OnHpNumbersCreated;
    }
}