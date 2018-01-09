using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BezierCalculation {
    public static Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p3, Vector3 p2, double t)
    {
        Vector3 bezier;
        bezier.x = (float)(p0.x * Math.Pow(1 - t, 3) + 3 * p1.x * t * Math.Pow(1 - t, 2) + 3 * p2.x * t * t * (1 - t) + p3.x * t * t * t);
        bezier.y = (float)(p0.y * Math.Pow(1 - t, 3) + 3 * p1.y * t * Math.Pow(1 - t, 2) + 3 * p2.y * t * t * (1 - t) + p3.y * t * t * t);
        bezier.z = (float)(p0.z * Math.Pow(1 - t, 3) + 3 * p1.z * t * Math.Pow(1 - t, 2) + 3 * p2.z * t * t * (1 - t) + p3.z * t * t * t);
        return bezier;
    } 

}
