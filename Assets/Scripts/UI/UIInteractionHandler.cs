using UnityEngine;
using UnityEngine.Events;

public class UIInteractionHandler : MonoBehaviour
{
    [SerializeField] UnityEvent OnInteract;

    public void Interact()
    {
        OnInteract.Invoke();
    }
}
