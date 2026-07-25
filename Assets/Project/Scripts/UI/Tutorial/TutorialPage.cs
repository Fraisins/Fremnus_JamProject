using System;
using UnityEngine;

namespace Project.UI.Tutorial
{
    public class TutorialPage : MonoBehaviour
    {
        private bool _isActive;
        public event Action OnPageClosed;

        public void StartPage()
        {
            _isActive = true;
        }

        public void ClosePage()
        {
            if (!_isActive) return;
            
            OnPageClosed?.Invoke();
            _isActive = false;
        }
    }
}