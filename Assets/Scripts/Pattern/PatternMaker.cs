using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternMaker : MonoBehaviour
{
    [SerializeField, BoxGroup("Components"), Required] GameStats stats;
    [SerializeField, BoxGroup("Components"), Required] PatternHolder patternHolder;

    [SerializeField, BoxGroup("Handles"), Required] public Transform startHandle;
    [SerializeField, BoxGroup("Handles"), Required] public Transform endHandle;
    [SerializeField, BoxGroup("Handles"), Required] public Transform startTangent;
    [SerializeField, BoxGroup("Handles"), Required] public Transform endTangent;

    [SerializeField, BoxGroup("Bezier")] bool useBezier = false;
    [SerializeField, Range(0f, 2f), BoxGroup("Bezier"), ShowIf(nameof(useBezier))] float bezierPointsAmountFactor;

    [SerializeField, BoxGroup, ShowIf(nameof(StartedPatternPositions))] bool trimPointsByDistance = false;
    [SerializeField, Range(0f, 2f), BoxGroup, ShowIf(EConditionOperator.And, nameof(StartedPatternPositions), nameof(trimPointsByDistance))] float trimRangeFactor;

    private List<Vector2> patternPositions = new List<Vector2>();

    private Vector2[] pointsPositions;

    private bool RequiredComponentsValid { get => stats != null && startHandle != null && endHandle != null; }
    private bool StartedPatternPositions { get => patternPositions.Count > 0; }

    private void OnDrawGizmos()
    {
        if (StartedPatternPositions)
        {
            Gizmos.color = ColorExtension.orange;
            patternPositions.ForEach(p =>
            {
                Gizmos.DrawSphere(p, .15f);
                Gizmos.DrawWireSphere(p, stats.Score.smokeValidRange / 2f);
            });
        }

        if (!RequiredComponentsValid) return;

        Gizmos.color = ColorExtension.cyan;
        Gizmos.DrawSphere(endHandle.position, .33f);
        Gizmos.color = ColorExtension.teal;
        Gizmos.DrawSphere(startHandle.position, .33f);

        var direction = endHandle.position - startHandle.position;
        int N = Mathf.FloorToInt(direction.magnitude * (useBezier ? bezierPointsAmountFactor : 1f) / stats.smokeDropDistance);
        direction = direction.normalized * stats.smokeDropDistance;

        Gizmos.color = Color.yellow;

        pointsPositions = useBezier ?
            CurveExtension.Bezier2D(startHandle.position, startTangent.position, endTangent.position, endHandle.position, N):
            GetPositionsBetween(startHandle.position, endHandle.position);

        if (StartedPatternPositions && trimPointsByDistance)
            pointsPositions = TrimPointsByDistance(pointsPositions);

        Vector2 p;
        for (int i = 0; i < pointsPositions.Length; ++i)
        {
            p = pointsPositions[i];

            if (i != 0)
                Debug.DrawLine(pointsPositions[i - 1], p, Color.yellow);

            Gizmos.DrawSphere(p, .15f);
            Gizmos.DrawWireSphere(p, stats.Score.smokeValidRange / 2f);
        }
    }

    private Vector2[] GetPositionsBetween(Vector2 start, Vector2 end)
    {
        var direction = end - start;
        int N = Mathf.FloorToInt(direction.magnitude / stats.smokeDropDistance);
        direction = direction.normalized * stats.smokeDropDistance;

        Vector2[] positions = new Vector2[N + 1];

        for (int i = 0; i <= N; ++i)
            positions[i] = start + (direction * i);

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
