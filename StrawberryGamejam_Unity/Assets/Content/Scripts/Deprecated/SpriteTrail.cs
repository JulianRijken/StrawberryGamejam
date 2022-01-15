using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteTrail : MonoBehaviour
{

    [SerializeField] private InputAction m_MoveControls;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private int m_Sprites;

    private SpriteRenderer m_SpriteRenderer;
    private SplineTrail m_SplineTrail;

    private SpriteRenderer[] m_TrailSprite;




    private void Awake()
    {
        m_MoveControls.Enable();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SplineTrail = GetComponent<SplineTrail>();
    }

    private void Start()
    {
        CreateTrailSprites();
    }

    private void Update()
    {
        transform.position = (Vector2)transform.position + (m_MoveControls.ReadValue<Vector2>() * m_MoveSpeed * Time.deltaTime);
        UpdateTrailSprites();
    }

    private void CreateTrailSprites()
    {
        m_TrailSprite = new SpriteRenderer[m_Sprites];

        for (int i = 0; i < m_Sprites; i++)
        {
            GameObject newGameObject = new GameObject("TrailSprite");
            m_TrailSprite[i] = newGameObject.AddComponent<SpriteRenderer>();
            m_TrailSprite[i].sprite = m_SpriteRenderer.sprite;
        }
    }

    private void UpdateTrailSprites()
    {
        for (int i = 0; i < m_TrailSprite.Length; i++)
        {
            float alpha = (float)i / (m_TrailSprite.Length - 1);
            
            m_TrailSprite[i].transform.position = m_SplineTrail.GetPoint(alpha);
           
        }
    }
}
