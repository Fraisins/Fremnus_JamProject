using System.Collections.Generic;
using Project.Core.Robot;
using UnityEngine;
using VContainer;

namespace Project.Scripts.UI
{
    public class RobotOrderView : MonoBehaviour
    {
        private IRobotService _robotService;
        
        [SerializeField] private Transform lightContainer;
        [SerializeField] private RobotLightView lightViewPrefab;
        
        private readonly List<RobotLightView> _lightViews = new();
        private RobotOrder _robotOrder;

        [Inject]
        public void Construct(IRobotService robotService)
        {
            _robotService = robotService;
            _robotService.OnInitialized += CreateRobotOrder;
        }

        private void CreateRobotOrder()
        {
            _robotOrder = _robotService.Order;
            
            _robotOrder.OnRobotOrderChanged += HandleRobotOrderChanged;
            _robotOrder.OnRobotOrderCreated += SpawnLights;
        }

        private void SpawnLights()
        {
            ClearOldLights();
            
            int amount = _robotOrder.RobotsStates.Count;
            for (int i = 0; i < amount; i++)
            {
                RobotLightView lightView = Instantiate(lightViewPrefab, lightContainer);
                _lightViews.Add(lightView);
            }
        }

        private void ClearOldLights()
        {
            for (int i = _lightViews.Count - 1; i >= 0 ; i--)
            {
                Destroy(_lightViews[i].gameObject);
            }
            _lightViews.Clear();
        }
        private void HandleRobotOrderChanged()
        {
            for (int i = 0; i < _lightViews.Count; i++)
            {
                RobotState state = _robotOrder.RobotsStates[i];
                _lightViews[i].SetState(state);
            }
        }

        private void OnDestroy()
        {
            if (_robotService != null)
                _robotService.OnInitialized -= CreateRobotOrder;
        }
    }
}