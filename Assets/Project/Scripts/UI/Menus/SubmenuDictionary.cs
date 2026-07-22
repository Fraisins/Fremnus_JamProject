using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    [Serializable]
    public class SubmenuDictionary
    {
        [Serializable]
        private class SubmenuData
        {
            public SubmenuType type;
            public GameObject submenu;
        }

        [SerializeField] private List<SubmenuData> submenus;
        private Dictionary<SubmenuType, GameObject> _submenusDict;
            
        public IReadOnlyDictionary<SubmenuType, GameObject> GetSubmenuDictionary()

        {
            if (_submenusDict != null)
                return _submenusDict;
            _submenusDict = new Dictionary<SubmenuType, GameObject>();
            foreach (SubmenuData data in submenus)
            {
                if (data == null) continue;
                if (data.submenu == null)
                {
                    Debug.LogError($"Submenu {data.type} has no submenu defined.");
                    continue;
                }
                if (_submenusDict.ContainsKey(data.type))
                {
                    Debug.LogError($"Submenu {data.type} has already been defined. Remove duplicate values.");
                    continue;
                }
                _submenusDict.Add(data.type, data.submenu);
            }
            return _submenusDict;
        }
    }
}