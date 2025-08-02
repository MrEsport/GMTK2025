using System;
using UnityEngine;

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
    public bool isPerfect { get; private set; }

    public SmokePoint validatingPoint { get; private set; }

    public event Action<bool> OnValidate = _ => { };
    public event Action OnDestroy = () => { };

    public SmokePointTarget(Vector2 position)
    {
        this.position = position;
        isValid = false;
        isPerfect = false;
    }

    public void SetValid(SmokePoint point, bool isPerfect = false)
    {
        validatingPoint = point;

        isValid = true;
        this.isPerfect = isPerfect;

        OnValidate(true);
    }

    public void Dispose()
    {
        OnDestroy();
    }
}
