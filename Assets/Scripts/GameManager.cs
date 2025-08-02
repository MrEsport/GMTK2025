using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float inbetweenPatternDuration = 2.5f;

    private Coroutine endingRoutine = null;

    private void Start()
    {
        SmokeManager.Instance.OnPatternValidated += PatternValidated;

        NextPattern();
    }

    private void OnDestroy()
    {
        if (SmokeManager.Instance == null) return;
        SmokeManager.Instance.OnPatternValidated -= PatternValidated; 
    }

    private void NextPattern()
    {
        SmokeManager.Instance.GeneratePointTargets();
    }

    private void PatternValidated()
    {
        if (endingRoutine != null) return;

        endingRoutine = StartCoroutine(WaitReset(inbetweenPatternDuration));
    }

    private IEnumerator WaitReset(float waitTime)
    {
        SmokeManager.Instance?.ClearTargets();

        yield return new WaitForSeconds(waitTime);

        SmokeManager.Instance?.ClearSmoke();
        SmokeManager.Instance?.GeneratePointTargets();
        endingRoutine = null;
    }

    private void OnGUI()
    {
        if (endingRoutine == null) return;
        GUI.TextField(new Rect(Screen.width / 2f - 250f / 2f, Screen.height - 32, 250, 35), $"Nice Job ! Resetting ...");
    }
}
