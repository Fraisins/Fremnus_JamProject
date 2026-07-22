using System;
using UnityEngine;

namespace Project.Scripts.UI.Data
{
    [Serializable]
    public class PersistentUIContainer // just transform wrapper for VContainer
    {
        [field: SerializeField] public Transform Container { get; set; }
    }
}