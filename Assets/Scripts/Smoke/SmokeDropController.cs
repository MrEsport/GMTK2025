using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmokeDropController : MonoBehaviour
{
    [SerializeField] GameObject smokeParticle;
    [SerializeField] GameStats stats;

    [SerializeField] bool infiniteReserve = false;

    private bool isDroppingInput = false;
    [SerializeField] private float reserveAmount;

    private Coroutine smokeDropRoutine = null;

    private void Start()
    {
        reserveAmount = stats.SmokeDrop.reserveMaxAmount;
    }

    public void OnDropSmokeInputReceived(InputAction.CallbackContext context)
    {
        if (isDroppingInput == (context.started || context.performed)) return;

        if (context.canceled)
        {
            isDroppingInput = false;
            if (smokeDropRoutine != null)
            {
                StopCoroutine(smokeDropRoutine);
                smokeDropRoutine = null;
            }
        }
        else
        {
            isDroppingInput = true;
            if (smokeDropRoutine != null)
            {
                StopCoroutine(smokeDropRoutine);
                smokeDropRoutine = null;
            }
            smokeDropRoutine = StartCoroutine(SmokeDropRoutine());
        }
    }

    public void OnInfiniteReserveInputReceived(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        infiniteReserve = !infiniteReserve;
        Debug.LogWarning($"Infinite Smoke Reserve {(infiniteReserve ? "ON" : "OFF")}");
    }

    private IEnumerator SmokeDropRoutine()
    {
        while (isDroppingInput && (reserveAmount > 0f || infiniteReserve))
        {
            DropSmoke();
            yield return new WaitForSeconds(1f / stats.SmokeDrop.reserveDropPerSecond);
        }

        smokeDropRoutine = null;
    }

    private void DropSmoke()
    {
        reserveAmount = Mathf.Max(reserveAmount - stats.SmokeDrop.reserveDropAmount, 0f);
        var part = Instantiate(smokeParticle, transform.position, Quaternion.identity);
        SmokeManager.Instance.RegisterSmoke(part);
    }
}
