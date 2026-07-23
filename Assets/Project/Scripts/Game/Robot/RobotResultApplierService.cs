using Project.Core.Numbers;
using Project.Core.Robot;

namespace Project.Game.Robot
{
    public class RobotResultApplierService
    {
        IHpNumbersService _hpNumbersService;
        
        public RobotResultApplierService(IHpNumbersService hpNumbersService)
        {
            _hpNumbersService = hpNumbersService;
        }
        
        public void Punish(RobotRequestCheckResult result)
        {
            foreach (HpNumber number in _hpNumbersService.Numbers)
            {
                number.SetRawNumber(number.CurrentValue);
                number.ClearModificator();
            }
            
            foreach (HpNumber number in result.HpNumbersToDamage)
            {
                _hpNumbersService.DamageNumber(number);
            }
        }
    }
}