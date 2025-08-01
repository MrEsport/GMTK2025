using UnityEngine;

public static class DebugExtension
{
    /// <summary>
    /// Draw a 12-segments circle of <paramref name="radius"/> centered around <paramref name="position"/>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    public static void DrawCircle(Vector3 position, float radius) => DrawCircle(position, radius, 12, Color.white, 1f);
    /// <summary>
    /// Draw a 12-segments circle of <paramref name="radius"/> centered around <paramref name="position"/>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    /// <param name="segments"></param>
    public static void DrawCircle(Vector3 position, float radius, int segments) => DrawCircle(position, radius, segments, Color.white, 1f);
    /// <summary>
    /// Draw a segmented circle of <paramref name="radius"/> centered around <paramref name="position"/>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    /// <param name="segments"></param>
    public static void DrawCircle(Vector3 position, float radius, int segments, Color color) => DrawCircle(position, radius, segments, color, 1f);
    /// <summary>
    /// Draw a segmented circle of <paramref name="radius"/> centered around <paramref name="position"/>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    /// <param name="segments"></param>
    public static void DrawCircle(Vector3 position, float radius, int segments, Color color, float duration)
    {
        if (radius <= 0.0f || segments <= 0) return;

        float angleStep = (360.0f / segments) * Mathf.Deg2Rad;

        Vector3 lineStart = Vector3.zero;
        Vector3 lineEnd = Vector3.zero;

        for (int i = 0; i < segments; i++)
        {
            lineStart.x = Mathf.Cos(angleStep * i);
            lineStart.y = Mathf.Sin(angleStep * i);

            lineEnd.x = Mathf.Cos(angleStep * (i + 1));
            lineEnd.y = Mathf.Sin(angleStep * (i + 1));

            lineStart *= radius;
            lineEnd *= radius;

            lineStart += position;
            lineEnd += position;

            Debug.DrawLine(lineStart, lineEnd, color, duration);
        }
    }

    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/>
    /// </summary>
    /// <param name="position">center point</param>
    public static void DrawCursor(Vector3 position, Color color) => DrawCursor(position, .66f, color, 1f);
    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/>
    /// </summary>
    /// <param name="position">center point</param>
    public static void DrawCursor(Vector3 position, Color color, float duration) => DrawCursor(position, .66f, color, duration);
    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/>
    /// </summary>
    /// <param name="position">center point</param>
    /// <param name="radius">line length</param>
    public static void DrawCursor(Vector3 position, float radius, Color color) => DrawCursor(position, radius, color, 1f);
    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/>
    /// </summary>
    /// <param name="position">center point</param>
    /// <param name="radius">line length</param>
    public static void DrawCursor(Vector3 position, float radius, Color color, float duration)
    {
        Debug.DrawRay(position + Vector3.left * radius / 2f, Vector3.right * radius, color, duration);
        Debug.DrawRay(position + Vector3.down * radius / 2f, Vector3.up * radius, color, duration);
        Debug.DrawRay(position + Vector3.back * radius / 2f, Vector3.forward * radius, color, duration);
    }

    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/>
    /// </summary>
    /// <param name="position">center point</param>
    public static void DrawCursor(Vector3 position) => DrawCursor(position, .66f, Color.red, Color.green, Color.blue, 1f);
    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/>
    /// </summary>
    /// <param name="position">center point</param>
    /// <param name="radius">line length</param>
    public static void DrawCursor(Vector3 position, float radius) => DrawCursor(position, radius, Color.red, Color.green, Color.blue, 1f);
    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/>
    /// </summary>
    /// <param name="position">center point</param>
    /// <param name="radius">line length</param>
    public static void DrawCursor(Vector3 position, float radius, float duration) => DrawCursor(position, radius, Color.red, Color.green, Color.blue, duration);
    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/> of different colors
    /// </summary>
    /// <param name="position">center point</param>
    /// <param name="x">X axis color</param>
    /// <param name="y">Y axis color</param>
    /// <param name="z">Z axis color</param>
    public static void DrawCursor(Vector3 position, Color x, Color y, Color z) => DrawCursor(position, .66f, x, y, z, 1f);
    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/> of different colors
    /// </summary>
    /// <param name="position">center point</param>
    /// <param name="x">X axis color</param>
    /// <param name="y">Y axis color</param>
    /// <param name="z">Z axis color</param>
    public static void DrawCursor(Vector3 position, Color x, Color y, Color z, float duration) => DrawCursor(position, .66f, x, y, z, duration);
    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/> of different colors
    /// </summary>
    /// <param name="position">center point</param>
    /// <param name="radius">line length</param>
    /// <param name="x">X axis color</param>
    /// <param name="y">Y axis color</param>
    /// <param name="z">Z axis color</param>
    public static void DrawCursor(Vector3 position, float radius, Color x, Color y, Color z) => DrawCursor(position, radius, x, y, z, 1f);
    /// <summary>
    /// Draw 3 lines crossing at <paramref name="position"/> of different colors
    /// </summary>
    /// <param name="position">center point</param>
    /// <param name="radius">line length</param>
    /// <param name="x">X axis color</param>
    /// <param name="y">Y axis color</param>
    /// <param name="z">Z axis color</param>
    public static void DrawCursor(Vector3 position, float radius, Color x, Color y, Color z, float duration)
    {
        Debug.DrawRay(position + Vector3.back * radius / 2f, Vector3.forward * radius, z, duration);
        Debug.DrawRay(position + Vector3.down * radius / 2f, Vector3.up * radius, y, duration);
        Debug.DrawRay(position + Vector3.left * radius / 2f, Vector3.right * radius, x, duration);
    }
}
