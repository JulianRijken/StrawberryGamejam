
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace Julian.Sound
{
    [CreateAssetMenu(fileName = "GameSounds", menuName = "ScriptableObjects/GameSounds", order = 1)]
    public class GameSounds : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(ValueLabel = "AudioClips", DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Sound")]
        [SerializeField] private Dictionary<SoundType, Sound> Sounds;

        public Sound GetAudio(SoundType soundType)
        {
            if (!Sounds.ContainsKey(soundType))
                return new Sound();

            return Sounds[soundType];
        }
    }

    public struct Sound
    {
        public AudioClip[] Clips;

        [Title("MixerGroup")]
        public AudioMixerGroup MixerGroup;


        [Title("Pitch")]
        public bool UsePitchRange;
        [ShowIf("UsePitchRange")]
        public Vector2 PitchRange;
        [HideIf("UsePitchRange")]
        public float Pitch;

        [Title("Volume")]
        public bool UseVolumeRange;
        [ShowIf("UseVolumeRange")]
        public Vector2 VolumeRange;
        [HideIf("UseVolumeRange")]
        public float Volume;

        public AudioClip Clip
        {
            get
            {
                if (Clips == null || Clips.Length == 0)
                    return null;

                return Clips.Length > 1 ? Clips[Random.Range(0, Clips.Length)] : Clips[0];
            }
        }
    }
}