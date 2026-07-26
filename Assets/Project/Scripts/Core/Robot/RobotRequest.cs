using System.Collections.Generic;

namespace Project.Core.Robot
{
    public class RobotRequest
    {
        private List<int> _goodValues = new();
        private List<int> _badValues = new();
        
        public IReadOnlyList<int> GoodValues => _goodValues;
        public IReadOnlyList<int> BadValues => _badValues;

        public event System.Action OnRequestChanged;
        
        public void Clear()
        {
            _goodValues.Clear();
            _badValues.Clear();
        }

        public void AddGoodValue(int value) => _goodValues.Add(value);
        public void AddBadValue(int value) => _badValues.Add(value);
        
        public void RaiseRequestChanged()
        {
            OnRequestChanged?.Invoke();
        }

        // public void SetValues(List<int> goodValues, List<int> badValues)
        // {
        //     if (goodValues == null)
        //         _goodValues = new List<int>();
        //     if (badValues == null)
        //         _badValues = new List<int>();
        //     
        //     _goodValues = goodValues;
        //     _badValues = badValues;
        //     
        //     OnRequestChanged?.Invoke();
        // }
    }
}