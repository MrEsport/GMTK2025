using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

using Random = UnityEngine.Random;

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

    private Coroutine endingRoutine = null;
    private bool isEnding = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        GeneratePointTargets();
    }

    private void Update()
    {
        if (isEnding) return;
        if (targetsPositions.Length <= 0 || smokeParticles.Count <= 1) return;

        foreach (var point in smokeParticles)
        {
            if (!smokePointTargets.Any(t => !t.isValid))
            {
                // ALL TARGETS VALID : RESET TARGETS
                endingRoutine = StartCoroutine(WaitReset(2.5f));
                break;
            }

            var closePoints = smokePointTargets.Where(t => !t.isValid && Vector2.SqrMagnitude(t.position - point.position) <= Mathf.Pow(stats.Score.smokeValidRange / 2f, 2));
            if (closePoints.Count() <= 0) continue;

            closePoints.First().SetValid(point);
        }

        foreach (var target in smokePointTargets.Where(t => t.isValid))
        {
            DebugExtension.DrawCircle(target.validatingPoint.position, stats.Score.smokeValidRange / 2f, 10, ColorExtension.red);
            Debug.DrawLine(target.validatingPoint.position, target.position, ColorExtension.darkRed);
        }
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

    private void DestroyCallback(GameObject obj)
    {
        Destroy(obj);
    }

    private void ClearSmoke()
    {
        foreach (var smoke in smokeParticles)
            smoke.Dispose();

        smokeParticles.Clear();
    }

    private void ClearTargets()
    {
        foreach (var target in smokePointTargets)
            target.Dispose();
        smokePointTargets.Clear();
        targetsPositions = new Vector2[0];
    }

    private void GeneratePointTargets()
    {
        targetsPositions = patternHolder.Library.Random().patternPositions;

        smokePointTargets = targetsPositions.Select(p => new SmokePointTarget(p)).ToList();

        smokePointTargets.ForEach(t =>
        {
            Instantiate(smokeTargetPrefab).GetComponent<SmokeTargetHandler>().Init(t);
        });
    }

    private IEnumerator WaitReset(float waitTime)
    {
        isEnding = true;
        ClearTargets();
        
        yield return new WaitForSeconds(waitTime);

        ClearSmoke();
        GeneratePointTargets();
        endingRoutine = null;
        isEnding = false;
    }

    private void OnDrawGizmos()
    {
        if (smokePointTargets.Count <= 0) return;
        Color pastColor = Gizmos.color;
        foreach (var target in smokePointTargets)
        {
            Gizmos.color = target.isValid ? Color.green : ColorExtension.orange;
            Gizmos.DrawWireSphere(target.position, stats.Score.smokeValidRange / 2f);
        }
        Gizmos.color = pastColor;
    }

    private void OnGUI()
    {
        int validTargets = smokePointTargets.Count(t => t.isValid);
        GUI.TextField(new Rect(0, 0, 250, 30), $"Targets : {validTargets} / {smokePointTargets.Count()}");

        if (isEnding)
            GUI.TextField(new Rect(Screen.width / 2f - 250f / 2f, Screen.height - 32, 250, 35), $"Nice Job ! Resetting ...");

        if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 30), "RESET TARGETS"))
        {
            ClearSmoke();
            ClearTargets();
            GeneratePointTargets();
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

public class SmokePoint : IDisposable
{
    public Vector2 position { get; private set; }
    
    private Action destroyCallback;

    public SmokePoint(GameObject particle, Action destroyCallback)
    {
        position = particle.transform.position;
        this.destroyCallback = destroyCallback;
    }

    public void Dispose()
    {
        if (destroyCallback == null) return;
        destroyCallback();
    }
}

public class SmokePointTarget : IDisposable
{
    public Vector2 position { get; private set; }
    public bool isValid { get; private set; }

    public SmokePoint validatingPoint { get; private set; }

    public event Action<bool> OnValidate = _ => { };
    public event Action OnDestroy = () => { };

    public SmokePointTarget(Vector2 position)
    {
        this.position = position;
        isValid = false;
    }

    public void SetValid(SmokePoint point)
    {
        validatingPoint = point;
        isValid = true;
        OnValidate(true);
    }

    public void Dispose()
    {
        OnDestroy();
    }
}
