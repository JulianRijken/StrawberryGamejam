using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private float m_RotateSpeed;
    [SerializeField] private float m_RotateSwayDistance;
    [SerializeField] private float m_RotateSwayDampSpeed;
    [SerializeField] private GameObject m_PlayerSprite;
    [SerializeField] private PlayerCollision m_PlayerCollision;

    private Controls m_Controls;

    

    private void Awake()
    {
        m_Controls = new Controls();
        m_Controls.Enable();
    }

    private void FixedUpdate()
    {

        float rotateInput = Mathf.Clamp(m_Controls.Player.Rotate.ReadValue<float>() , m_PlayerCollision.CurrentHitState.Equals(PlayerCollision.HitState.Left) ? 0 : -1f, m_PlayerCollision.CurrentHitState.Equals(PlayerCollision.HitState.Right) ? 0 : 1f);


        float rotateValue = m_RotateSpeed * Time.deltaTime * -rotateInput;

        transform.Rotate(Vector3.forward, rotateValue);

        if(m_PlayerSprite)
        {
            Quaternion targetRotation = transform.rotation;
            targetRotation *= Quaternion.Euler(0, 0, rotateValue * m_RotateSwayDistance);

            m_PlayerSprite.transform.rotation = Quaternion.Slerp(m_PlayerSprite.transform.rotation,targetRotation, m_RotateSwayDampSpeed * Time.deltaTime);
        }

    }




}
