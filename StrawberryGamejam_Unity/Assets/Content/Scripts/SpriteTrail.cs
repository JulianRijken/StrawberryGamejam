using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrail : MonoBehaviour
{

    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private float m_SpawnInterval;
    [SerializeField] private float m_LifeTime;
    [SerializeField] private Gradient m_ColorCurve;
    [SerializeField] private AnimationCurve m_SizeCurve;

    private List<TrailObject> m_Clones = new List<TrailObject>();

    private ObjectPool<SpriteRenderer> m_Pool = new ObjectPool<SpriteRenderer>();

    private void Start()
    {
        StartCoroutine(SpawnInterval());
    }

    private float m_MoveDelta;
    private Vector2 m_LastPosition;
    private Quaternion m_lastRotaiton;

    private void Update()
    {

        // Update Move Delta
        m_MoveDelta = 0f;
        m_MoveDelta += Vector2.Distance(transform.position, m_LastPosition);
        m_MoveDelta += Quaternion.Angle(transform.rotation,m_lastRotaiton);

        m_LastPosition = transform.position;
        m_lastRotaiton = transform.rotation;





        for (int i = 0; i < m_Clones.Count; i++)
        {

            m_Clones[i].TimeAlive += Time.deltaTime * (1f / m_LifeTime);


            if(m_Clones[i].TimeAlive > 1f)
            {
                m_Clones[i].SpriteRenderer.gameObject.SetActive(false);
                m_Clones.RemoveAt(i);
                i--;
            }
            else
            {
                m_Clones[i].SpriteRenderer.color = m_ColorCurve.Evaluate(m_Clones[i].TimeAlive);

                m_Clones[i].SpriteRenderer.transform.localScale = transform.localScale * m_SizeCurve.Evaluate(m_Clones[i].TimeAlive);
            }
        }
    }

    private IEnumerator SpawnInterval()
    {
        while(true)
        {

            SpriteRenderer spawnedSpriteRenderer = m_Pool.GetObject(transform.position, transform.rotation, "Sprite Trail Object");

            spawnedSpriteRenderer.sprite = m_SpriteRenderer.sprite;

            m_Clones.Add(new TrailObject(spawnedSpriteRenderer, 0f));

            yield return new WaitForSeconds(m_SpawnInterval);
            yield return new WaitUntil(() => m_MoveDelta > 0f);
        }
    }

    private class TrailObject
    {
        public TrailObject(SpriteRenderer SpriteRenderer,float LifeTime)
        {
            this.SpriteRenderer = SpriteRenderer;
            this.TimeAlive = LifeTime;
        }

        public SpriteRenderer SpriteRenderer;
        public float TimeAlive;
    }
}
