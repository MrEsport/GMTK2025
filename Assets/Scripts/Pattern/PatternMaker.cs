using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class PatternMaker : MonoBehaviour
{
    [SerializeField, Required] GameStats stats;
    [SerializeField, Required] Transform startHandle;
    [SerializeField, Required] Transform endHandle;

    private List<Vector2> patternPositions = new List<Vector2>();

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
        Debug.DrawLine(startHandle.position, endHandle.position, Color.yellow);

        var direction = endHandle.position - startHandle.position;
        int N = Mathf.FloorToInt(direction.magnitude / stats.smokeDropDistance);
        direction = direction.normalized * stats.smokeDropDistance;

        Gizmos.color = Color.yellow;

        var positions = GetPositionsBetween(startHandle.position, endHandle.position);

        Vector2 p;
        for (int i = 0; i < positions.Length; ++i)
        {
            p = positions[i];
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

    [Button("Add Positions"), ShowIf(nameof(RequiredComponentsValid))]
    private void AddPosition()
    {
        if (!RequiredComponentsValid)
        {
            Debug.LogError($"Missing Components in PatternMaker !");
            return;
        }

        patternPositions.AddRange(GetPositionsBetween(startHandle.position, endHandle.position));
    }

    [Button("Clear Current Positions"), ShowIf(nameof(StartedPatternPositions))]
    private void ClearPositions() => patternPositions.Clear();
}
