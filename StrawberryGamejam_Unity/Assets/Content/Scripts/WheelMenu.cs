using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Julian.Extention;
using Sirenix.OdinInspector;

public class WheelMenu : MonoBehaviour
{

    [SerializeField] private string[] m_WheelOptions;
    [SerializeField] private WheelMenuOption m_WheelMenuOptionPrefab;
    [SerializeField] private float m_Spacing;
    [SerializeField] private int m_Buffer;
    [SerializeField] private float m_TransitionTime;
    [SerializeField] private float m_Offset;


    [ReadOnly, ShowInInspector] private int m_SelectedIndex;
    [ReadOnly, ShowInInspector] private bool m_Rotating;


    private Dictionary<int, WheelMenuOption> wheelMenuOptions = new Dictionary<int, WheelMenuOption>();


    private void Awake()
    {
        InitializeBufferOptions();
    }

    private void InitializeBufferOptions()
    {
        for (int i = -m_Buffer; i <= m_Buffer; i++)
        {
            WheelMenuOption wheelOption = Instantiate(m_WheelMenuOptionPrefab, transform);

          
            wheelOption.transform.rotation = Quaternion.Euler(Vector3.forward * (i * m_Spacing));
            wheelOption.transform.position = transform.position + (wheelOption.transform.up * m_Offset);

            wheelMenuOptions.Add(i, wheelOption);
        }
    }

    private void UpdateAnimation(float rotationAlpha, int rotateDirection)
    {

        // Update Wheel Rotation
        float wheelTargetRotation = Mathf.LerpAngle(rotateDirection > 0 ? -m_Spacing : m_Spacing, 0f, rotationAlpha);
        transform.localRotation = Quaternion.Euler(Vector3.forward * wheelTargetRotation);

        // Update option distance alpha
        foreach (KeyValuePair<int, WheelMenuOption> option in wheelMenuOptions)
        {
            float optionAlpha = Vector2.Angle(Vector2.up, option.Value.transform.up);
            float distanceAlpha = Mathf.Abs(1 - Mathf.Clamp01(optionAlpha / (m_Buffer * m_Spacing)));

            option.Value.SetAlpha(distanceAlpha);
        }
    }


    private void UpdateNames()
    {
        foreach (KeyValuePair<int, WheelMenuOption> option in wheelMenuOptions)
        {
            int index = ((option.Key * -1) + m_SelectedIndex).ClampContinue(0, m_WheelOptions.Length);
            option.Value.OptionName = m_WheelOptions[index];
        }
    }




    private IEnumerator TransitionAnimation(int rotateDirection)
    {
        m_Rotating = true;

        float alpha = 0f;
        for (; ; )
        {        
            UpdateAnimation(alpha, rotateDirection);

            if (alpha >= 1f) break;

            yield return new WaitForEndOfFrame();
            alpha = Mathf.Clamp01(alpha + (Time.deltaTime * (1 / m_TransitionTime)));
        }

        m_Rotating = false;
    }



    public void TryRotate(int delta)
    {
        if (m_Rotating)
            return;

        if (delta == 0)
            return;



        delta = Mathf.Clamp(delta,-1, 1);
        m_SelectedIndex += delta;
        m_SelectedIndex = m_SelectedIndex.ClampContinue(0, m_WheelOptions.Length);

        UpdateNames();

        StartCoroutine(TransitionAnimation(delta));    
    }
}
