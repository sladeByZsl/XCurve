using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "CurveData", menuName = "CreateCurveData")]
public class CurveData : ScriptableObject
{
    static CurveData _instance;
    public static CurveData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = AssetDatabase.LoadAssetAtPath<CurveData>("Assets/XCurve/Data/CurveData.asset");
            }
            return _instance;
        }
    }

    public List<BezierData> bezierData = new List<BezierData>();

}

[System.Serializable]
public class BezierData
{
    public string name;

    public Vector3[] data;
}

