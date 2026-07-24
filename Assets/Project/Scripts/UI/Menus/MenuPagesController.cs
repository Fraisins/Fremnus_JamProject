using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class MenuPagesController : MonoBehaviour
    {
        [Serializable]
        private class SubmenuData
        {
            public SubmenuType type;
            public Submenu submenu;
        }
        
        [SerializeField] private List<SubmenuData> submenus;
        [SerializeField] private List<MenuOpenTrigger> triggers;
        
        private Dictionary<SubmenuType, Submenu> _submenusDict;
        private SubmenuType _currentSubmenu = SubmenuType.None;

        private void OnEnable()
        {
            SubscribeToTriggers();
        }

        private void OnDisable()
        {
            UnsubscribeFromTriggers();
        }
        
        private void Awake()
        {
            InitializeDictionary();
        }
        
        private void Start()
        {
            OpenMenu(_currentSubmenu);
        }

        public void OpenMenu(SubmenuType submenuType)
        {
            _currentSubmenu = submenuType;
            
            if (_submenusDict == null) return;
            foreach ((SubmenuType type, Submenu menu) in _submenusDict)
            {
                if (menu == null) continue;
                if (_currentSubmenu == type)
                {
                    menu.gameObject.SetActive(true);
                    menu.OnOpened();
                }
                else
                {
                    menu.gameObject.SetActive(false);
                    menu.OnClosed();
                }
            }
        }
        
        #region Triggers

        public void SubscribeToTriggers()
        {
            foreach (MenuOpenTrigger trigger in triggers)
            {
                if (trigger == null)
                    continue;
                trigger.OnTrigger += OpenMenu;
            }
        }

        public void UnsubscribeFromTriggers()
        {
            foreach (MenuOpenTrigger trigger in triggers)
            {
                if (trigger == null)
                    continue;
                trigger.OnTrigger -= OpenMenu;
            }
        }
            
        #endregion

        #region Dictionary

        private void InitializeDictionary()
        {
            _submenusDict = new Dictionary<SubmenuType, Submenu>();
            foreach (SubmenuData data in submenus)
            {
                if (data == null) continue;
                if (data.submenu == null)
                {
                    Debug.LogError($"[MenuPagesController] Submenu {data.type} has no submenu defined.");
                    continue;
                }
                if (_submenusDict.ContainsKey(data.type))
                {
                    Debug.LogError($"[MenuPagesController] Submenu {data.type} has already been defined. Remove duplicate values.");
                    continue;
                }
                _submenusDict.Add(data.type, data.submenu);
            }
        }

        #endregion
    }
}