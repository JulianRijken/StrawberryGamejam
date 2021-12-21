using UnityEngine;

[System.Serializable]
public struct ObstacleSettings
{
    public float EdgeSize;
    [Range(0f, 1f)] public float FillAlpha;
    [Range(0f, 1f)] public float RotationAlpha;

    [HideInInspector] public float MoveSpeed;
    [HideInInspector] public float Distance;
}
