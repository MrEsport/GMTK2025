using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

using Random = UnityEngine.Random;

public class SmokeManager : MonoBehaviour
{
    private static SmokeManager instance;
    public static SmokeManager Instance { get => instance; }

    [SerializeField] GameObject smokeTargetPrefab;

    private List<SmokePoint> smokeParticles = new();
    [SerializeField] private List<Vector2> targetsPositions = new();
    private List<SmokePointTarget> smokePointTargets = new();

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
        if (smokeParticles.Count <= 1) return;

        foreach (var point in smokeParticles)
        {
            if (!smokePointTargets.Any(t => !t.isValid))
            {
                // ALL TARGETS VALID : RESET TARGETS
                ResetTargets();
                break;
            }

            var closePoints = smokePointTargets.Where(t => !t.isValid && Vector2.SqrMagnitude(t.position - point.position) <= .25f * .25f);
            if (closePoints.Count() <= 0) continue;

            closePoints.First().SetValid(point);
        }

        foreach (var target in smokePointTargets.Where(t => t.isValid))
        {
            DebugExtension.DrawCircle(target.validatingPoint.position, .25f, 10, ColorExtension.red);
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

    private void GeneratePointTargets()
    {
        foreach (var target in smokePointTargets)
            target.Dispose();
        smokePointTargets.Clear();
        targetsPositions.Clear();

        int N = Random.Range(4, 8);

        for (; N > 0; --N)
        {
            targetsPositions.Add(RandomExtension.PointInsideBox(Vector2.zero, 10f));
        }

        smokePointTargets = targetsPositions.Select(p => new SmokePointTarget(p)).ToList();

        smokePointTargets.ForEach(t =>
        {
            Instantiate(smokeTargetPrefab).GetComponent<SmokeTargetHandler>().Init(t);
        });
    }

    private void ResetTargets()
    {
        ClearSmoke();
        GeneratePointTargets();
    }

    private void OnDrawGizmos()
    {
        if (smokePointTargets.Count <= 0) return;
        Color pastColor = Gizmos.color;
        foreach (var target in smokePointTargets)
        {
            Gizmos.color = target.isValid ? Color.green : ColorExtension.orange;
            Gizmos.DrawWireSphere(target.position, .25f);
        }
        Gizmos.color = pastColor;
    }

    private void OnGUI()
    {
        int validTargets = smokePointTargets.Count(t => t.isValid);
        GUI.TextField(new Rect(0, 0, 500, 25), $"Targets : {validTargets} / {smokePointTargets.Count()}");

        if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 25), "RESET TARGETS"))
            ResetTargets();
        if (GUI.Button(new Rect(Screen.width - 200, 28, 200, 25), "CLEAR SMOKE"))
            ClearSmoke();
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
