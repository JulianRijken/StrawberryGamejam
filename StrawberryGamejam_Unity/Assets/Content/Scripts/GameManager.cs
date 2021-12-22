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
    private float m_DistanceOvershot;


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

                        // Set move speed
                        spawnObstacle.MoveSpeed = m_SpawnSpeed;

                        // Set distance
                        spawnObstacle.Distance = (m_SpawnDistance + spawnObstacle.EdgeSize + obstacleGroup.GlobalEdgeSize + ring.Spacing + obstacleGroup.GlobalSpacing) - m_DistanceOvershot;

                        // Set rotation
                        spawnObstacle.RotationAlpha += ring.loopSettings.GetRotateAlpha(ringRepeteIndex);

                        // Set edgeSize
                        spawnObstacle.EdgeSize += obstacleGroup.GlobalEdgeSize;


                        // Spawn Obstacle \\
                        m_LastSpawnedObstacle = SpawnObstacle(spawnObstacle);
                    }




                    // Ring Spawn Delay \\
                    if (m_LastSpawnedObstacle)
                    {
                        // Compensate for lack of frames
                        if (m_LastSpawnedObstacle.Distance > m_SpawnDistance)
                        {
                            yield return new WaitUntil(() => m_LastSpawnedObstacle.Distance < m_SpawnDistance);
                            m_DistanceOvershot = m_SpawnDistance - m_LastSpawnedObstacle.Distance;
                        }
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

//    // Repete Obstacle \\
//    for (int obstacleRepeteIndex = 0; obstacleRepeteIndex < Mathf.Max(1, obstacleGroup.loopSettings.RepeteTimes); obstacleRepeteIndex++)
//    {

//        // Ring Spawn Loop \\
//        for (int ringIndex = 0; ringIndex < testGroup.Rings.Length; ringIndex++)
//        {
//            ObstacleRing ring = testGroup.Rings[ringIndex];

//            // Repete Ring \\
//            for (int ringRepeteIndex = 0; ringRepeteIndex < Mathf.Max(1, ring.loopSettings.RepeteTimes); ringRepeteIndex++)
//            {



//                // Obstacle Spawn Loop \\
//                for (int obstacleIndex = 0; obstacleIndex < ring.Obstacles.Length; obstacleIndex++)
//                {

//                    ObstacleSettings spawnObstacle = ring.Obstacles[obstacleIndex];
//                    spawnObstacle.MoveSpeed = m_SpawnSpeed;
//                    spawnObstacle.Distance = (m_SpawnDistance - m_LastSpawnedDistanceOvershot) + (spawnObstacle.EdgeSize + obstacleGroup.GlobalEdgeSize);

//                    spawnObstacle.RotationAlpha += ring.loopSettings.GetRotateAlpha(ringRepeteIndex);

//                    spawnObstacle.EdgeSize += obstacleGroup.GlobalEdgeSize;

//                    m_LastSpawnedObstacle = SpawnObstacle(spawnObstacle);
//                }




//                // Ring Spawn Delay \\
//                if (m_LastSpawnedObstacle)
//                {
//                    yield return new WaitUntil(() => (m_SpawnDistance - m_LastSpawnedObstacle.Distance) > m_LastSpawnedObstacle.EdgeSize);
//                    m_LastSpawnedDistanceOvershot = (m_SpawnDistance - m_LastSpawnedObstacle.Distance) - (ring.Spacing + obstacleGroup.GlobalSpacing);
//                }


//            }
//        }

//    }
//}