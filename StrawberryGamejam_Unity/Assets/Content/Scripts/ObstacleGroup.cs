using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleGroup", menuName = "ScriptableObjects/ObstacleGroup", order = 1)]
public class ObstacleGroup : ScriptableObject
{

    [Title("Settings")]

    [ShowInInspector]
    [EnumToggleButtons]
    public static EditModeType EditMode;

    public float GroupSize;

    [Space(30f)]

    [Title("Obstacles")]

    [LabelText("Obstacles")]
    [ListDrawerSettings(ShowItemCount = false, DraggableItems = false, OnBeginListElementGUI = "BeginDrawListElement", OnEndListElementGUI = "EndDrawListElement")]
    public SpawnObstacle[] SpawnObstacles;

    private void BeginDrawListElement(int index)
    {
        SirenixEditorGUI.BeginInlineBox();
    }

    private void EndDrawListElement(int index)
    {
        SirenixEditorGUI.EndInlineBox();
    }
}


[Serializable]
public struct SpawnObstacle
{
    public ObstacleSettings ObstacleSettings;

    public float DistanceOffset;

    [ToggleGroup("Repete")]
    public bool Repete;

    [ToggleGroup("Repete")]
    public RepeteSettings RepeteSettings;

}


[Serializable]
[HideLabel]
public struct RepeteSettings
{   
    [Min(1)] 
    public int RepeteTimes;

    public float RepeteOffset;

    public OffsetMode OffsetMode;

    //public float GetRotateAlpha(int _repeteIndex)
    //{
    //    if (RepeteOffset == 0)
    //        return 0f;

    //    float rotationOffset = OffsetMode.Equals(OffsetMode.Add) ? RepeteOffset * _repeteIndex : (_repeteIndex / (float)RepeteTimes) * RepeteOffset;
    //    return Mathf.Repeat(rotationOffset, 1f);
    //}
}

[Serializable]
public enum OffsetMode
{
    Add,
    OverRepeteTimes
}


[Serializable]
[HideLabel]
public struct ObstacleSettings
{
    [HideInInspector]
    private EditModeType m_EditMode { get { return ObstacleGroup.EditMode; } }


    [Space]


    [ShowIf("m_EditMode", EditModeType.Alpha)]
    [CustomValueDrawer("FillAlphaDrawer")]
    public float FillAlpha;
    private float FillAlphaDrawer(float value, GUIContent lable)
    {
        FillAngle = Mathf.Clamp(value * 360f,0f,360f);
        return EditorGUILayout.Slider(lable, value, 0f, 1f);
    }



    [ShowIf("m_EditMode", EditModeType.Angle)]
    [CustomValueDrawer("FillAngleDrawer")]
    public float FillAngle;
    private float FillAngleDrawer(float value, GUIContent lable)
    {
        FillAlpha = Mathf.Clamp01(value / 360f);
        return EditorGUILayout.Slider(lable, value, 0f, 360f);
    }







    [ShowIf("m_EditMode", EditModeType.Alpha)]
    [CustomValueDrawer("RotationDrawer")]
    public float RotationAlpha;
    private float RotationDrawer(float value, GUIContent lable)
    {
        RotationAngle = Mathf.Clamp(value * 360f, 0f, 360f);
        return EditorGUILayout.Slider(lable, value, 0f, 1f);
    }

    [ShowIf("m_EditMode", EditModeType.Angle)]
    [CustomValueDrawer("RotatioAngleDrawer")]
    public float RotationAngle;
    private float RotatioAngleDrawer(float value, GUIContent lable)
    {
        RotationAlpha = Mathf.Clamp01(value / 360f);
        return EditorGUILayout.Slider(lable, value, 0f, 360f);
    }


    public float EdgeSize;


    [HideInInspector] public float MoveSpeed;
    [HideInInspector] public float Distance;



}

public enum EditModeType
{
    Angle,
    Alpha
}