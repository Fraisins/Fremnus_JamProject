using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace Project.UI
{
    public class LoadScreenView : MonoBehaviour
    {
        [SerializeField] private GameObject spinner;
        [SerializeField] private TMP_Text loadScreenText;
        
        [Header("Animation Settings")]
        [SerializeField] private float rotationSpeed = 360f;
        [SerializeField] private float pauseDuration = 0.25f;

        [Header("Text Settings")] 
        [SerializeField] private string baseTitle = "Loading";
        [SerializeField] private string animatedAddition = "...";
        [SerializeField] private float textAnimationDelay = 0.25f;
        
        private Sequence _spinnerAnimation;
        private CancellationTokenSource _textAnimCts;
        
        public void StartAnimation()
        {
            StartSpinnerAnimation();
            StartTextAnimation();
        }

        public void StopAnimation()
        {
            StopSpinnerAnimation();
            StopTextAnimation();
        }

        #region Spinner animation

        private void StartSpinnerAnimation()
        {
            StopSpinnerAnimation();
            
            float duration = 360f / rotationSpeed;
            Quaternion startRotation = spinner.transform.localRotation;
            
            _spinnerAnimation = Sequence.Create(cycles: -1, useUnscaledTime: true)
                .Chain(Tween.Custom(
                    startValue: 0f,
                    endValue: 360f,
                    duration: duration,
                    onValueChange: angle => spinner.transform.localRotation = 
                        startRotation * Quaternion.Euler(0f, 0f, -angle),
                    ease: Ease.InOutQuad))
                .ChainDelay(pauseDuration);
        }

        private void StopSpinnerAnimation()
        {
            if (_spinnerAnimation.isAlive)
                _spinnerAnimation.Stop();
        }

        #endregion

        #region Text animation

        private void StartTextAnimation()
        {
            StopTextAnimation();
            _textAnimCts = new CancellationTokenSource();
            TextAnimationLoop(_textAnimCts.Token).Forget();
        }

        private async UniTaskVoid TextAnimationLoop(CancellationToken token)
        {
            int count = 0;
            loadScreenText.text = baseTitle;

            while (!token.IsCancellationRequested)
            {
                loadScreenText.text = baseTitle + animatedAddition[..count];
                count = (count + 1) % (animatedAddition.Length + 1);
                
                await UniTask.Delay(TimeSpan.FromSeconds(textAnimationDelay), cancellationToken: token, 
                        ignoreTimeScale: true)
                    .SuppressCancellationThrow();
            }
        }

        private void StopTextAnimation()
        {
            _textAnimCts?.Cancel();
            _textAnimCts?.Dispose();
            _textAnimCts = null;
        }

        #endregion

        private void OnDestroy()
        {
            StopAnimation();
            StopTextAnimation();
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (loadScreenText != null)
            {
                loadScreenText.text = baseTitle + animatedAddition;
            }
        }
        #endif
    }
}