using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [HideInInspector] public HitState CurrentHitState;

    [SerializeField] private float m_DeathFether;
    [SerializeField] private Vector2 m_TopPointOffset;
    [SerializeField] private LayerMask m_CollisionLayer;

    private Vector2 m_TopPoint
    {
        get { return transform.position + transform.TransformDirection(m_TopPointOffset); }
    }

    //private void FixedUpdate()
    //{
    //    RaycastHit2D hit2D = Physics2D.Linecast(m_TopPoint, Vector2.zero, m_CollisionLayer);
    //    if(hit2D.collider)
    //    {
    //        Debug.DrawLine(m_TopPoint, hit2D.point, Color.blue);

    //        if (hit2D.distance < m_DeathFether)
    //            Debug.Log("dead");
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider)
        {
            Vector2 direciton = collision.contacts[0].point - (Vector2)transform.position;
            float Angle = Vector2.SignedAngle(transform.up, direciton);

            Debug.DrawRay(transform.position, direciton.normalized * 20f, Angle > 0f ? Color.green : Color.blue, 10f);
            CurrentHitState = Angle > 0f ? HitState.Left : HitState.Right;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CurrentHitState = HitState.None;
    }



    public enum HitState
    {
        None,
        Dead,
        Left,
        Right
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(m_TopPoint, 0.1f);
    }
}
