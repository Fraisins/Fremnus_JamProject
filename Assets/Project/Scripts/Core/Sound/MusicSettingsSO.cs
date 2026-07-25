using UnityEngine;

namespace Project.Sound
{
    [CreateAssetMenu(fileName = "MusicSettingsSO", menuName = "FremnusJam/Sound/MusicSettingsSO")]
    public class MusicSettingsSO : ScriptableObject
    {
        [field: SerializeField] public AudioClip GameMusic { get; private set; }
    }
}