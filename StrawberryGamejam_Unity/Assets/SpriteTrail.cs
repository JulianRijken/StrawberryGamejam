using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrail : MonoBehaviour
{

    [SerializeField] private float m_SpawnInterval;
    [SerializeField] private float m_TrailLength;
    [SerializeField] private Gradient m_Gradient = new Gradient();

    private SpriteRenderer m_TargetSpriteRender;
    private ObjectPool<DissolveSprite> m_TrailObjects = new ObjectPool<DissolveSprite>();

    float timer;

    private void Awake()
    {
        m_TargetSpriteRender = GetComponent<SpriteRenderer>();

        //Time.timeScale = 0.1f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= m_SpawnInterval)
        {
            bool createdNew = false;
            DissolveSprite trailObject = m_TrailObjects.GetObject(transform.position, transform.rotation, out createdNew);

            if (!createdNew)
            {
                trailObject.Gradient = m_Gradient;
                trailObject.DesolveSpeed = m_TrailLength;
                trailObject.SpriteRenderer.sprite = m_TargetSpriteRender.sprite;
                trailObject.SpriteRenderer.material = m_TargetSpriteRender.material;
            }

            timer -= m_SpawnInterval;
        }
    }


}

public class DissolveSprite : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    private float Alpha;

    [HideInInspector] public float DesolveSpeed;
    [HideInInspector] public Gradient Gradient;

    private void Awake()
    {
        SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Alpha = 0f;
    }

    private void Update()
    {
        Alpha += Time.deltaTime / DesolveSpeed;

        if(this.Gradient != null)
            SpriteRenderer.color = this.Gradient.Evaluate(Alpha);

        if(Alpha >= 1f)
        {
            gameObject.SetActive(false);
        }
    }

}


