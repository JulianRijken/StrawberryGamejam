using UnityEngine;

[System.Serializable]
public struct HalfCircleSettings
{

    public float EdgeSize;
    [Range(0f, 1f)] public float FillAlpha;
    [Range(0f, 1f)] public float RotationAlpha;
}
