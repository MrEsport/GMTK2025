using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmokeManager : MonoBehaviour
{
    private static SmokeManager instance;
    public static SmokeManager Instance { get => instance; }

    private List<GameObject> smokeParticles = new();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void OnClearSmokeInputReceived(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        foreach (var particle in smokeParticles)
            Destroy(particle.gameObject);

        smokeParticles.Clear();
    }

    public void RegisterSmoke(GameObject particle)
    {
        smokeParticles.Add(particle);
    }
}
