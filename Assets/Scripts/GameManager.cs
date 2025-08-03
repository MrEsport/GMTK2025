using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float inbetweenPatternDuration = 2.5f;

    private bool isEnding = false;
    private bool theEnd = false;

    private void Start()
    {
        SmokeManager.Instance.OnPatternValidated += PatternValidated;
        SmokeManager.Instance.OnPatternQueueEmptied += PatternQueueCompleted;

        SmokeManager.Instance.GetAvailablePatterns();

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
        NextPatternSequence(inbetweenPatternDuration);
    }

    private async void NextPatternSequence(float waitTime)
    {
        await ScoreManager.Instance.RegisterPointsScore(SmokeManager.Instance.GetSmokePoints());

        isEnding = true;

        SmokeManager.Instance?.ClearTargets();

        await Task.Delay(Mathf.RoundToInt(waitTime * 1000));

        SmokeManager.Instance?.ClearSmoke();
        SmokeManager.Instance?.GeneratePointTargets();
        isEnding = false;
    }

    private void PatternQueueCompleted()
    {
        theEnd = true;
    }

    private void OnGUI()
    {
        if (theEnd)
        {
            GUI.TextField(new Rect(Screen.width / 2f - 250f, Screen.height / 2f - 45, 500f, 43f), $"You Completed Every Pattern Request !\nYour Score is {ScoreManager.Instance.Score}");
            if (GUI.Button(new Rect(Screen.width / 2f - 250f / 2f, Screen.height / 2f + 2, 200f, 35f), "Back To Menu"))
                SceneManager.Instance?.LoadMenu();
        }

        if (isEnding)
            GUI.TextField(new Rect(Screen.width / 2f - 125f, Screen.height - 32f, 250f, 35f), $"Nice Job ! Resetting ...");
    }
}
