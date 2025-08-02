using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonInteractionHandler : MonoBehaviour
{
    [SerializeField, Required] Button button;
    [SerializeField, Required] new BoxCollider2D collider;

    private void Start()
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        collider.size = rect.sizeDelta;
    }

    public void PressButton() => button.onClick.Invoke();
}
