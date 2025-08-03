using System;
using System.Threading.Tasks;
using UnityEngine;

public class PopupUIHandler : MonoBehaviour
{
    [SerializeField] bool startActiveState;

    private void Start()
    {
        gameObject.SetActive(startActiveState);
    }

    public void DelayHide(float delayInSeconds)
    {
        DelayAction(delayInSeconds, Hide);
    }

    private void Hide() => gameObject.SetActive(false);

    private async void DelayAction(float delayInSeconds, Action action)
    {
        await Task.Delay(Mathf.RoundToInt(delayInSeconds * 1000));
        action();
    }
}
