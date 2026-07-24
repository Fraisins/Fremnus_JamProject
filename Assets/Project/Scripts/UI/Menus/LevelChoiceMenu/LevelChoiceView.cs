using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class LevelChoiceView : CustomButton
    {
        [Header("Level")]
        [SerializeField] private string levelName;
        
        [Header("Appearance")]
        [SerializeField] private TMP_Text additionalInfoText;
        [SerializeField] private Color disabledBackgroundColor;
        
        public string LevelName => levelName;
        public event Action<string> OnLevelPicked;

        public void SetAvailable(bool available)
        {
            isActive = available;
            canSetVisuals = available;
            background.color = disabledBackgroundColor;
            SetVisualsHereAndHow();
        }

        public void SetAdditionalInfoText(string text)
        {
            if (additionalInfoText == null)
            {
                Debug.LogWarning("[LevelChoiceView]: additionalInfoText is null");
                return;
            }
            additionalInfoText.text = text;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!isActive)
                return;
            
            base.OnPointerDown(eventData);
            OnLevelPicked?.Invoke(levelName);
        }

        protected override void SetColors(State state)
        {
            base.SetColors(state);
            
            if (!isActive)
                background.color = disabledBackgroundColor;
        }
    }
}