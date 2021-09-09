using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurveData))]
public class CurveDataInspector : Editor
{
    private CurveData config;
    private SerializedProperty bezierData = null;
    FieldInfo[] propertyInfos;
    void OnEnable()
    {
        config = (CurveData)target;
        propertyInfos = config.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly); // 获取一个类的所有属性
        bezierData = serializedObject.FindProperty("bezierData");
    }

    public override void OnInspectorGUI()
    {
        // 设置整个界面是以垂直方向来布局
        EditorGUILayout.BeginVertical();


        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(bezierData, true);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.EndHorizontal();
        EditorUtility.SetDirty(config);
    }
}
