using System.Collections.Generic;
using System.Linq;
using Project.Core.Numbers;
using Project.Core.Robot;

namespace Project.Game.Robot
{
    public class RobotRequestChecker
    {
        private readonly IHpNumbersService _hpNumbersService;

        public RobotRequestChecker(IHpNumbersService hpNumbersService)
        {
            _hpNumbersService = hpNumbersService;
        }

        public RobotRequestCheckResult CheckRequest(RobotRequest request)
        {
            List<int> unfinishedPositiveNumbers = new(request.GoodValues);
            List<HpNumber> hitNegativeNumbers = new();
            
            foreach (HpNumber number in _hpNumbersService.Numbers)
            {
                if (number.State != HpNumberState.Active)
                    continue;
                
                if (unfinishedPositiveNumbers.Contains(number.CurrentValue))
                    unfinishedPositiveNumbers.Remove(number.CurrentValue);
                
                else if (request.BadValues.Contains(number.CurrentValue))
                    hitNegativeNumbers.Add(number);
            }
            
            return new RobotRequestCheckResult(unfinishedPositiveNumbers.Count == 0, hitNegativeNumbers);
        }
        
    }
}