using System.Collections.Generic;

namespace Project.Core.Robot
{
    public class RobotRequest
    {
        private List<int> _goodValues;
        private List<int> _badValues;
        
        public IReadOnlyList<int> GoodValues => _goodValues;
        public IReadOnlyList<int> BadValues => _badValues;

        public event System.Action OnRequestChanged;

        public void SetValues(List<int> goodValues, List<int> badValues)
        {
            if (goodValues == null)
                _goodValues = new List<int>();
            if (badValues == null)
                _badValues = new List<int>();
            
            _goodValues = goodValues;
            _badValues = badValues;
            
            OnRequestChanged?.Invoke();
        }
    }
}