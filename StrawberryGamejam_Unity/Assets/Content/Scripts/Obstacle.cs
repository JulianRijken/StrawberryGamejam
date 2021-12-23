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
    [HideInInspector] public float MoveSpeed;
    [HideInInspector] public float Distance;
    [HideInInspector] public float EdgeSize;
    [HideInInspector] public float FillAlpha;
    [HideInInspector] public float RotationAlpha;


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

        // Maybe don't do this in update if not needed
        UpdateMaterialProperties();
    }


    private void UpdateMaterialProperties()
    {
        m_Matarial.SetFloat(m_EdgeSizeMProperty, EdgeSize);
        m_Matarial.SetFloat(m_FillAlphaMProperty, FillAlpha);
        m_Matarial.SetFloat(m_RotationAlphaMProperty, RotationAlpha);
    }

}

