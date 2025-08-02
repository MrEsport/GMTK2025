using UnityEngine;

public class SmokeTargetHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Color invalideColor;
    [SerializeField] Color validColor;

    public void Init(SmokePointTarget target)
    {
        transform.position = target.position;
        target.OnValidate += OnTargetValidate;
        target.OnDestroy += () => Destroy(gameObject);
        OnTargetValidate(false);
    }

    private void OnTargetValidate(bool isValid)
    {
        sr.color = isValid ? validColor : invalideColor;
    }
}
