using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Obstacle m_ObstaclePrefab;

    private void Start()
    {
        StartCoroutine(SpawnEnumerator());
        Application.targetFrameRate = 30;
    }

    private IEnumerator SpawnEnumerator()
    {
        while (true)
        {
            Obstacle spawnedObstacle = Instantiate(m_ObstaclePrefab);

            if (spawnedObstacle)
            {
                HalfCircleSettings settings;
                settings.EdgeSize = 10f;
                settings.FillAlpha = 0.8f;
                settings.RotationAlpha = Random.value;

                spawnedObstacle.InitializeObstacle(100f,  30, settings);
            }

            

            //yield return new WaitForSeconds(Random.Range(0.5f,0.75f));
            yield return new WaitForSeconds(3);
        }
    }

}
