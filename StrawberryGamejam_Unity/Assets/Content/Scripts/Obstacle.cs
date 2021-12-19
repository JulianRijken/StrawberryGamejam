using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private string m_EdgeSizeMProperty;
    [SerializeField] private string m_FillAlphaMProperty;
    [SerializeField] private string m_RotationAlphaMProperty;

    [SerializeField] private float m_CollideDistance;

    [SerializeField] private HalfCircleCollider2D m_HalfCircleCollider2D;
    [SerializeField] private GameObject m_HalfCircle;

    private float m_Distance;
    private float m_MoveSpeed;

    private Material m_Matarial;
    private Vector3 m_Center = Vector3.zero;


    public void InitializeObstacle(float _startDistance, float _moveSpeed, HalfCircleSettings _settings)
    {
        m_Distance = _startDistance;
        m_MoveSpeed = _moveSpeed;

        m_Matarial.SetFloat(m_EdgeSizeMProperty, _settings.EdgeSize);
        m_Matarial.SetFloat(m_FillAlphaMProperty, _settings.FillAlpha);
        m_Matarial.SetFloat(m_RotationAlphaMProperty, _settings.RotationAlpha);

        m_HalfCircleCollider2D.SetSetings(_settings);

        UpdateObstacle();
    }


    private void Awake()
    {
        transform.position = m_Center;
        m_Matarial = m_HalfCircle.GetComponent<SpriteRenderer>().material;
    }


    private void Update()
    {
        UpdateObstacle();
    }


    private void UpdateObstacle()
    {
        m_Distance -= Time.deltaTime * m_MoveSpeed;

        if (m_Distance <= 0)
        {
            Destroy(gameObject);
            return;
        }

        m_HalfCircle.transform.localScale = Vector3.one * m_Distance;


        if(m_Distance < m_CollideDistance)
            m_HalfCircleCollider2D.UpdateCollider();   
    }



}

