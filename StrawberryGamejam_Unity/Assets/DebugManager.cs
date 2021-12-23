using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private InputAction  m_TimeSkipAction;

    private void Start()
    {
        m_TimeSkipAction.Enable();
        m_TimeSkipAction.performed += OnTimeSkipAction;
        m_TimeSkipAction.canceled += OnTimeSkipAction;
    }

    private void OnTimeSkipAction(InputAction.CallbackContext obj)
    {
        Debug.Log("hi");
        Time.timeScale = obj.performed ? 10 : 1;
    }


}
