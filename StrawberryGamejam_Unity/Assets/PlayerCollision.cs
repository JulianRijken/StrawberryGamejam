using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField] private float m_DeathFether;
    [SerializeField] private HitState m_HitState;

    private Vector2 CheckPoint
    {
        get
        {
            return transform.position + transform.TransformDirection(m_CheckOffset);
        }

    }
    [SerializeField] private Vector2 m_CheckOffset;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
        m_HitState = GetHitState(collision);
        //Debug.Log(m_HitState);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_HitState = HitState.None;
    }

    private HitState GetHitState(Collision2D collision)
    {
        // No collision is no hit
        if (!collision.collider)
            return m_HitState = HitState.None;


        Vector2 direciton = collision.contacts[0].point - CheckPoint;
        float dot = Vector2.Dot(direciton, transform.up);

        Debug.DrawRay(CheckPoint, direciton.normalized * 20f, dot > 0f ? Color.green : Color.blue, 10f);
        Debug.DrawLine(transform.position, collision.contacts[0].point, Color.red, 10f);
        Debug.Log(dot);


        //// check death 
        //if (Mathf.Abs(dot) < m_DeathFether)
        //    return HitState.Dead;


        return dot > 0 ? HitState.Left : HitState.Right;
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
        Gizmos.DrawSphere(CheckPoint, 0.1f);
    }
}
