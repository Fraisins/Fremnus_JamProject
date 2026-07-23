using PrimeTween;
using TMPro;
using UnityEngine;

namespace Project.UI
{
    public class ModAttachedView : MonoBehaviour
    {
        private bool _isShowing = false;
        
        [SerializeField] private Transform visualTransform;
        [SerializeField] private TMP_Text modValueText;

        [SerializeField] private float yPosition;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;
        
        private float _originalYPosition;
        private Tween _showHideTween;

        private void Awake()
        {
            if (visualTransform == null)
                return;
            
            visualTransform.localPosition = new Vector3(
                visualTransform.localPosition.x,
                visualTransform.localPosition.y - yPosition,
                visualTransform.localPosition.z);
            
            _originalYPosition = visualTransform.localPosition.y;
            modValueText.gameObject.SetActive(false);
        }
        
        public void SetValue(int value)
        {
            if (value != 0)
                ShowMod();
            else
                HideMod();
            
            if (modValueText == null)
                return;
            modValueText.text = ModHelper.ValueToString(value);
        }

        public void ShowMod()
        {
            if(_isShowing)
                return;
            
            modValueText.gameObject.SetActive(true);
            _isShowing = true;
            PlayVisualMoveAnimation(_originalYPosition + yPosition);
        }

        private void HideMod()
        {
            if (!_isShowing)
                return;

            modValueText.gameObject.SetActive(false);
            _isShowing = false;
            PlayVisualMoveAnimation(_originalYPosition);
        }

        private void PlayVisualMoveAnimation(float targetYPos)
        {
            if (visualTransform  == null)
                return;
            
            if (_showHideTween.isAlive)
                _showHideTween.Stop();

            if (Mathf.Approximately(visualTransform.localPosition.y, targetYPos)) 
                return;
            _showHideTween = Tween.LocalPositionY(visualTransform, targetYPos, duration, ease);
        }

        private void OnDestroy()
        {
            if (_showHideTween.isAlive)
                _showHideTween.Stop();
        }
    }
}