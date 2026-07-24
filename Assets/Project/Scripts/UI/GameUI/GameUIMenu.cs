using System;
using PrimeTween;
using UnityEngine;

namespace Project.UI
{
    public class GameUIMenu : MonoBehaviour
    {
        [Header("Appearance animation")]
        
        [SerializeField] protected float appearDuration = 0.25f;
        [SerializeField] protected float disappearDuration = 0.10f;
        
        private Tween _appearanceTween;
        private float _originalYScale;

        private void Awake()
        {
            _originalYScale = transform.localScale.y;
        }

        public virtual Tween PlayAppear()
        {
            if (_appearanceTween.isAlive)
                _appearanceTween.Stop();

            transform.localScale = new Vector3(
                transform.localScale.x,
                0f,
                transform.localScale.z);
            
            _appearanceTween = Tween.ScaleY(
                transform, 
                _originalYScale, 
                appearDuration,
                ease : Ease.OutBack,
                useUnscaledTime: true);
            
            return _appearanceTween;
        }

        public virtual Tween PlayHide()
        {
            if (_appearanceTween.isAlive)
                _appearanceTween.Stop();
            
            _appearanceTween = Tween.ScaleY(
                transform, 
                0f, 
                appearDuration,
                ease : Ease.InBack,
                useUnscaledTime: true);
            
            return _appearanceTween;
        }

        private void OnDestroy()
        {
           if (_appearanceTween.isAlive)
               _appearanceTween.Stop();
        }
    }
}