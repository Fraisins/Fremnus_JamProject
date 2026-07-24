using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Robot
{
    public class RobotOrder
    {
        public int CurrentIndex { get; private set; } = -1;
        
        private List<RobotState> _robotsStates = new();
        public IReadOnlyList<RobotState> RobotsStates => _robotsStates;
        
        public event Action OnRobotOrderChanged;
        public event Action OnRobotOrderCreated;

        public void Recreate(int amount)
        {
            _robotsStates.Clear();
            for (int i = 0; i < amount; i++)
                _robotsStates.Add(RobotState.NotStarted);
            
            CurrentIndex = -1;
            OnRobotOrderCreated?.Invoke();
        }

        public void SuccessCurrentRobot() => SetCurrentRobotState(RobotState.Success);
        public void FailCurrentRobot() => SetCurrentRobotState(RobotState.Failed);
        
        public void NextRobot()
        {
            CurrentIndex++;
            if (CurrentIndex >= _robotsStates.Count)
                return;
            SetCurrentRobotState(RobotState.Active);
        }
        
        private void SetCurrentRobotState(RobotState state)
        {
            if (CurrentIndex < 0 || CurrentIndex > _robotsStates.Count - 1)
                return;
            
            _robotsStates[CurrentIndex] = state;
            OnRobotOrderChanged?.Invoke();
        }

        public void Clear()
        {
            _robotsStates.Clear();
        }
        
        public int CountSuccessRobots()
        {
            int count = 0;
            foreach (RobotState robotState in _robotsStates)
            {
                if (robotState == RobotState.Success)
                    count++;
            }
            return count;
        }
    }
}