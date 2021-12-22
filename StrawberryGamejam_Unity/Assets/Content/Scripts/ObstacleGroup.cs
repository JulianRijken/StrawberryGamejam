using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleGroup", menuName = "ScriptableObjects/ObstacleGroup", order = 1)]
public class ObstacleGroup : ScriptableObject
{
    public float GlobalSpacing;
    public float GlobalEdgeSize;
    public float GlobalRotationAlpha;

    public LoopSettings loopSettings;

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

    public LoopSettings loopSettings;
}

[System.Serializable]
public struct LoopSettings
{
    [Min(1)] public int RepeteTimes;
    public float RepeteOffset;
    public OffsetMode OffsetMode;

    public float GetRotateAlpha(int _repeteIndex)
    {
        if (RepeteOffset == 0)
            return 0f;

        float rotationOffset = OffsetMode.Equals(OffsetMode.Add) ? RepeteOffset * _repeteIndex : (_repeteIndex / (float)RepeteTimes) * RepeteOffset;
        return Mathf.Repeat(rotationOffset, 1f);
    }
}

[System.Serializable]
public enum OffsetMode
{
    Add,
    OverRepeteTimes
}