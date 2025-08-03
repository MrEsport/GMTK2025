using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmokeManager : MonoBehaviour
{
    private static SmokeManager instance;
    public static SmokeManager Instance { get => instance; }

    [SerializeField, Required] GameObject smokeTargetPrefab;
    [SerializeField, Required] GameStats stats;
    [SerializeField, Required] PatternHolder patternHolder;

    private List<SmokePoint> smokeParticles = new();
    [SerializeField] private Vector2[] targetsPositions;
    private List<SmokePointTarget> smokePointTargets = new();

    private Queue<string> patternNames;
    private bool isPatternCompleted = false;

    public event Action OnPatternValidated = () => { };
    public event Action OnPatternQueueEmptied = () => { };

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if (isPatternCompleted) return;
        if (targetsPositions.Length <= 0 || smokeParticles.Count <= 1) return;

        UpdateSmokePointsStatus();
    }

    public void OnClearSmokeInputReceived(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        ClearSmoke();
    }

    public void RegisterSmoke(GameObject particle)
    {
        var point = new SmokePoint(particle, () => DestroyCallback(particle));
        smokeParticles.Add(point);
    }

    public void GetAvailablePatterns()
    {
        patternNames = new Queue<string>(patternHolder.GetKeys().Shuffle());
    }

    public void GeneratePointTargets()
    {
        if (patternNames.Count == 0)
        {
            OnPatternQueueEmptied.Invoke();
            return;
        }

        SetTargets(patternHolder[patternNames.Dequeue()]);
        isPatternCompleted = false;
    }

    public SmokePointTarget[] GetSmokePoints() => smokePointTargets.ToArray();

    public void ClearSmoke()
    {
        foreach (var smoke in smokeParticles)
            smoke.Dispose();

        smokeParticles.Clear();
    }

    public void ClearTargets()
    {
        foreach (var target in smokePointTargets)
            target.Dispose();
        smokePointTargets.Clear();
        targetsPositions = new Vector2[0];
    }

    private void DestroyCallback(GameObject obj)
    {
        Destroy(obj);
    }

    private void UpdateSmokePointsStatus()
    {
        foreach (var point in smokeParticles)
        {
            if (!smokePointTargets.Any(t => !t.isValid))
            {
                // ALL TARGETS VALID : RESET TARGETS
                isPatternCompleted = true;
                OnPatternValidated.Invoke();
                break;
            }

            var closePoints = smokePointTargets.Where(t => !t.isValid && Vector2.SqrMagnitude(t.position - point.position) <= Mathf.Pow(stats.Score.smokeValidRange / 2f, 2));
            if (closePoints.Count() <= 0) continue;

            closePoints.First().SetValid(point, Vector2.SqrMagnitude(point.position - closePoints.First().position) <= Mathf.Pow(stats.Score.smokePerfectRange / 2f, 2));
        }
    }

    private void SetTargets(Vector2[] positions)
    {
        targetsPositions = positions;
        smokePointTargets = targetsPositions.Select(p => new SmokePointTarget(p)).ToList();
        smokePointTargets.ForEach(t =>
        {
            Instantiate(smokeTargetPrefab).GetComponent<SmokeTargetHandler>().Init(t);
        });
    }

    private void OnDrawGizmos()
    {
        if (smokePointTargets.Count <= 0) return;
        foreach (var target in smokePointTargets)
        {
            if (!target.isValid) continue;
            Debug.DrawLine(target.position, target.validatingPoint.position, target.isPerfect ? ColorExtension.lime : ColorExtension.lightRed);
        }
    }

    private void OnGUI()
    {
        int validTargets = smokePointTargets.Count(t => t.isValid);
        GUI.TextField(new Rect(0, 0, 250, 30), $"Targets : {validTargets} / {smokePointTargets.Count()}");

        if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 30), "RESET TARGETS"))
        {
            ClearSmoke();
            ClearTargets();
            SetTargets(patternHolder.Library.Random().patternPositions);
        }
        if (GUI.Button(new Rect(Screen.width - 200, 32, 200, 30), "CLEAR SMOKE"))
            ClearSmoke();

        if (GUI.Button(new Rect(Screen.width - 222, Screen.height - 32, 220, 30), "~I JUST WANT TO DRAW~"))
        {
            ClearSmoke();
            ClearTargets();
        }
    }
}
