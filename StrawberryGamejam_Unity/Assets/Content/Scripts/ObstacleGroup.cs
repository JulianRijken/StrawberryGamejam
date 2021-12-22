using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleGroup", menuName = "ScriptableObjects/ObstacleGroup", order = 1)]
public class ObstacleGroup : ScriptableObject
{
    public float GlobalSpacing;
    public float GlobalEdgeSize;
    public float GlobalRotationAlpha;

    [Header("LoopSettings")]
    [Min(1)] public int RepeteGroupTimes;

    [Header("")]
    public ObstacleRing[] Rings;
}

[System.Serializable]
public struct ObstacleRing
{
    [Header("")]
    public ObstacleSettings[] Obstacles;
    [Header("RingSettings")]
    public float Spacing;

    [Header("LoopSettings")]
    [Min(1)] public int RepeteRingTimes;
    public float RotateOffsetPerRing;

}