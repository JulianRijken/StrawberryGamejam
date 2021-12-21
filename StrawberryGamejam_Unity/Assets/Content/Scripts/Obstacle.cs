using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Material Properties
    [SerializeField] private string m_EdgeSizeMProperty;
    [SerializeField] private string m_FillAlphaMProperty;
    [SerializeField] private string m_RotationAlphaMProperty;
    private Material m_Matarial;

    // Visual Circle
    [SerializeField] private GameObject m_HalfCircle;

    // Obstacle Properties 
    public float MoveSpeed;
    public float Distance;
    public float EdgeSize;
    public float FillAlpha;
    public float RotationAlpha;



    private void Awake()
    {
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


    public void InitializeObstacle(ObstacleSettings _spawnSettings)
    {
        // Apply Settings
        MoveSpeed = _spawnSettings.MoveSpeed;
        Distance = _spawnSettings.Distance;
        EdgeSize = _spawnSettings.EdgeSize;
        FillAlpha = _spawnSettings.FillAlpha;
        RotationAlpha = _spawnSettings.RotationAlpha;

        m_Matarial.SetFloat(m_EdgeSizeMProperty, _spawnSettings.EdgeSize);
        m_Matarial.SetFloat(m_FillAlphaMProperty, _spawnSettings.FillAlpha);
        m_Matarial.SetFloat(m_RotationAlphaMProperty, _spawnSettings.RotationAlpha);
    }

}

