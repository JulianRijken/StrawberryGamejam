using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleGroup", menuName = "ScriptableObjects/ObstacleGroup", order = 1)]
public class ObstacleGroup : ScriptableObject
{
    public float GlobalSpacing;
    public float GlobalEdgeSize;
    public float GlobalRotationAlpha;
    public ObstacleRing[] obstacleRings;
    public int RepeteTimes;
}

[System.Serializable]
public struct ObstacleRing
{
    public ObstacleSettings[] ObstacleSettings;
    public float Spacing;
    public int RepeteTimes;

}