using System.Collections.Generic;
using PrimeTween;
using Project.Core.Robot;
using UnityEngine;
using VContainer;

namespace Project.Scripts.UI
{
    public class RobotRequestView : MonoBehaviour
    {
        [SerializeField] private List<RobotRequestLetter> goodLetters;
        [SerializeField] private List<RobotRequestLetter> badLetters;
        
        [Header("Request appearance")] 
        [SerializeField] private Transform visibilityTransform;
        [SerializeField] private float appearanceDuration = 0.15f;
        [SerializeField] private float tiltRotation = 15f;
        
        private RobotRequest _request;
        private IRobotService _robotService;

        private bool _isActive = false;
        private Vector3 _originalScale;
        private Sequence _requestAppearance;

        private List<RobotRequestLetter> _visibleLetters = new();

        [Inject]
        public void Construct(IRobotService robotService)
        {
            _robotService = robotService;
            _request = robotService.Request;
            
            _robotService.OnRequestStarted += HandleRequestStarted;
            _robotService.OnRequestFinished += HandleRequestFinished;
            _request.OnRequestChanged += HandleRequestChanged;
        }
        
        private void Awake()
        {
            _originalScale = visibilityTransform.localScale;
            visibilityTransform.localScale = Vector3.zero;
            SetLettersVisibility(false);
        }

        private void HandleRequestStarted(RobotRequest request)
        {
            if (_isActive)
                return;
            
            _isActive = true;
            
            visibilityTransform.gameObject.SetActive(true);
            PlayAppearanceAnimation();
        }
        
        private void HandleRequestFinished(RobotRequestCheckResult result)
        {
            if (!_isActive)
                return;
            
            _isActive = false;
            PlayDisappearanceAnimation();
        }

        private void HandleAppearance(bool value)
        {
            if (value == _isActive)
                return;
            _isActive = value;
            
            if (_isActive)
            {
                visibilityTransform.gameObject.SetActive(true);
                PlayAppearanceAnimation();
            }
            else
            {
                PlayDisappearanceAnimation();
            }
        }

        private void HandleRequestChanged()
        {
            Clear();
            
            if (_request.GoodValues == null || _request.BadValues == null)
                return;
            
            for (int i = 0; i < _request.GoodValues.Count; i++)
            {
                if (i > goodLetters.Count)
                    break;
                
                _visibleLetters.Add(goodLetters[i]);
                goodLetters[i].SetLabel(_request.GoodValues[i]);
            }
            
            for (int i = 0; i < _request.BadValues.Count; i++)
            {
                if (i > badLetters.Count)
                    break;
                
                _visibleLetters.Add(badLetters[i]);
                badLetters[i].SetLabel(_request.BadValues[i]);
            }
        }

        private void Clear()
        {
            _visibleLetters.Clear();
        }

        #region Appear / Disappear
        
        private void PlayAppearanceAnimation()
        {
            // avoid errors when changing level
            if (this == null || visibilityTransform == null)
                return;
            
            if (_requestAppearance.isAlive)
                _requestAppearance.Stop();

            visibilityTransform.localScale = Vector3.zero;
            visibilityTransform.localRotation = Quaternion.Euler(0f, 0f, tiltRotation);
            
            SetLettersVisibility(true);

            _requestAppearance = Sequence.Create()
                .Group(Tween.Scale(
                    visibilityTransform,
                    endValue: _originalScale, 
                    duration: appearanceDuration, 
                    ease: Ease.OutBack))
                .Group(Tween.LocalRotation(
                    visibilityTransform, endValue: 
                    Quaternion.identity, duration: 
                    appearanceDuration, ease: 
                    Ease.OutBack));
        }

        private void PlayDisappearanceAnimation()
        {
            // avoid errors when changing level
            if (this == null || visibilityTransform == null)
                return;
            
            if (_requestAppearance.isAlive)
                _requestAppearance.Stop();
            
            SetLettersVisibility(false);

            _requestAppearance = Sequence.Create().Group(
                    Tween.Scale(visibilityTransform, 
                        endValue: Vector3.zero, 
                        duration: appearanceDuration, 
                        ease: Ease.InBack))
                .Group(Tween.LocalRotation(visibilityTransform, 
                    endValue: Quaternion.Euler(0f, 0f, tiltRotation), 
                    duration: appearanceDuration, 
                    ease: Ease.InBack))
                .OnComplete(() =>
                {
                    // check to see if it activated again
                    if (!_isActive)
                        visibilityTransform.gameObject.SetActive(false);
                });
        }

        private void SetLettersVisibility(bool visible)
        {
            if (!visible)
            {
                foreach (RobotRequestLetter letter in goodLetters)
                    letter.gameObject.SetActive(false);
                foreach (RobotRequestLetter letter in badLetters)
                    letter.gameObject.SetActive(false);
                return;
            }

            foreach (RobotRequestLetter letter in _visibleLetters)
            {
                letter.gameObject.SetActive(true);
            }
        }
        
        #endregion

        private void OnDestroy()
        {
            if (_request != null)
                _request.OnRequestChanged -= HandleRequestChanged;
            
            Clear();
            
            if (_requestAppearance.isAlive)
                _requestAppearance.Stop();
        }
    }
}