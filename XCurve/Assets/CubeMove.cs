using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WalkerMode
{
    Once,
    Loop,
    PingPong
}

public class CubeMove : MonoBehaviour
{
    private BezierData curve;

    public float duration;

    public bool lookForward;

    public WalkerMode mode;

    private float progress;
    private bool goingForward = true;

    private Vector3 srcPos;

    public void Start()
    {
        curve = CurveData.Instance.bezierData[0];
        srcPos = this.transform.localPosition;
    }

    private void Update()
    {
        if (goingForward)
        {
            progress += Time.deltaTime / duration;
            if (progress > 1f)
            {
                if (mode == WalkerMode.Once)
                {
                    progress = 1f;
                }
                else if (mode == WalkerMode.Loop)
                {
                    progress -= 1f;
                }
                else
                {
                    progress = 2f - progress;
                    goingForward = false;
                }
            }
        }
        else
        {
            progress -= Time.deltaTime / duration;
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
            }
        }
        Vector3 p = Bezier.GetPoint(curve.data[0], curve.data[1], curve.data[2], curve.data[3], progress);
        Vector3 position = p;
        transform.localPosition = srcPos+position;
        //if (lookForward)
        //{
        //    transform.LookAt(position + curve.GetDirection(progress));
        //}
    }
}
