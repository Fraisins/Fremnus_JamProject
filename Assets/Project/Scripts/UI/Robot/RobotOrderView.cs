using System.Collections.Generic;
using Project.Core.Robot;
using Project.Sound;
using UnityEngine;
using VContainer;

namespace Project.Scripts.UI
{
    public class RobotOrderView : MonoBehaviour
    {
        private IRobotService _robotService;
        private ISoundService _soundService;
        
        [SerializeField] private Transform lightContainer;
        [SerializeField] private RobotLightView lightViewPrefab;
        
        [Header("Colors")]
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private Color unactiveColor = Color.black;
        [SerializeField] private Color failedColor = Color.red;
        [SerializeField] private Color successColor = Color.green;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip successLightSound;
        [SerializeField] private AudioClip failLightSound;
        
        private readonly List<RobotLightView> _lightViews = new();
        private RobotOrder _robotOrder;

        [Inject]
        public void Construct(IRobotService robotService, ISoundService soundService)
        {
            _robotService = robotService;
            _soundService = soundService;
            
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
                lightView.OnStateChanged += HandleStateChanged;
                _lightViews.Add(lightView);
            }
        }

        private void ClearOldLights()
        {
            for (int i = _lightViews.Count - 1; i >= 0 ; i--)
            {
                if (_lightViews[i] == null)
                    continue;
            
                _lightViews[i].OnStateChanged -= HandleStateChanged;
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

        private void HandleStateChanged(RobotLightView view, RobotState state)
        {
            Color color = state switch
            {
                RobotState.NotStarted => unactiveColor,
                RobotState.Active => activeColor,
                RobotState.Success => successColor,
                RobotState.Failed => failedColor,
                _ => unactiveColor
            };
            
            view.SetColor(color);

            if (_soundService == null)
            {
                Debug.LogWarning("[RobotOrderView] SoundService is null!");
                return;
            }

            if (state == RobotState.Success && successLightSound)
                _soundService.PlaySfx(successLightSound);
            else if (state == RobotState.Failed && failLightSound)
                _soundService.PlaySfx(failLightSound);

        }

        private void OnDestroy()
        {
            ClearOldLights();
            
            if (_robotService != null)
                _robotService.OnInitialized -= CreateRobotOrder;
            
            if (_robotOrder != null)
            {
                _robotOrder.OnRobotOrderChanged -= HandleRobotOrderChanged;
                _robotOrder.OnRobotOrderCreated -= SpawnLights;
            }
        }
    }
}