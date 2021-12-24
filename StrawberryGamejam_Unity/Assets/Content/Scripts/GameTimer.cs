using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_SecondsText;
    [SerializeField] private TextMeshProUGUI m_MillisecondsText;
    private float m_Timer;
    private bool m_TimerTicking;

    public void StartTimer()
    {
        m_TimerTicking = true;
    }

    public void StopTimer()
    {
        m_TimerTicking = false;
    }

    public void ResetTimer()
    {
        m_Timer = 0f;
    }

    public float GetTime()
    {
        return m_Timer;
    }


    private void Update()
    {
        if (m_TimerTicking)
        {
            m_Timer += Time.deltaTime;

            TimeSpan timerTimespan = TimeSpan.FromSeconds(m_Timer);
            m_SecondsText.text = Mathf.RoundToInt(m_Timer).ToString();
            m_MillisecondsText.text = Mathf.Clamp(timerTimespan.Milliseconds / 10f, 0f, 99f).ToString("00");
        }
    }

    private void LateUpdate()
    {
        Camera camera = Camera.main;

        transform.position = camera.WorldToScreenPoint(Vector3.zero);
    }
}
