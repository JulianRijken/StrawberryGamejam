

using UnityEngine;
using UnityEngine.InputSystem;
using Julian.Sound;

public class TestScriptDelete : MonoBehaviour
{
    [SerializeField] private SoundType m_SoundType;

    [SerializeField] private InputAction m_testAction;

    void Start()
    {
        m_testAction.Enable();
        m_testAction.performed += OnPerformed;
    }

    private void OnPerformed(InputAction.CallbackContext context)
    {
        AudioManager.PlaySound(m_SoundType, 0.5f);
    }




}
