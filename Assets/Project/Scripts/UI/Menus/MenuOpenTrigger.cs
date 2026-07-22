using System;
using UnityEngine;

namespace Project.UI
{
    [RequireComponent(typeof(CustomButton))]
    public class MenuOpenTrigger : MonoBehaviour // I'm just lazy to make a child of button itself
    {
        [SerializeField] private SubmenuType submenuToOpen;
        
        public SubmenuType SubmenuToOpen => submenuToOpen;
        public CustomButton Button { get; private set; }
        
        public event Action<SubmenuType> OnTrigger;

        private void Awake()
        {
            Button = GetComponent<CustomButton>();
            Button.onPressed.AddListener(Trigger);
        }
        
        private void Trigger() => OnTrigger?.Invoke(submenuToOpen);

        private void OnDestroy()
        {
            if (Button != null)
                Button.onPressed.RemoveListener(Trigger);
        }
    }
}