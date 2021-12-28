using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleGroup", menuName = "ScriptableObjects/ObstacleGroup", order = 1)]
[Serializable]
public class ObstacleGroup : ScriptableObject
{
    // To add
    // Shrinking & Growing
    // Randomizing

    [FoldoutGroup("Settings")]
    [ShowInInspector]
    private string ObstacleName
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }

    }

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



#if UNITY_EDITOR
    private void BeginDrawListElement(int index)
    {
        Sirenix.Utilities.Editor.SirenixEditorGUI.BeginInlineBox();
    }

    private void EndDrawListElement(int index)
    {
        Sirenix.Utilities.Editor.SirenixEditorGUI.EndInlineBox();
    }
#endif
}


[Serializable]
public struct ObstacleSpawnSettings
{

    [CustomValueDrawer("DegreeAndAlpha")]
    public float FillAngle;

    [CustomValueDrawer("DegreeAndAlpha")]
    public float Rotation;


    public float EdgeWith;
    public float DistanceOffset;

    public float MoveSpeedMultiplier;

    [ToggleGroup("Scale")]
    public bool Scale;



    #region RepeteSettings

    [ToggleGroup("Repeat")]
    public bool Repeat;

    [Title("Around")]
    [Min(1)]
    [ToggleGroup("Repeat")]
    public int RepeatAroundTimes;

    [ToggleGroup("Repeat")]
    [CustomValueDrawer("DegreeAndAlpha")]
    public float RepeatAroundRotationOffset;


    [Title("Up")]


    [Min(1)]
    [ToggleGroup("Repeat")]
    public int RepeatUpTimes;


    [ToggleGroup("Repeat")]
    public float RepeatUpDistanceOffset;

    [ToggleGroup("Repeat")]
    [CustomValueDrawer("DegreeAndAlpha")]
    public float RepeatUpAroundRotationOffset;

    #endregion


#if UNITY_EDITOR
    private float DegreeAndAlpha(float value, GUIContent label)
    {
        if (ObstacleGroup.EditMode.Equals(EditModeType.Degrees))
        {
            //label.image = Sirenix.Utilities.Editor.EditorIcons.Rotate.Raw;
            return UnityEditor.EditorGUILayout.Slider(label, value, 0f, 360f);
        }
        else
        {
            //label.image = Sirenix.Utilities.Editor.EditorIcons.Minus.Raw;
            return UnityEditor.EditorGUILayout.Slider(label, value / 360f, 0f, 1f) * 360f;
        }
    }
#endif

}


public enum EditModeType
{
    Degrees,
    Alpha
}


























//using Sirenix.OdinInspector;
//using System;
//using UnityEditor;
//using UnityEngine;

//[CreateAssetMenu(fileName = "ObstacleGroup", menuName = "ScriptableObjects/ObstacleGroup", order = 1)]
//public class ObstacleGroup : ScriptableObject
//{

//    [ShowInInspector]
//    [EnumToggleButtons]
//    [FoldoutGroup("Settings")]
//    public static EditModeType EditMode;

//    [FoldoutGroup("Settings")]
//    public float GroupSize;

//    [Space(30f)]


//    [LabelText("Obstacles")]
//    [ListDrawerSettings(ShowItemCount = false, DraggableItems = false, OnBeginListElementGUI = "BeginDrawListElement", OnEndListElementGUI = "EndDrawListElement")]
//    public ObstacleSpawnSettings[] SpawnObstacles;

//#if UNITY_EDITOR
//    private void BeginDrawListElement(int index)
//    {
//        Sirenix.Utilities.Editor.SirenixEditorGUI.BeginInlineBox();
//    }

//    private void EndDrawListElement(int index)
//    {
//        Sirenix.Utilities.Editor.SirenixEditorGUI.EndInlineBox();
//    }
//#endif
//}


//[Serializable]
//public struct ObstacleSpawnSettings
//{

//    [HideInInspector]
//    private EditModeType m_EditMode { get { return ObstacleGroup.EditMode; } }


//    [Space]


//    [ShowIf("m_EditMode", EditModeType.Alpha)]
//    [CustomValueDrawer("FillAlphaDrawer")]
//    public float FillAlpha;


//    [ShowIf("m_EditMode", EditModeType.Angle)]
//    [CustomValueDrawer("FillAngleDrawer")]
//    [SerializeField]
//    private float FillAngle;

//    [ShowIf("m_EditMode", EditModeType.Alpha)]
//    [CustomValueDrawer("RotationDrawer")]
//    public float RotationAlpha;

//    [ShowIf("m_EditMode", EditModeType.Angle)]
//    [CustomValueDrawer("RotationAngleDrawer")]
//    [SerializeField]
//    private float RotationAngle;

//#if UNITY_EDITOR

//    private float FillAlphaDrawer(float value, GUIContent label)
//    {
//        FillAngle = Mathf.Clamp(value * 360f, 0f, 360f);
//        return EditorGUILayout.Slider(label, value, 0f, 1f);
//    }


//    private float FillAngleDrawer(float value, GUIContent label)
//    {
//        FillAlpha = Mathf.Clamp01(value / 360f);
//        return EditorGUILayout.Slider(label, value, 0f, 360f);
//    }


//    private float RotationDrawer(float value, GUIContent label)
//    {
//        RotationAngle = Mathf.Clamp(value * 360f, 0f, 360f);
//        return EditorGUILayout.Slider(label, value, 0f, 1f);
//    }

//    private float RotationAngleDrawer(float value, GUIContent label)
//    {
//        RotationAlpha = Mathf.Clamp01(value / 360f);
//        return EditorGUILayout.Slider(label, value, 0f, 360f);
//    }

//#endif

//    public float EdgeSize;

//    public float DistanceOffset;


//    [ToggleGroup("Repeat")]
//    public bool Repeat;

//    [Min(1)]
//    [ToggleGroup("Repeat")]
//    public int RepeatAroundTimes;

//    [ToggleGroup("Repeat")]
//    public float RepeatAroundOffset;

//    [Min(1)]
//    [ToggleGroup("Repeat")]
//    public int RepeatUpTimes;

//    [ToggleGroup("Repeat")]
//    public float RepeatUpOffset;

//    [ToggleGroup("Repeat")]
//    public float RepeatUpAroundOffset;

//    //[ToggleGroup("Repeat")]
//    //public OffsetMode OffsetMode;

//}


////public enum OffsetMode
////{
////    Add,
////    OverRepeatTimes
////}


//public enum EditModeType
//{
//    Angle,
//    Alpha
//}