using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Obstacle m_ObstaclePrefab;
    [SerializeField] private ObstacleGroup[] m_TestObstacles;
    [SerializeField] private ObstacleGroup m_TestGroup;

    [SerializeField] private float m_SpawnDistance = 100f;
    [SerializeField] private float m_SpawnSpeed = 50f;
    [SerializeField] private float m_GroupSpace = 30f;

    [SerializeField] private int m_targetFPS = 300;


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
        ObstacleGroup spawnGroup = m_TestGroup ? m_TestGroup : m_TestObstacles[Random.Range(0,m_TestObstacles.Length)];

        while (true)
        {
            SpawnObstacleGroup(spawnGroup);
            
            // Temp
            yield return new WaitForSeconds((spawnGroup.GroupSize + m_GroupSpace) / m_SpawnSpeed);
        }
    }

    private void SpawnObstacleGroup(ObstacleGroup obstacleGroup)
    {
        // Spawn Obstacle \\
        foreach (ObstacleSpawnSettings s in obstacleGroup.SpawnObstacles)
        {
            for (int RepeatUpIndex = 0; RepeatUpIndex < (s.Repeat ? Mathf.Max(1, s.RepeatUpTimes) : 1); RepeatUpIndex++)
            {
                for (int RepeatAroundIndex = 0; RepeatAroundIndex < (s.Repeat ? Mathf.Max(1, s.RepeatAroundTimes) : 1); RepeatAroundIndex++)
                {

                    // Spawn Obstacle \\
                    Obstacle obstacle = Instantiate(m_ObstaclePrefab);

                    obstacle.MoveSpeed = m_SpawnSpeed;
                    obstacle.Distance = m_SpawnDistance + s.DistanceOffset + (RepeatUpIndex * s.RepeatUpOffset);
                    obstacle.EdgeSize = s.EdgeSize;
                    obstacle.FillAlpha = s.FillAlpha;
                    obstacle.RotationAlpha = s.RotationAlpha + (RepeatAroundIndex * s.RepeatAroundOffset) + (RepeatUpIndex * s.RepeatUpAroundOffset);
                }
            }

        }
    }

}






//    private Obstacle m_LastSpawnedObstacle;
//private float m_DistanceOvershot;


//ObstacleGroup obstacleGroup = testGroup;

//// Repete Obstacle \\
//for (int obstacleRepeteIndex = 0; obstacleRepeteIndex < Mathf.Max(1, obstacleGroup.loopSettings.RepeteTimes); obstacleRepeteIndex++)
//{

//    // Ring Spawn Loop \\
//    for (int ringIndex = 0; ringIndex < testGroup.Rings.Length; ringIndex++)
//    {
//        ObstacleRing ring = testGroup.Rings[ringIndex];

//        // Repete Ring \\
//        for (int ringRepeteIndex = 0; ringRepeteIndex < Mathf.Max(1, ring.loopSettings.RepeteTimes); ringRepeteIndex++)
//        {



//            // Obstacle Spawn Loop \\
//            for (int obstacleIndex = 0; obstacleIndex < ring.Obstacles.Length; obstacleIndex++)
//            {

//                ObstacleSettings spawnObstacle = ring.Obstacles[obstacleIndex];

//                // Set move speed
//                spawnObstacle.MoveSpeed = m_SpawnSpeed;

//                // Set distance
//                spawnObstacle.Distance = (m_SpawnDistance + spawnObstacle.EdgeSize + obstacleGroup.GlobalEdgeSize + ring.Spacing + obstacleGroup.GlobalSpacing) - m_DistanceOvershot;

//                // Set rotation
//                spawnObstacle.RotationAlpha += ring.loopSettings.GetRotateAlpha(ringRepeteIndex);

//                // Set edgeSize
//                spawnObstacle.EdgeSize += obstacleGroup.GlobalEdgeSize;


//                // Spawn Obstacle \\
//                m_LastSpawnedObstacle = SpawnObstacle(spawnObstacle);
//            }




//            // Ring Spawn Delay \\
//            if (m_LastSpawnedObstacle)
//            {
//                // Compensate for lack of frames
//                if (m_LastSpawnedObstacle.Distance > m_SpawnDistance)
//                {
//                    yield return new WaitUntil(() => m_LastSpawnedObstacle.Distance < m_SpawnDistance);
//                    m_DistanceOvershot = m_SpawnDistance - m_LastSpawnedObstacle.Distance;
//                }
//            }


//        }
//    }

//}