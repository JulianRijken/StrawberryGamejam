using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private Obstacle m_ObstaclePrefab;
    [SerializeField] private int m_DefaultAmmountToPool;

    [SerializeField] private float m_SpawnDistance = 100f;
    [SerializeField] private float m_GlobalMoveSpeed = 50f;
    [SerializeField] private float Rate = 1f;

    public float GlobalMoveSpeed => m_GlobalMoveSpeed;


    public List<Obstacle> ActiveObstacles { get; private set; }
    public List<Obstacle> InactiveObstacles { get; private set; }

    //[SerializeField] private ObstacleGroup[] m_TestObstacles;
    [SerializeField] private ObstacleGroup m_TestGroup;


    private void Awake()
    {
        ActiveObstacles = new List<Obstacle>();
        InactiveObstacles = new List<Obstacle>();

        AddStandardPoolObjects();
        SpawnObstacleGroup(m_TestGroup);
    }


    private void Update()
    {

        UpdateObstacles();
    }

    //private IEnumerator Testing()
    //{
    //    while (true)
    //    {
    //        SpawnObstacleGroup(m_TestGroup);
    //        yield return new WaitForSeconds(Rate);
    //    }
    //}



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
                    Obstacle obstacle = GetNewObstacle();
                    obstacle.MoveSpeedMultiplier = 1f; //s.MoveSpeedMultiplier;
                    obstacle.Distance = m_SpawnDistance + s.DistanceOffset + (RepeatUpIndex * s.RepeatUpDistanceOffset);
                    obstacle.EdgeWith = s.EdgeWith;
                    obstacle.FillAngle = s.FillAngle;
                    obstacle.Rotation = s.Rotation + (RepeatAroundIndex * s.RepeatAroundRotationOffset) + (RepeatUpIndex * s.RepeatUpAroundRotationOffset);
                }
            }
        }
    }

    private void UpdateObstacles()
    {
        for (int i = 0; i < ActiveObstacles.Count; i++)
        {
            // Move obstacle every frame
            ActiveObstacles[i].Distance += -m_GlobalMoveSpeed * ActiveObstacles[i].MoveSpeedMultiplier * Time.deltaTime;

            // Destroy when under 0
            if (ActiveObstacles[i].Distance <= 0)
            {
                HideObstacle(ActiveObstacles[i]);
                return;
            }
        }    
    }




    #region FileSaving

    private void SaveObstacleGroup()
    {
        string groupJson = JsonConvert.SerializeObject(m_TestGroup, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/{m_TestGroup.name}.json", groupJson);

        AssetDatabase.Refresh();
        Debug.Log(Application.dataPath);
    }

    #endregion


    #region ObstaclePool

    private void AddStandardPoolObjects()
    {
        for (int i = 0; i < m_DefaultAmmountToPool; i++)
        {
            Obstacle newObstacle = GetNewObstacle();
            newObstacle.gameObject.SetActive(false);
        }
    }


    public Obstacle GetNewObstacle()
    {
        return GetNewObstacle(Vector3.zero, Quaternion.identity);
    }

    public Obstacle GetNewObstacle(Vector3 position, Quaternion rotation)
    {
        Obstacle spawnedObject;

        // If the inactive list has more then 0, grab the object from the inactiveList
        if (InactiveObstacles.Count > 0)
        {
            spawnedObject = InactiveObstacles[0];

            InactiveObstacles.Remove(spawnedObject);
            ActiveObstacles.Add(spawnedObject);

            spawnedObject.transform.position = position;
            spawnedObject.transform.rotation = rotation;
        }
        else
        {
            spawnedObject = Instantiate(m_ObstaclePrefab, position, rotation, transform);
            ActiveObstacles.Add(spawnedObject);
        }

        spawnedObject.gameObject.SetActive(true);

        return spawnedObject;
    }


    public void HideObstacle(Obstacle obstacle)
    {
        ActiveObstacles.Remove(obstacle);
        InactiveObstacles.Add(obstacle);
        obstacle.gameObject.SetActive(false);
    }

    public void HideAllActiveObstacles()
    {
        Obstacle[] obstaclesToHide = ActiveObstacles.ToArray();
        for (int i = 0; i < obstaclesToHide.Length; i++)
        {
            HideObstacle(obstaclesToHide[i]);
        }
    }

    #endregion

}










// OLD 1 \\


//private IEnumerator SpawnEnumerator()
//{
//    while (true)
//    {
//ObstacleGroup spawnGroup = m_TestGroup ? m_TestGroup : m_TestObstacles[Random.Range(0, m_TestObstacles.Length)];

//SpawnObstacleGroup(spawnGroup);

//yield return new WaitForSeconds((spawnGroup.GroupSize + m_GroupSpace) / m_SpawnSpeed);
//}
//}





//private void SpawnObstacleGroup(ObstacleGroup obstacleGroup)
//{
//    // Spawn Obstacle \\
//    foreach (ObstacleSpawnSettings s in obstacleGroup.SpawnObstacles)
//    {
//        for (int RepeatUpIndex = 0; RepeatUpIndex < (s.Repeat ? Mathf.Max(1, s.RepeatUpTimes) : 1); RepeatUpIndex++)
//        {
//            for (int RepeatAroundIndex = 0; RepeatAroundIndex < (s.Repeat ? Mathf.Max(1, s.RepeatAroundTimes) : 1); RepeatAroundIndex++)
//            {

//                // Spawn Obstacle \\
//                Obstacle obstacle = Instantiate(m_ObstaclePrefab);

//                obstacle.MoveSpeed = m_SpawnSpeed;
//                obstacle.Distance = m_SpawnDistance + s.DistanceOffset + (RepeatUpIndex * s.RepeatUpDistanceOffset);
//                obstacle.EdgeWith = s.EdgeWith;
//                obstacle.FillAngle = s.FillAngle;
//                obstacle.Rotation = s.Rotation + (RepeatAroundIndex * s.RepeatAroundRotationOffset) + (RepeatUpIndex * s.RepeatUpAroundRotationOffset);
//            }
//        }

//    }
//}















// OLD 2 \\


//private Obstacle m_LastSpawnedObstacle;
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