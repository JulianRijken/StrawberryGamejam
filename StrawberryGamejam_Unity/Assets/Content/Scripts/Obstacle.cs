using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private string m_RotationMProperty;
    [SerializeField] private string m_EdgeSizeMProperty;
    [SerializeField] private GameObject m_HalfCircle;


    private float m_Distance;
    private float m_MoveSpeed;

    private Material m_Matarial;
    private Vector3 m_Center = Vector3.zero;

    private void Awake()
    {
        transform.position = m_Center;
        m_Matarial = m_HalfCircle.GetComponent<SpriteRenderer>().material;

        rotateDirection = Random.value > 0.5f ? 1 : -1;
    }


    private void Update()
    {
        UpdateObstacle();
    }

    int rotateDirection;

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

            transform.Rotate(Vector3.forward * Time.deltaTime * 100);
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
