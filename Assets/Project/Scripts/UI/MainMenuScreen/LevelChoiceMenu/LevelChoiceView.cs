using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class LevelChoiceView : CustomButton
    {
        [SerializeField] public string levelName;
        public string LevelName => levelName;

        public event Action<string> OnLevelPicked;

        private void Start()
        {
            label.text = levelName;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnLevelPicked?.Invoke(levelName);
        }
    }
}