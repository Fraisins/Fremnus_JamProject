using System.Collections.Generic;
using Project.Core.Numbers;
using Project.Core.Robot;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    public class HpNumbersContainerView : MonoBehaviour
    {
        private IHpNumbersService _hpNumbersService;
        private IModMachineService _modMachineService;
        private IRobotService _robotService;

        [SerializeField] private List<HpNumberView> hpNumberViews = new();
        
        [Inject]
        public void Construct(IHpNumbersService hpNumbersService, IModMachineService modMachineService,
            IRobotService robotService)
        {
            _hpNumbersService = hpNumbersService ?? 
                throw new System.ArgumentNullException(nameof(hpNumbersService));
            _modMachineService = modMachineService ??
                throw new System.ArgumentNullException(nameof(modMachineService));
            _robotService = robotService ??
                throw new System.ArgumentNullException(nameof(robotService));
            
            _hpNumbersService.OnHpNumbersCreated += HandleHpNumbersCreated;
        }

        private void HandleHpNumbersCreated(IReadOnlyList<HpNumber> numbers)
        {
            Reset();

            if (numbers.Count > hpNumberViews.Count)
            {
                Debug.LogError("[HpNumbersContainerView] numbers.Count > hpNumberViews.Count");
                return;
            }
            
            for (int i = hpNumberViews.Count - 1; i >= 0; i--)
            {
                if (i <= numbers.Count - 1)
                {
                    hpNumberViews[i].gameObject.SetActive(true);
                    hpNumberViews[i].OnHpNumberViewSelected += HandleOnHpNumberViewSelected;
                    hpNumberViews[i].Init(numbers[i], _robotService);
                }
                else
                {
                    hpNumberViews[i].SetUnactive();
                    hpNumberViews[i].gameObject.SetActive(false);
                }
            }
        }

        private void Reset()
        {
            foreach (HpNumberView hpNumberView in hpNumberViews)
            {
                hpNumberView.SetUnactive();
                hpNumberView.gameObject.SetActive(false);
                hpNumberView.OnHpNumberViewSelected -= HandleOnHpNumberViewSelected;
            }
        }

        #region Selection
        
        private void HandleOnHpNumberViewSelected(HpNumber number)
        {
            _modMachineService.AddCurrentModificatorToHpNumber(number);
        }

        #endregion

        private void OnDestroy()
        {
            hpNumberViews.Clear();
            if (_hpNumbersService != null)
                _hpNumbersService.OnHpNumbersCreated -= HandleHpNumbersCreated;
        }
    }
}