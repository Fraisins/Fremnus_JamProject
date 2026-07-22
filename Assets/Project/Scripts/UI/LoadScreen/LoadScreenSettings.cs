using System;
using UnityEngine;

namespace Project.UI
{
    [Serializable]
    public class LoadScreenSettings
    {
        [SerializeField] private bool useLoadScreen;
        [SerializeField] private LoadScreenView loadScreenPrefab;
        
        public bool UseLoadScreen => useLoadScreen;
        public LoadScreenView LoadScreenPrefab => loadScreenPrefab;
    }
}