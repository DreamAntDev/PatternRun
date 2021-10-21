using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private Transform[] points;


    void Start()
    {
        lr = GetComponent<LineRenderer>();   
    }

    public void SetUpLine(Transform[] points)
    {
        Debug.Log(points.Length);
        lr.positionCount = points.Length;
        this.points = points;
    }

    private void Update()
    {
        if (points != null)
        {
            for (int i = 0; i < points.Length; i++)
            {
                lr.SetPosition(i, points[i].position);
            }
        }
    }
}
