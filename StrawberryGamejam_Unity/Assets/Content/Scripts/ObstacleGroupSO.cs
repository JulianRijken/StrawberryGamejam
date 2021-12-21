using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleGroup", menuName = "ScriptableObjects/ObstacleGroup", order = 1)]
public class ObstacleGroupSO : ScriptableObject
{
    public float GlobalSpacing;
    public float GlobalEdgeWith;
}
