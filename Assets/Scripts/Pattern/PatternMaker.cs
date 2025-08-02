using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternMaker : MonoBehaviour
{
    private enum PatternMode { STRAIGHT, BEZIER, CIRCLE };

    [SerializeField, BoxGroup("Components"), Required] GameStats stats;
    [SerializeField, BoxGroup("Components"), Required] PatternHolder patternHolder;

    [SerializeField, BoxGroup("Handles"), Required] public Transform startHandle;
    [SerializeField, BoxGroup("Handles"), Required] public Transform endHandle;
    [SerializeField, BoxGroup("Handles"), Required] public Transform startTangent;
    [SerializeField, BoxGroup("Handles"), Required] public Transform endTangent;

    [SerializeField, BoxGroup] PatternMode mode;
    [SerializeField, Range(0f, 2f), BoxGroup] float pointsAmountFactor;
    [SerializeField, Range(0f, 1f), BoxGroup, ShowIf(nameof(UseCircle))] float circleArcThreshold = 1f;

    [SerializeField, BoxGroup, ShowIf(nameof(StartedPatternPositions))] bool trimPointsByDistance = false;
    [SerializeField, Range(0f, 2f), BoxGroup, ShowIf(EConditionOperator.And, nameof(StartedPatternPositions), nameof(trimPointsByDistance))] float trimRangeFactor;

    private List<Vector2> patternPositions = new List<Vector2>();

    private Vector2[] pointsPositions;

    private bool RequiredComponentsValid { get => stats != null && startHandle != null && endHandle != null; }
    private bool StartedPatternPositions { get => patternPositions.Count > 0; }
    private bool UseBezier { get => mode == PatternMode.BEZIER; }
    private bool UseCircle { get => mode == PatternMode.CIRCLE; }

    private void OnDrawGizmos()
    {
        if (StartedPatternPositions)
            patternPositions.ForEach(p =>
            {
                Gizmos.color = ColorExtension.darkRed;
                Gizmos.DrawWireSphere(p, stats.Score.smokeValidRange / 2f);
                Gizmos.color = ColorExtension.orange;
                Gizmos.DrawSphere(p, .15f);
                Gizmos.DrawWireSphere(p, stats.Score.smokePerfectRange / 2f);
            });

        if (!RequiredComponentsValid) return;

        Gizmos.color = ColorExtension.darkBlue;
        Gizmos.DrawSphere(endHandle.position, .33f);
        Gizmos.color = ColorExtension.teal;
        Gizmos.DrawSphere(startHandle.position, .33f);

        var direction = endHandle.position - startHandle.position;
        int N = Mathf.FloorToInt(direction.magnitude * pointsAmountFactor / stats.smokeDropDistance);
        direction = direction.normalized * stats.smokeDropDistance;

        pointsPositions = mode switch
        {
            PatternMode.STRAIGHT => GetPositionsBetween(startHandle.position, endHandle.position, N),
            PatternMode.BEZIER => CurveExtension.Bezier2D(startHandle.position, startTangent.position, endTangent.position, endHandle.position, N),
            PatternMode.CIRCLE => CurveExtension.Circle2D(startHandle.position, endHandle.position, N * 2 - 2, circleArcThreshold),
            _ => throw new System.NotImplementedException()
        };

        if (StartedPatternPositions && trimPointsByDistance)
            pointsPositions = TrimPointsByDistance(pointsPositions);

        Vector2 p;
        for (int i = 0; i < pointsPositions.Length; ++i)
        {
            p = pointsPositions[i];

            if (i != 0)
                Debug.DrawLine(pointsPositions[i - 1], p, Color.yellow);

            Gizmos.color = ColorExtension.lightRed;
            Gizmos.DrawWireSphere(p, stats.Score.smokeValidRange / 2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(p, .15f);
            Gizmos.DrawWireSphere(p, stats.Score.smokePerfectRange / 2f);
        }
    }

    private Vector2[] GetPositionsBetween(Vector2 start, Vector2 end, int points)
    {
        var direction = end - start;

        Vector2[] positions = new Vector2[points];

        for (int i = 0; i < points; ++i)
            positions[i] = start + ((float)i / (points - 1) * direction);

        return positions;
    }

    private Vector2[] TrimPointsByDistance(Vector2[] points)
    {
        return points.Where(point => !patternPositions.Any(pos => (point - pos).sqrMagnitude < stats.Score.smokeValidRange * trimRangeFactor)).ToArray();
    }

    [Button("Add Positions"), ShowIf(nameof(RequiredComponentsValid))]
    private void AddPosition()
    {
        patternPositions.AddRange(pointsPositions);
    }

    [Button("Clear Current Positions"), ShowIf(nameof(StartedPatternPositions))]
    private void ClearPositions() => patternPositions.Clear();


    [Button("Make Pattern"), ShowIf(nameof(StartedPatternPositions))]
    private void ValidatePattern()
    {
        patternHolder.AddPattern(patternPositions.ToArray());
    }
}
