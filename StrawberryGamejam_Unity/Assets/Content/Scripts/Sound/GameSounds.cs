
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Julian.Sound
{
    [CreateAssetMenu(fileName = "GameSounds", menuName = "ScriptableObjects/GameSounds", order = 1)]
    public class GameSounds : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(ValueLabel = "AudioClips", DisplayMode = DictionaryDisplayOptions.OneLine, KeyLabel = "Sound")]
        [SerializeField] private Dictionary<SoundType, AudioClip[]> Sounds;

        public AudioClip GetAudio(SoundType soundType)
        {
            if (!Sounds.ContainsKey(soundType))
                return null;

            if (Sounds[soundType] == null || Sounds[soundType].Length == 0)
                return null;

            return Sounds[soundType].Length > 1 ? Sounds[soundType][Random.Range(0, Sounds[soundType].Length)] : Sounds[soundType][0];
        }
    }
}