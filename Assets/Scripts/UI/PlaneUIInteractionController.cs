using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneUIInteractionController : MonoBehaviour
{
    [SerializeField] float uiCastRadius = 1f;

    public void OnUIInteractInputReceived(InputAction.CallbackContext context)
    {
        if (context.canceled) return;

        TryInteract();
    }

    private void TryInteract()
    {
        if (!TryCastForUIInteract(out var hit, out var handler)) return;

        handler.Interact();
    }

    private bool TryCastForUIInteract(out RaycastHit2D hit, out UIInteractionHandler handler)
    {
        handler = null;
        hit = Physics2D.CircleCast(transform.position, uiCastRadius, Vector2.zero);
        return hit && hit.collider.TryGetComponent(out handler);
    }
}
