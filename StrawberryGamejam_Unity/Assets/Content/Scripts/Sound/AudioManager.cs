using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Julian.Sound
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource m_AudioSource;
        private GameSounds m_GameSounds;

        private static AudioManager m_Instance;
        public static AudioManager Instance
        {
            get
            {
                if (!m_Instance)
                {
                    m_Instance = new GameObject(typeof(AudioManager).Name).AddComponent<AudioManager>();
                    m_Instance.m_AudioSource = m_Instance.gameObject.AddComponent<AudioSource>();
                    m_Instance.m_GameSounds = Resources.Load<GameSounds>("GameSounds/GameSounds");
                    DontDestroyOnLoad(m_Instance);
                }

                return m_Instance;
            }
        }


        public static void PlaySound(SoundType soundType, float volumeScale = 1f)
        {
            // Avoid Constant Checking
            AudioManager I = Instance;

            AudioClip clip = I.m_GameSounds.GetAudio(soundType);
            if(!clip)
            {
                Debug.LogWarning($"No sound for: {soundType}");
                return;
            }

            I.m_AudioSource.PlayOneShot(I.m_GameSounds.GetAudio(soundType), volumeScale);
        }
    }

}


