using System.Collections.Generic;
using Project.Core.Numbers;
using UnityEngine;
using VContainer;

namespace Project.UI
{
    public class ModMachineContainerView : MonoBehaviour
    {
        private IModMachineService _modMachineService;

        [SerializeField] private  List<ModMachineModificatorView> modificatorViews = new();
        
        [Inject]
        public void Construct(IModMachineService modMachineService)
        {
            _modMachineService = modMachineService ?? 
                throw new System.ArgumentNullException(nameof(modMachineService));
            
            _modMachineService.OnModificatorsCreated += HandleModificatorsCreated;
        }

        private void HandleModificatorsCreated(IReadOnlyList<ModMachineModificator> mods)
        {
            ForceReset();

            for (int i = 0; i < modificatorViews.Count; i++)
            {
                if (i <= mods.Count - 1)
                {
                    modificatorViews[i].gameObject.SetActive(true);
                    modificatorViews[i].OnSelected += HandleModificatorSelected;
                    modificatorViews[i].Init(mods[i]);
                }
                else
                {
                    modificatorViews[i].Clear();
                    modificatorViews[i].gameObject.SetActive(false);
                }
            }
        }

        private void ForceReset()
        {
            foreach (ModMachineModificatorView view in modificatorViews)
            {
                view.OnSelected -= HandleModificatorSelected;
                view.Clear();
                view.gameObject.SetActive(false);
            }
        }

        #region Selection
        
        private void HandleModificatorSelected(ModMachineModificator mod)
        {
            _modMachineService.SelectModificator(mod);
        }
        
        #endregion

        private void OnDestroy()
        {
            ForceReset();
            modificatorViews.Clear();
            
            if (_modMachineService != null)
                _modMachineService.OnModificatorsCreated -= HandleModificatorsCreated;
        }
    }
}