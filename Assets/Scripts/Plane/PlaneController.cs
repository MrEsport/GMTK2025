using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneController : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private GameStats stats;

    [SerializeField, Range(-20f, 20f)] float turnSpeed;

    private Vector2 moveDirection = Vector2.up;

    private void Update()
    {
        CheckForPositionOutsideScreen();
    }

    private void FixedUpdate()
    {
        float moveSpeed = stats.Plane.moveSpeed;

        moveDirection = Quaternion.AngleAxis(turnSpeed * moveSpeed * Time.fixedDeltaTime, Vector3.back) * moveDirection;

        spriteTransform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);

        transform.Translate(moveSpeed * Time.fixedDeltaTime * moveDirection);

        Debug.DrawRay(transform.position, moveDirection * 2.5f, Color.magenta);
        Debug.DrawRay(transform.position, transform.up * 2f, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 2f, Color.red);
    }

    public void OnMoveInputReceived(InputAction.CallbackContext context)
    {
        turnSpeed = context.performed ? context.ReadValue<Vector2>().x * stats.Plane.turnSpeedMax : 0f;
    }

    public void OnRespawnInputReceived(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        transform.position = Vector3.zero;
    }

    private void CheckForPositionOutsideScreen()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 screenPosRatio = screenPos / Camera.main.pixelRect.size;
        Vector2 directionFromCenter = (screenPosRatio - Vector2.one * .5f) * 2f;

        Vector2 flipFactor = new Vector2(
            Mathf.Clamp(1.03f - Mathf.Abs(directionFromCenter.x), -1f, 0f),
            Mathf.Clamp(1.05f - Mathf.Abs(directionFromCenter.y), -1f, 0f));

        if (flipFactor.x >= 0f && flipFactor.y >= 0f) return;

        Vector2 dotVector = moveDirection * directionFromCenter;
        FlipDirection(dotVector * flipFactor);
    }

    private void FlipDirection(Vector2 flip)
    {
        flip.x = Mathf.Sign(flip.x);
        flip.y = Mathf.Sign(flip.y);
        moveDirection *= flip;
    }
}
