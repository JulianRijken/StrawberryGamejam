using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTrail : MonoBehaviour
{

    [Header("Trail Settings")]
    [SerializeField] private float m_SpawnDelay;
    [SerializeField] private int m_MaxPoints;

    [SerializeField] private bool m_DebugTrail;

    private List<Vector2> m_PathPoints = new List<Vector2>();


    private void Awake()
    {
        StartCoroutine(AddPointTick());
    }

    private void OnDrawGizmos()
    {
        if (!m_DebugTrail)
            return;

        if (m_PathPoints.Count <= 0)
            return;


        List<Vector2> pathPoints = new List<Vector2>(m_PathPoints);
        pathPoints.Add(transform.position);


        List<PathPointInfo> pathPointsInfo = new List<PathPointInfo>();

        // Get all path points and info
        float totalDistance = 0f;
        for (int i = 0; i < pathPoints.Count; i++)
        {

            PathPointInfo pathPointInfo = new PathPointInfo();
            pathPointInfo.Point = pathPoints[i];

            // If next point is out of range
            if (i + 1 < pathPoints.Count)
            {
                // Get distance
                float distanceToNextPoint = Vector2.Distance(pathPoints[i], pathPoints[i + 1]);

                // Add distance to total
                totalDistance += distanceToNextPoint;

                pathPointInfo.bFinalPoint = false;
                pathPointInfo.NextPoint = pathPoints[i + 1];
                pathPointInfo.DistanceToNextPoint = distanceToNextPoint;

            }
            else
            {
                pathPointInfo.bFinalPoint = true;
            }

            pathPointsInfo.Add(pathPointInfo);
        }




        // Debug Draw Line
        for (int i = 0; i < pathPointsInfo.Count; i++)
        {
            Gizmos.DrawSphere(pathPointsInfo[i].Point, 0.2f);

            if (!pathPointsInfo[i].bFinalPoint)
                Gizmos.DrawLine(pathPointsInfo[i].Point, pathPointsInfo[i].NextPoint);
        }
    }


    private IEnumerator AddPointTick()
    {
        while(true)
        {
            AddPoint();
            yield return new WaitForSeconds(m_SpawnDelay);
        }
    }

    private void AddPoint()
    {
        m_PathPoints.Add(transform.position);

        // Remove if to long
        if (m_PathPoints.Count > m_MaxPoints)
        {
            m_PathPoints.RemoveAt(0);
        }
    }


    public Vector2 GetPoint(float pathAlpha)
    {
        if(m_PathPoints.Count < 1)
            return transform.position;


        List<Vector2> pathPoints = new List<Vector2>(m_PathPoints);
        pathPoints.Add(transform.position);


        List<PathPointInfo> pathPointsInfo = new List<PathPointInfo>();

        // Get all path points and info
        float totalDistance = 0f;
        for (int i = 0; i < pathPoints.Count; i++)
        {

            PathPointInfo pathPointInfo = new PathPointInfo();
            pathPointInfo.Point = pathPoints[i];

            // If next point is out of range
            if (i + 1 < pathPoints.Count)
            {
                // Get distance
                float distanceToNextPoint = Vector2.Distance(pathPoints[i], pathPoints[i + 1]);

                // Add distance to total
                totalDistance += distanceToNextPoint;

                pathPointInfo.bFinalPoint = false;
                pathPointInfo.NextPoint = pathPoints[i + 1];
                pathPointInfo.DistanceToNextPoint = distanceToNextPoint;
            }
            else
            {
                pathPointInfo.bFinalPoint = true;
            }

            pathPointsInfo.Add(pathPointInfo);
        }

        if (totalDistance <= 0)
            return transform.position;


        float getDistance = totalDistance * Mathf.Abs(pathAlpha - 1);
        float currentDistance = 0f;

        for (int i = 0; i < pathPointsInfo.Count; i++)
        {
            currentDistance += pathPointsInfo[i].DistanceToNextPoint;

            if (currentDistance >= getDistance)
            {
                float rest = currentDistance - getDistance;
                float alpha = rest / pathPointsInfo[i].DistanceToNextPoint;

                if (pathPointsInfo[i].NextPoint == pathPointsInfo[i].Point)
                    return pathPointsInfo[i].Point;
 

                return Vector2.Lerp(pathPointsInfo[i].NextPoint, pathPointsInfo[i].Point, alpha);
            }
        }

        return transform.position;
    }


    private struct PathPointInfo
    {
        public bool bFinalPoint;
        public Vector2 Point;
        public Vector2 NextPoint;
        public float DistanceToNextPoint;
    }
}