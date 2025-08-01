using UnityEngine;

public static class RandomExtension
{
    /// <summary>
    /// Returns a random point inside a circle of Radius <paramref name="radius"/>, around <paramref name="center"/>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector2 PointInsideCircle(Vector2 center, float radius)
    {
        return center + Random.insideUnitCircle * radius;
    }

    /// <summary>
    /// Returns a random point inside a circle of Max Radius <paramref name="radius"/> at least <paramref name="innerRadius"/> distance away from its <paramref name="center"/>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="innerRadius"></param>
    /// <returns></returns>
    public static Vector2 PointInsideCircle(Vector2 center, float radius, float innerRadius)
    {
        float distanceToCenter = Random.Range(innerRadius, radius);
        return center + Random.insideUnitCircle.normalized * distanceToCenter;
    }

    /// <summary>
    /// Random point on a circle's surface
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector2 PointOnCircle(Vector2 center, float radius)
    {
        return center + Random.insideUnitCircle.normalized * radius;
    }

    /// <summary>
    /// Returns a random point in a box between its <paramref name="bottomLeft"/> and <paramref name="topRight"/> corners
    /// </summary>
    /// <param name="bottomLeft">Lower Left box corner position</param>
    /// <param name="topRight">Upper Right box corner position</param>
    /// <returns></returns>
    public static Vector2 PointInsideBox(Vector2 bottomLeft, Vector2 topRight)
    {
        return new Vector2(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y));
    }

    /// <summary>
    /// Returns a random point in a box of <paramref name="size"/>, around <paramref name="center"/>
    /// </summary>
    /// <param name="center">Center position</param>
    /// <param name="size">Box size</param>
    /// <returns></returns>
    public static Vector2 PointInsideBox(Vector2 center, float size)
    {
        Vector2 halfBox = Vector2.one * (size / 2f);
        return PointInsideBox(center - halfBox, center + halfBox);
    }

    /// <summary>
    /// Returns a random point in a box of size (<paramref name="width"/>, <paramref name="height"/>), around <paramref name="center"/>
    /// </summary>
    /// <param name="center">Center position</param>
    /// <param name="width">Box width</param>
    /// <param name="height">Box height</param>
    /// <returns></returns>
    public static Vector2 PointInsideBox(Vector2 center, float width, float height)
    {
        Vector2 halfBox = new Vector2(width, height) / 2f;
        return PointInsideBox(center - halfBox, center + halfBox);
    }

    /// <summary>
    /// Get a random normalized direction vector
    /// </summary>
    /// <returns>Normalized direction</returns>
    public static Vector2 Direction()
    {
        return Random.insideUnitCircle.normalized;
    }

    /// <summary>
    /// Returns a random point inside a sphere of Radius <paramref name="radius"/>, around <paramref name="center"/>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector3 PointInsideSphere(Vector3 center, float radius)
    {
        return center + Random.insideUnitSphere * radius;
    }

    /// <summary>
    /// Returns a random point inside a sphere of Max Radius <paramref name="radius"/> at least <paramref name="innerRadius"/> distance away from its <paramref name="center"/>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="innerRadius"></param>
    /// <returns></returns>
    public static Vector3 PointInsideSphere(Vector3 center, float radius, float innerRadius)
    {
        float distanceToCenter = Random.Range(innerRadius, radius);
        return center + Random.insideUnitSphere.normalized * distanceToCenter;
    }

    /// <summary>
    /// Random point on a sphere's surface
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector3 PointOnSphere(Vector3 center, float radius)
    {
        return center + Random.insideUnitSphere.normalized * radius;
    }
}
