using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Material Properties
    [SerializeField] private string m_EdgeSizeMProperty;
    [SerializeField] private string m_FillAlphaMProperty;
    [SerializeField] private string m_RotationAlphaMProperty;
    private Material m_Material;

    // Visual Circle
    [SerializeField] private GameObject m_HalfCircle;

    // Obstacle Properties 
    [HideInInspector] public float MoveSpeed;
    [HideInInspector] public float Distance;
    [HideInInspector] public float EdgeSize;
    [HideInInspector] public float FillAlpha;
    [HideInInspector] public float RotationAlpha;


    private void Awake()
    {
        m_Material = m_HalfCircle.GetComponent<SpriteRenderer>().material;
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

        // Maybe don't do this in update if not needed
        UpdateMaterialProperties();
    }


    private void UpdateMaterialProperties()
    {
        m_Material.SetFloat(m_EdgeSizeMProperty, EdgeSize);
        m_Material.SetFloat(m_FillAlphaMProperty, FillAlpha);
        m_Material.SetFloat(m_RotationAlphaMProperty, RotationAlpha);
    }

}

