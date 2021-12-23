using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private GameTimer m_GameTimer;

    [SerializeField] private TextMeshProUGUI highScore;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("Score"))
            highScore.text = PlayerPrefs.GetFloat("Score").ToString();  
        
        m_GameTimer.ResetTimer();
        m_GameTimer.StartTimer();
    }

    private void OnDestroy()
    {
        if (PlayerPrefs.HasKey("Score"))
            if(PlayerPrefs.GetFloat("Score") < m_GameTimer.GetTime())
                PlayerPrefs.SetFloat("Score", m_GameTimer.GetTime());
    }
}
