using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelMenu : MonoBehaviour
{

    [SerializeField] private string[] m_WheelOptions;
    [SerializeField] private WheelMenuOption m_WheelMenuOptionPrefab;
    [SerializeField] private float m_Spacing;
    [SerializeField] private int m_Buffer;
    [SerializeField] private float m_TransitionTime;
    [SerializeField] private int m_SelectedIndex;
    [SerializeField] private float test;


    private Dictionary<int, WheelMenuOption> wheelMenuOptions = new Dictionary<int, WheelMenuOption>();


    private void Start()
    {
        Controls controls = new Controls();
        controls.UI.Enable();
        controls.UI.Left.performed += OnLeftInput;
        controls.UI.Right.performed += OnRightInput;

        for (int i = -m_Buffer; i < (m_Buffer*2); i++)
        {

      

            WheelMenuOption wheelOption = Instantiate(m_WheelMenuOptionPrefab, transform);
            wheelOption.transform.rotation = Quaternion.Euler(Vector3.forward * (i * m_Spacing));

            wheelMenuOptions.Add(i, wheelOption);

        }


        //for (int i = 0; i < m_WheelOptions.Length; i++)
        //{
        //    //m_WheelOptions[]Instantiate(m_WheelMenuOptionPrefab,transform);
            
        //}
    }

    private void OnLeftInput(InputAction.CallbackContext context)
    {
        OnRotate(-1);
    }

    private void OnRightInput(InputAction.CallbackContext context)
    {
        OnRotate(1);
    }

    private void OnRotate(int delta)
    {
        if (delta == 0)
            return;

        delta = Mathf.Clamp(delta,-1, 1);

        m_SelectedIndex += delta;

        m_SelectedIndex = ClampContinue(m_SelectedIndex, 0, m_WheelOptions.Length);


        StartCoroutine(TransitionAnimation(delta));
       
    }


    private int ClampContinue(int value, int min, int max)
    {       
        if (value < min)
            return max - 1;
        else if (value >= max)
            return min;

        return value;
    }

    private void UpdateAlpha(float alpha)
    {
        foreach (KeyValuePair<int, WheelMenuOption> option in wheelMenuOptions)
        {
            int index = ClampContinue((option.Key * -1) + m_SelectedIndex, 0, m_WheelOptions.Length);


            option.Value.ColorAlpha = Quaternion.Dot(Quaternion.identity, option.Value.transform.rotation);

            //if (option.Key == 0)
            //    option.Value.ColorAlpha = 1f;
            //else
            //    option.Value.ColorAlpha = 0.5f;

            //int index = ClampContinue((option.Key * -1) + m_SelectedOption, 0, m_WheelOptions.Length);
            //option.Value.OptionName = m_WheelOptions[index];
        }
    }


    private void UpdateNames()
    {
        foreach (KeyValuePair<int, WheelMenuOption> option in wheelMenuOptions)
        {
            int index = ClampContinue((option.Key * -1) + m_SelectedIndex, 0, m_WheelOptions.Length);
            option.Value.OptionName = m_WheelOptions[index];

            //entry.
            // do something with entry.Value or entry.Key
        }

    }




    private IEnumerator TransitionAnimation(int rotateDirection)
    {

        UpdateNames();

        float alpha = 0f;
        for (; ; )
        {

            float angle =  Mathf.LerpAngle(rotateDirection > 0 ? -m_Spacing : m_Spacing, 0f, alpha);
            transform.localRotation = Quaternion.Euler(Vector3.forward * angle);
            UpdateAlpha(alpha);

            if (alpha >= 1f) break;

            yield return new WaitForEndOfFrame();
            alpha = Mathf.Clamp01(alpha + (Time.deltaTime * (1 / m_TransitionTime)));
        }
    }
}
