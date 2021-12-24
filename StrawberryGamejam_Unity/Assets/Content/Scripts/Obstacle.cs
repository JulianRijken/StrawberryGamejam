using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Obstacle : MonoBehaviour
{
    // Material Properties
    private const string m_EdgeWith_MProperty = "_EdgeWith";
    private const string m_FillAngle_MProperty = "_FillAngle";
    private const string m_Rotation_MProperty = "_Rotation";


    private Material m_Material;
    public Material Material
    {
        get
        {
            if(!m_Material)
                m_Material = GetComponent<SpriteRenderer>().material;

            return m_Material;
        }
    }




    private float m_Distance;
    private float m_EdgeWith;
    private float m_FillAngle;
    private float m_Rotation;


    // Obstacle Properties 
    public float MoveSpeed;
    public float Distance
    {
        get { return m_Distance; }
        set
        {
            transform.localScale = Vector3.one * value;
            m_Distance = value;
        }
    }
    public float EdgeWith
    {
        get {return m_EdgeWith;}

        set 
        {
            Material.SetFloat(m_EdgeWith_MProperty, value);
            m_EdgeWith = value;
        }
    }
    public float FillAngle
    {
        get { return m_FillAngle; }
        set
        {
            Material.SetFloat(m_FillAngle_MProperty, value);
            m_FillAngle = value;
        }
    }
    public float Rotation
    {
        get { return m_Rotation; }
        set
        {
            Material.SetFloat(m_Rotation_MProperty, value);
            m_Rotation = value;
        }
    }


    private void Awake()
    {
        m_Material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        UpdateObstaclePosition();
    }


    private void UpdateObstaclePosition()
    {
        // Move obstacle every frame
        Distance += Time.deltaTime * -MoveSpeed;

        // Destroy when under 0
        if (Distance <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }
}

