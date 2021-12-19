using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Vector2 m_CheckOffset;

    private Vector2 CheckPoint
    {
        get
        {
            return transform.position + transform.TransformDirection(m_CheckOffset);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.DrawLine(transform.position, collision.contacts[0].point, Color.red, 5f);
        Vector2 direciton = collision.contacts[0].point - CheckPoint;

        float dot = Vector2.Dot(direciton, transform.up);

        Debug.DrawRay(CheckPoint, direciton.normalized * 20f, dot > 0f ? Color.green : Color.blue, 10f);
        Debug.Log(dot);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(CheckPoint, 0.1f);
    }

}
