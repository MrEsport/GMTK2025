using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;

    [SerializeField] float moveSpeed = 6;
    [SerializeField, Range(-20f, 20f)] float turnSpeed;

    private Vector2 moveDirection = Vector2.up;

    private void FixedUpdate()
    {
        moveDirection = Quaternion.AngleAxis(turnSpeed * moveSpeed * Time.fixedDeltaTime, Vector3.back) * moveDirection;

        spriteTransform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);

        transform.Translate(moveSpeed * Time.fixedDeltaTime * moveDirection);

        Debug.DrawRay(transform.position, moveDirection * 2.5f, Color.magenta);
        Debug.DrawRay(transform.position, transform.up * 2f, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 2f, Color.red);
    }
}
