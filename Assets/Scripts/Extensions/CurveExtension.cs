using UnityEngine;

public static class CurveExtension
{
    public static Vector3[] Bezier(Vector3 start, Vector3 startTangent, Vector3 endTangent, Vector3 end, int points)
    {
        var curvePoints = new Vector3[points];
        float fi = 0f;
        for (int i = 0; i < points; ++i, fi = i)
        {
            curvePoints[i] = GetPositionOnBezierCurve(start, startTangent, endTangent, end, fi / (points - 1));
        }

        return curvePoints;
    }

    public static Vector2[] Bezier2D(Vector2 start, Vector2 startTangent, Vector2 endTangent, Vector2 end, int points)
    {
        var curvePoints = new Vector2[points];
        float fi = 0f;
        for (int i = 0; i < points; ++i, fi = i)
        {
            curvePoints[i] = GetPositionOnBezierCurve(start, startTangent, endTangent, end, fi / (points - 1));
        }

        return curvePoints;
    }

    private static Vector3 GetPositionOnBezierCurve(Vector3 start, Vector3 startTangent, Vector3 endTangent, Vector3 end, float t)
    {
        // (1−t)3 P1 + 3(1−t)2 tP2 +3(1−t) t2 P3 + t3 P4
        //
        // Pow(1-t, 3) * start +
        // 3 * Pow(1-t, 2) * t * startTangent +
        // 3 * (1-t) * Pow(t, 2) * endTangent +
        // Pow(t, 3) * end

        return Mathf.Pow(1 - t, 3) * start +
               3 * Mathf.Pow(1 - t, 2) * t * startTangent +
               3 * (1 - t) * Mathf.Pow(t, 2) * endTangent +
               Mathf.Pow(t, 3) * end;
    }

    public static Vector2[] Circle2D(Vector2 pointA, Vector2 mirrorPointB, int points, float arcThreshold = 1f)
    {
        if (points <= 0) return new Vector2[0];

        int arcPoints = Mathf.FloorToInt(points * arcThreshold);
        float r = Vector2.Distance(pointA, mirrorPointB) / 2f;
        Vector2 center = pointA + (mirrorPointB - pointA).normalized * r;
        float angleStep = (360.0f / points) * Mathf.Deg2Rad;
        float startAngle = Vector2.SignedAngle(Vector2.right, (pointA - center).normalized) * Mathf.Deg2Rad;

        Vector2[] positions = new Vector2[arcPoints];
        Vector2 p;
        for (int i = 0; i < arcPoints; i++)
        {
            p.x = Mathf.Cos(angleStep * i + startAngle);
            p.y = Mathf.Sin(angleStep * i + startAngle);

            p *= r;
            p += center;

            positions[i] = p;
        }

        return positions;
    }
}
