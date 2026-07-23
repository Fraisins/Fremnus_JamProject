using System;
using UnityEngine;

namespace Project.UI
{
    [Serializable]
    public class PersistentUIContainer // just transform wrapper for VContainer
    {
        [field: SerializeField] public Transform Container { get; set; }
    }
}