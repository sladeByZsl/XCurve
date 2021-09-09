using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
    private const int lineSteps = 10;
    private const float directionScale = 0.5f;

    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI()
    {
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        Handles.color = Color.gray;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        //ShowDirections();
        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = curve.GetPoint(0f);
        Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
        for (int i = 1; i <= lineSteps; i++)
        {
            point = curve.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DrawSaveBtn();
    }

    private void DrawSaveBtn()
    {
        if (GUILayout.Button("保存为二进制文件"))
        {
            SavePathPrefab(Selection.activeGameObject);
            SaveBinData(Selection.activeGameObject);
            AssetDatabase.Refresh();
        }
    }

    //路径预设的保存目录
    const string c_pathPrefabsSaveDir = "Assets/XCurve/Prefab";
    //路径二进制文件保存目录
    const string c_pathBinSaveDir = "Assets/XCurve/Data";

    public void SavePathPrefab(GameObject obj)
    {
        var targetSaveDir = string.Format("{0}/{1}.prefab", c_pathPrefabsSaveDir, obj.name);

#if UNITY_5
        PrefabUtility.CreatePrefab(targetSaveDir, spline.gameObject, ReplacePrefabOptions.ConnectToPrefab);
#else
        PrefabUtility.SaveAsPrefabAssetAndConnect(obj, targetSaveDir, InteractionMode.AutomatedAction);
#endif
        AssetDatabase.SaveAssets();
    }

    public void SaveBinData(GameObject obj)
    {
        string name = obj.name;
        Vector3[] data = obj.GetComponent<BezierCurve>().points;
        List<BezierData> lst = CurveData.Instance.bezierData;
        bool contain = false;
        for (int i = 0; i < lst.Count; i++)
        {
            if (lst[i].name== name)
            {
                lst[i].data = data;
                contain = true;
            }
        }

        if(!contain)
        {
            BezierData bezierData = new BezierData();
            bezierData.name = name;
            bezierData.data = data;
            lst.Add(bezierData);
        }
    }
}
