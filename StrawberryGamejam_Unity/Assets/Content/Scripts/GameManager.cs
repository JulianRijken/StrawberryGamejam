using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Obstacle m_ObstaclePrefab;

    private void Start()
    {
        StartCoroutine(SpawnEnumerator());
    }

    private IEnumerator SpawnEnumerator()
    {
        while (true)
        {
            Obstacle spawnedObstacle = Instantiate(m_ObstaclePrefab);
            if (spawnedObstacle)        
                spawnedObstacle.InitializeObstacle(100f, Random.Range(30f,35f), 3.5f, Random.value);
            

            yield return new WaitForSeconds(Random.Range(0.5f,0.75f));
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
