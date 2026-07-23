using System.Collections.Generic;
using Project.Core.Robot;
using UnityEngine;
using VContainer;

namespace Project.Scripts.UI
{
    public class RobotRequestView : MonoBehaviour
    {
        [SerializeField] private Transform visibilityTransform;
        [SerializeField] private List<RobotRequestLetter> goodLetters;
        [SerializeField] private List<RobotRequestLetter> badLetters;
        
        private RobotRequest _request;
        private RobotRequestTimer _timer;

        [Inject]
        public void Construct(IRobotService robotService)
        {
            _request = robotService.Request;
            _timer = robotService.Timer;
            
            _timer.OnIsRunningChanged += HandleVisibility;
            _request.OnRequestChanged += HandleRequestChanged;
            HandleRequestChanged();
            HandleVisibility(_timer.IsRunning);
        }

        private void HandleVisibility(bool value)
        {
            if (visibilityTransform != null)
                visibilityTransform.gameObject.SetActive(value);
        }

        private void HandleRequestChanged()
        {
            Clear();
            
            if (_request.GoodValues == null || _request.BadValues == null)
                return;
            
            for (int i = 0; i < goodLetters.Count; i++)
            {
                if (_request.GoodValues.Count > i)
                {
                    goodLetters[i].SetLabel(_request.GoodValues[i]);
                    goodLetters[i].gameObject.SetActive(true);
                }
                else
                {
                    goodLetters[i].gameObject.SetActive(false);
                }
            }
            
            for (int i = 0; i < badLetters.Count; i++)
            {
                if (_request.BadValues.Count > i)
                {
                    badLetters[i].SetLabel(_request.BadValues[i]);
                    badLetters[i].gameObject.SetActive(true);
                }
                else
                {
                    badLetters[i].gameObject.SetActive(false);
                }
            }
        }

        private void Clear()
        {
            foreach (RobotRequestLetter letter in goodLetters)
                letter.gameObject.SetActive(false);
            foreach (RobotRequestLetter letter in badLetters)
                letter.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_request != null)
                _request.OnRequestChanged -= HandleRequestChanged;
        }
    }
}