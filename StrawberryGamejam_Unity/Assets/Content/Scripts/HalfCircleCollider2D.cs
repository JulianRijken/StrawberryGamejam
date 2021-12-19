using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class HalfCircleCollider2D : MonoBehaviour
{

    [SerializeField] public Transform ScaleTransform;
    [SerializeField] private int m_FixedDivisions;
    [SerializeField] private float m_Resolution;
    [Range(1,100)]
    [SerializeField] private int m_MaxDivisions;

    [SerializeField] private float m_HeightOffset = 0;

    [SerializeField] private bool m_DynamicDevisions;
    [SerializeField] private HalfCircleSettings m_Settings;

    private PolygonCollider2D m_PolygonCollider2D;


    private void Awake()
    {
        m_PolygonCollider2D = GetComponent<PolygonCollider2D>();
        m_PolygonCollider2D.enabled = false;
    }

    public void SetSetings(HalfCircleSettings _settings)
    {
        m_Settings = _settings;
    }

    public void UpdateCollider()
    {
        Vector2[] points = GetColliderPoints();
        m_PolygonCollider2D.enabled = points.Length > 0 ? true : false;
        m_PolygonCollider2D.points = points;
    }



    private Vector2[] GetColliderPoints()
    {
        List<Vector2> points = new List<Vector2>();

        
        int targetDevisions = m_DynamicDevisions ? Mathf.RoundToInt(m_Resolution * m_Settings.FillAlpha * ScaleTransform.lossyScale.y) : m_FixedDivisions;
        int finalDeveisions = Mathf.Clamp(targetDevisions + 1, 1, m_MaxDivisions);


        // Get top edge
        for (int i = 0; i <= finalDeveisions; i++)
        {
            // Get The Point Offset
            Vector2 point = transform.position;
            point.y += (0.5f + m_HeightOffset) * Mathf.Max(0, ScaleTransform.lossyScale.y);

            // Get Rotated Point
            float pointLerp = Mathf.Lerp(-1, 1, i / (float)finalDeveisions);
            float pointAngleAlpha = pointLerp * (m_Settings.FillAlpha / 2f);
            point = RotateAround(transform.position, point, pointAngleAlpha + m_Settings.RotationAlpha);

            // Add Point To List
            points.Add(point);
        }

        // Get bottom edge
        for (int i = finalDeveisions; i >= 0; i--)
        {
            // Get Point Offset
            Vector2 point = transform.position;
            point.y += ((0.5f + m_HeightOffset) * Mathf.Max(m_Settings.EdgeSize, ScaleTransform.lossyScale.y)) - m_Settings.EdgeSize / 2f;

            // Get Rotated Point
            float times = Mathf.Lerp(-1, 1, i / (float)finalDeveisions);
            float pointAngleAlpha = times * m_Settings.FillAlpha / 2f;
            point = RotateAround(transform.position, point, pointAngleAlpha + m_Settings.RotationAlpha);

            // Add Point To List
            points.Add(point);
        }


        return points.ToArray();
    }

    private Vector2 RotateAround(Vector2 _center, Vector2 _point, float _rotationAlpha)
    {
        float angle = _rotationAlpha * Mathf.PI * 2f;

        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);

        // translate point back to origin:
        _point.x -= _center.x;
        _point.y -= _center.y;

        // rotate point
        float xnew = _point.x * c - _point.y * s;
        float ynew = _point.x * s + _point.y * c;

        // translate point back:
        _point.x = xnew + _center.x;
        _point.y = ynew + _center.y;
        return _point;
    }




#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        if (!ScaleTransform)
            return;

        Vector2[] points = GetColliderPoints();

        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(points[i], 0.5f);
        }

        if(m_PolygonCollider2D)
        {
            if (m_PolygonCollider2D.enabled == true)
            {
                for (int i = 0; i < m_PolygonCollider2D.points.Length; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(m_PolygonCollider2D.points[i], 0.5f);
                }
            }
        }

    }

    private void OnValidate()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        if(!collider)
        {
            collider = gameObject.AddComponent<PolygonCollider2D>();
        }

        m_PolygonCollider2D = collider;
        m_PolygonCollider2D.enabled = false;
    }

#endif

}
