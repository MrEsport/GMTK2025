using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float inbetweenPatternDuration = 2.5f;

    private bool isEnding = false;

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
        Debug.Log($"Validated Pattern");
        EndSequence(inbetweenPatternDuration);
    }

    private async void EndSequence(float waitTime)
    {
        Debug.Log("WAIT RESET ... WAITING FOR SCORE ...");

        await ScoreManager.Instance.RegisterPointsScore(SmokeManager.Instance.GetSmokePoints());

        Debug.Log("SCORE REGISTERED");
        isEnding = true;

        SmokeManager.Instance?.ClearTargets();

        await Task.Delay(Mathf.RoundToInt(waitTime * 1000));

        SmokeManager.Instance?.ClearSmoke();
        SmokeManager.Instance?.GeneratePointTargets();
        isEnding = false;
    }

    private void OnGUI()
    {
        if (isEnding) return;
        GUI.TextField(new Rect(Screen.width / 2f - 250f / 2f, Screen.height - 32, 250, 35), $"Nice Job ! Resetting ...");
    }
}
