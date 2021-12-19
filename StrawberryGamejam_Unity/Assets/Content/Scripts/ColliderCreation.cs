using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderCreation : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D m_PolygonCollider2D;
    [SerializeField] private int m_Points;

    [SerializeField] private float m_EdgeSize;
    [SerializeField] private float m_FillAlpha;
    [SerializeField] private float m_RotationAlpha;

    [ExecuteInEditMode]
    private void Update()
    {
        if (!m_PolygonCollider2D)
            return;

        m_PolygonCollider2D.points = GetColliderPoints();
    }

    private void OnDrawGizmos()
    {
        Vector2[] points = GetColliderPoints();

        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(points[i], 0.01f);
        }
       
    }

    private Vector2[] GetColliderPoints()
    {
        List<Vector2> points = new List<Vector2>();


        Vector2 corner1 = transform.position;
        corner1.y += 0.5f * transform.lossyScale.y;
        corner1 = RotateAround(transform.position, corner1, Mathf.Abs(m_FillAlpha - 1) / 2f);
        points.Add(corner1);

        Vector2 corner3 = transform.position;
        corner3.y += 0.5f * transform.lossyScale.y;
        corner3.y -= m_EdgeSize / 2f;
        corner3 = RotateAround(transform.position, corner3, Mathf.Abs(m_FillAlpha - 1) / 2f);
        points.Add(corner3);

        Vector2 corner4 = transform.position;
        corner4.y += 0.5f * transform.lossyScale.y;
        corner4.y -= m_EdgeSize / 2f;
        corner4 = RotateAround(transform.position, corner4, -Mathf.Abs(m_FillAlpha - 1) / 2f);
        points.Add(corner4);

        Vector2 corner2 = transform.position;
        corner2.y += 0.5f * transform.lossyScale.y;
        corner2 = RotateAround(transform.position, corner2, -Mathf.Abs(m_FillAlpha - 1) / 2f);
        points.Add(corner2);

        return points.ToArray();
    }


    Vector2 RotateAround(Vector2 _center, Vector2 _point, float _rotationAlpha)
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
}
