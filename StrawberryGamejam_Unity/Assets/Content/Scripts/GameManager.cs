using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Obstacle m_ObstaclePrefab;
    [SerializeField] private ObstacleGroup testGroup;

    [SerializeField] private float m_SpawnDistance = 100f;
    [SerializeField] private float m_SpawnSpeed = 50f;

    [SerializeField] private int m_targetFPS = 300;


    private Obstacle m_LastSpawnedObstacle;
    private float m_LastSpawnedDistanceOvershot;


    private void Awake()
    {
        Application.targetFrameRate = m_targetFPS;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnumerator());

        Controls controls = new Controls();
        controls.Enable();
        controls.Game.Restart.performed += RestartGame;
    }

    private void Update()
    {
        // Delete
        Application.targetFrameRate = m_targetFPS;
    }


    private void RestartGame(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }




    private IEnumerator SpawnEnumerator()
    {
        ObstacleGroup obstacleGroup = testGroup;

        // Repete Obstacle \\
        for (int obstacleRepeteIndex = 0; obstacleRepeteIndex < Mathf.Max(1, obstacleGroup.loopSettings.RepeteTimes); obstacleRepeteIndex++)
        {

            // Ring Spawn Loop \\
            for (int ringIndex = 0; ringIndex < testGroup.Rings.Length; ringIndex++)
            {
                ObstacleRing ring = testGroup.Rings[ringIndex];

                // Repete Ring \\
                for (int ringRepeteIndex = 0; ringRepeteIndex < Mathf.Max(1, ring.loopSettings.RepeteTimes); ringRepeteIndex++)
                {



                    // Obstacle Spawn Loop \\
                    for (int obstacleIndex = 0; obstacleIndex < ring.Obstacles.Length; obstacleIndex++)
                    {
                        
                        ObstacleSettings spawnObstacle = ring.Obstacles[obstacleIndex];
                        spawnObstacle.MoveSpeed = m_SpawnSpeed;
                        spawnObstacle.Distance = (m_SpawnDistance - m_LastSpawnedDistanceOvershot) + (spawnObstacle.EdgeSize + obstacleGroup.GlobalEdgeSize);

                        spawnObstacle.RotationAlpha += ring.loopSettings.GetRotateAlpha(ringRepeteIndex);

                        spawnObstacle.EdgeSize += obstacleGroup.GlobalEdgeSize;

                        m_LastSpawnedObstacle = SpawnObstacle(spawnObstacle);
                    }




                    // Ring Spawn Delay \\
                    if (m_LastSpawnedObstacle)
                    {
                        yield return new WaitUntil(() => (m_SpawnDistance - m_LastSpawnedObstacle.Distance) > m_LastSpawnedObstacle.EdgeSize);
                        m_LastSpawnedDistanceOvershot = (m_SpawnDistance - m_LastSpawnedObstacle.Distance) - (ring.Spacing + obstacleGroup.GlobalSpacing);
                    }


                }
            }

        }
    }



    private Obstacle SpawnObstacle(ObstacleSettings settings)
    {
        Obstacle obstacle = Instantiate(m_ObstaclePrefab, Vector3.zero, Quaternion.identity);
        obstacle.InitializeObstacle(settings);

        return obstacle;
    }
}



//private IEnumerator SpawnEnumerator()
//{
//    ObstacleGroup obstacleGroup = testGroup;

//    for (int repeteObstacle = 0; repeteObstacle < Mathf.Max(1, obstacleGroup.RepeteGroupTimes); repeteObstacle++)
//    {
//        int ringIndex = 0;

//        //Loop Rings
//        foreach (ObstacleRing ring in testGroup.Rings)
//        {
//            for (int repeteRing = 0; repeteRing < Mathf.Max(1, ring.RepeteRingTimes); repeteRing++)
//            {




//                for (int obstacleIndex = 0; obstacleIndex < ring.Obstacles.Length; obstacleIndex++)
//                {
//                    ObstacleSettings obstacle = ring.Obstacles[obstacleIndex];


//                    ObstacleSettings spawnObstacle = obstacle;
//                    spawnObstacle.MoveSpeed = m_SpawnSpeed;
//                    spawnObstacle.Distance = m_SpawnDistance - m_LastSpawnedDistanceOvershot;
//                    spawnObstacle.RotationAlpha += Mathf.Repeat(ring.RotateOffsetPerRing * ringIndex, 1f);
//                    spawnObstacle.EdgeSize += obstacleGroup.GlobalEdgeSize;

//                    m_LastSpawnedObstacle = SpawnObstacle(spawnObstacle);
//                }






//                // Wait to spawn next one
//                if (m_LastSpawnedObstacle)
//                {
//                    yield return new WaitUntil(() => (m_SpawnDistance - m_LastSpawnedObstacle.Distance) > m_LastSpawnedObstacle.EdgeSize);

//                    m_LastSpawnedDistanceOvershot = m_SpawnDistance - m_LastSpawnedObstacle.Distance - m_LastSpawnedObstacle.EdgeSize - (ring.Spacing + obstacleGroup.GlobalSpacing);
//                }





//                ringIndex++;



//            }
//        }

//    }


//}