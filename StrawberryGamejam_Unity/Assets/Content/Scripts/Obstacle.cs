using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private string m_RotationMProperty;
    [SerializeField] private string m_EdgeSizeMProperty;

    private float m_Distance;
    private float m_MoveSpeed;

    private Material m_Matarial;
    private Vector3 m_Center = Vector3.zero;

    private void Awake()
    {
        transform.position = m_Center;
        m_Matarial = GetComponent<SpriteRenderer>().material;
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
        }
        else
        {
            transform.localScale = Vector3.one * m_Distance;
        }
    }

    public void InitializeObstacle(float _startDistance, float _moveSpeed, float _edgeSize, float _rotation)
    {
        m_Distance = _startDistance;
        m_MoveSpeed = _moveSpeed;

        m_Matarial.SetFloat(m_EdgeSizeMProperty, _edgeSize);
        m_Matarial.SetFloat(m_RotationMProperty, _rotation);

        UpdateObstacle();
    }
}
