using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private string m_EdgeSizeMProperty;
    [SerializeField] private string m_FillAlphaMProperty;
    [SerializeField] private string m_RotationAlphaMProperty;
    [SerializeField] private GameObject m_HalfCircle;

    public HalfCircleSettings HalfCircleSettings;

    public float Distance;
    public float MoveSpeed;

    private Material m_Matarial;
    private Vector3 m_Center = Vector3.zero;


    public void InitializeObstacle(float _startDistance, float _moveSpeed, HalfCircleSettings _settings)
    {
        Distance = _startDistance;
        MoveSpeed = _moveSpeed;

        m_Matarial.SetFloat(m_EdgeSizeMProperty, _settings.EdgeSize);
        m_Matarial.SetFloat(m_FillAlphaMProperty, _settings.FillAlpha);
        m_Matarial.SetFloat(m_RotationAlphaMProperty, _settings.RotationAlpha);

        HalfCircleSettings = _settings;
    }


    private void Awake()
    {
        transform.position = m_Center;
        m_Matarial = m_HalfCircle.GetComponent<SpriteRenderer>().material;
    }


    private void Update()
    {
        Distance -= Time.deltaTime * MoveSpeed;

        if (Distance <= 0)
        {
            Destroy(gameObject);
            return;
        }

        m_HalfCircle.transform.localScale = Vector3.one * Distance;
    }

}

