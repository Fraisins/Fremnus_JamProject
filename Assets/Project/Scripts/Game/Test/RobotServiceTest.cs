using Project.Core.Events;
using Project.Core.Numbers;
using Project.Core.Robot;
using UnityEngine;
using VContainer;

namespace Project.Game.Test
{
    public class RobotServiceTest : MonoBehaviour
    {
        private IEventBus _eventBus;
        private IRobotService _robotService;
        private IHpNumbersService _hpNumberService;
        private IModMachineService _modMachineService;
        private GameService _gameService;
        
        [Inject]
        public void Construct(
            IEventBus eventBus,
            IRobotService robotService,
            IHpNumbersService hpNumbersService,
            IModMachineService modMachineService,
            GameService gameService)
        {
            _eventBus = eventBus;
            _hpNumberService = hpNumbersService;
            _robotService = robotService;
            _modMachineService = modMachineService;
            _gameService = gameService;
        }

        private void Start()
        {
            _gameService.InitScene();
                
            //_eventBus.Publish(new GameOverEvent());
            // _hpNumberService.CreateNumbers(4);
            // string str = "HP numbers: ";
            //
            // foreach (HpNumber number in _hpNumberService.Numbers)
            //     str += $"{number.CurrentValue} ";
            // Debug.Log(str);
            //
            // _hpNumberService.DamageNumber(_hpNumberService.Numbers[0]);
            //
            // _modMachineService.CreateModificators(4);
            // string str_2 = "ModMachineModificators: ";
            //
            // foreach (ModMachineModificator modificator in _modMachineService.Modificators)
            //     str_2 += $"{modificator.Value} ";
            // Debug.Log(str_2);
            //
            // _robotService.StartRobotLoop().Forget();
        }
        
    }
}