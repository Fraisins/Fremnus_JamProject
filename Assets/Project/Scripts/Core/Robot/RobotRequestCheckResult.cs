using System.Collections.Generic;
using Project.Core.Numbers;

namespace Project.Core.Robot
{
    public class RobotRequestCheckResult
    {
        private List<HpNumber> _hpNumbersToDamage;
        
        public bool FinishedPositive { get; private set; }
        public IReadOnlyList<HpNumber> HpNumbersToDamage => _hpNumbersToDamage;

        public RobotRequestCheckResult(bool finishedPositive, List<HpNumber> hpNumbersToDamage)
        {
            FinishedPositive = finishedPositive;
            _hpNumbersToDamage = hpNumbersToDamage;
        }
        
    }
}