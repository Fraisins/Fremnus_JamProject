using System.Linq;
using Project.Core.Robot;

namespace Project.UI
{
    public static class HpNumberChecker
    {
        public enum Prediction
        {
            None,
            Success,
            Damage
        }

        public static Prediction PredictNumberResult(int value, RobotRequest request)
        {
            if (request.GoodValues.Contains(value))
                return Prediction.Success;
            if (request.BadValues.Contains(value))
                return Prediction.Damage;
            return Prediction.None;
        }
    }
}