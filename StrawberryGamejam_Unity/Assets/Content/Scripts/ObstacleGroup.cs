using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleGroup", menuName = "ScriptableObjects/ObstacleGroup", order = 1)]
public class ObstacleGroup : ScriptableObject
{

    [ShowInInspector]
    [EnumToggleButtons]
    [FoldoutGroup("Settings")]
    public static EditModeType EditMode;

    [FoldoutGroup("Settings")]
    public float GroupSize;

    [Space(30f)]


    [LabelText("Obstacles")]
    [ListDrawerSettings(ShowItemCount = false, DraggableItems = false, OnBeginListElementGUI = "BeginDrawListElement", OnEndListElementGUI = "EndDrawListElement")]
    public ObstacleSpawnSettings[] SpawnObstacles;

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
public struct ObstacleSpawnSettings
{

    [HideInInspector]
    private EditModeType m_EditMode { get { return ObstacleGroup.EditMode; } }


    [Space]


    [ShowIf("m_EditMode", EditModeType.Alpha)]
    [CustomValueDrawer("FillAlphaDrawer")]
    public float FillAlpha;
    private float FillAlphaDrawer(float value, GUIContent lable)
    {
        FillAngle = Mathf.Clamp(value * 360f, 0f, 360f);
        return EditorGUILayout.Slider(lable, value, 0f, 1f);
    }



    [ShowIf("m_EditMode", EditModeType.Angle)]
    [CustomValueDrawer("FillAngleDrawer")]
    [SerializeField]
    private float FillAngle;
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
    [SerializeField]
    private float RotationAngle;
    private float RotatioAngleDrawer(float value, GUIContent lable)
    {
        RotationAlpha = Mathf.Clamp01(value / 360f);
        return EditorGUILayout.Slider(lable, value, 0f, 360f);
    }


    public float EdgeSize;

    public float DistanceOffset;
    

    [ToggleGroup("Repeat")]
    public bool Repeat;

    [Min(2)]
    [ToggleGroup("Repeat")]
    public int RepeatAroundTimes;

    [ToggleGroup("Repeat")]
    public float RepeatAroundOffset;

    [Min(2)]
    [ToggleGroup("Repeat")]
    public int RepeatUpTimes;

    [ToggleGroup("Repeat")]
    public float RepeatUpOffset;

    [ToggleGroup("Repeat")]
    public float RepeatUpAroundOffset;

    //[ToggleGroup("Repeat")]
    //public OffsetMode OffsetMode;

}


//public enum OffsetMode
//{
//    Add,
//    OverRepeatTimes
//}


public enum EditModeType
{
    Angle,
    Alpha
}