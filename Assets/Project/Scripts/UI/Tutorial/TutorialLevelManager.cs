using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Tutorial
{
    public class TutorialLevelManager : MonoBehaviour
    {
        private int _currentPage;
        [SerializeField] public List<TutorialPage> pageOrder;
        [SerializeField] public Image tutorialBackground;

        public event Action OnTutorialFinished;

        private void Awake()
        {
            foreach (TutorialPage page in pageOrder)
            {
                page.gameObject.SetActive(false);
            }
            if (tutorialBackground)
                tutorialBackground.gameObject.SetActive(false);
        }

        public void StartTutorial()
        {
            if (pageOrder == null || pageOrder.Count == 0)
            {
                Debug.LogWarning("[TutorialLevelManager] there's no pages in list!");
                return;
            }
            
            _currentPage = 0;
            if (tutorialBackground)
                tutorialBackground.gameObject.SetActive(true);
            StartCurrentPage();
        }

        private void StartCurrentPage()
        {
            pageOrder[_currentPage].OnPageClosed += HandleNextPage;
            pageOrder[_currentPage].gameObject.SetActive(true);
            pageOrder[_currentPage].StartPage();
        }
        
        private void HandleNextPage()
        {
            pageOrder[_currentPage].OnPageClosed -= HandleNextPage;
            pageOrder[_currentPage].gameObject.SetActive(false);
            
            _currentPage++;
            if (_currentPage > pageOrder.Count - 1)
            {
                if (tutorialBackground)
                    tutorialBackground.gameObject.SetActive(false);
                OnTutorialFinished?.Invoke();
                Debug.Log("[TutorialLevelManager] tutorial finished.");
                return;
            }
            StartCurrentPage();
        }

        public void OnDestroy()
        {
            if (_currentPage < pageOrder.Count - 1)
                pageOrder[_currentPage].OnPageClosed -= HandleNextPage;
        }
    }
}